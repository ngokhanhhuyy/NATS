using UAParser;

namespace NATS.Services;

public class TrafficService : ITrafficService
{
    private readonly DatabaseContext _context;
    
    public TrafficService(DatabaseContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Get today's traffic statistics which contains recorded date as today, access count and guess count.
    /// </summary>
    /// <returns>An object containing the statistics data.</returns>
    public async Task<ServiceResult<TrafficStatisticsByDateResponseDto>> GetTodayStatisticsAsync()
    {
        TrafficByDate trafficByDate = await _context.TrafficByDates
            .SingleAsync(td => td.RecordedDateTime.Date == DateTime.Today);
        
        TrafficStatisticsByDateResponseDto responseDto;
        responseDto = new TrafficStatisticsByDateResponseDto
        {
            RecordedDate = trafficByDate.RecordedDateTime,
            AccessCount = trafficByDate.AccessCount,
            GuessCount = trafficByDate.GuestCount
        };
          
        return ServiceResult<TrafficStatisticsByDateResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Get the traffic statistics over last specific number of days (including today).
    /// </summary>
    /// <param name="lastDays">The number of the last specific days.</param>
    /// <returns>A list of objects containing the statistics data.</returns>
    public async Task<ServiceResult<List<TrafficStatisticsByDateResponseDto>>> GetStatisticsByDateRangeAsync(
            int lastDays)
    {
        List<TrafficByDate> trafficsByDates = await _context.TrafficByDates
            .Where(td => 
                td.RecordedDateTime.Date > DateTime.Today.Date.AddDays(-lastDays) &&
                td.RecordedDateTime.Date <= DateTime.Today.Date)
            .OrderBy(td => td.RecordedDateTime)
            .ToListAsync();

        List<TrafficStatisticsByDateResponseDto> responseDtos = trafficsByDates
            .Select(td => new TrafficStatisticsByDateResponseDto
            {
                RecordedDate = td.RecordedDateTime.Date,
                AccessCount = td.AccessCount,
                GuessCount = td.GuestCount
            }).ToList();

        return ServiceResult<List<TrafficStatisticsByDateResponseDto>>.Success(responseDtos);
    }

    /// <summary>
    /// Get the traffic average statistics over last specific number of days (including today)
    /// by hour range (morning, noon, afternoon, evening, night).
    /// </summary>
    /// <param name="lastDays">The number of the last specific days.</param>
    /// <returns>A list of objects containing the statistics data.</returns>
    public async Task<ServiceResult<List<TrafficStatisticsByHourRangeResponseDto>>> GetStatisticsByHourRangeAsync(
            int lastDays)
    {
        List<TrafficByHour> trafficByHours = await _context.TrafficByHours
            .Where(th => 
                th.RecordedDateTime.Date > DateTime.Today.Date.AddDays(-lastDays) &&
                th.RecordedDateTime.Date <= DateTime.Today.Date)
            .OrderBy(th => th.RecordedDateTime)
            .ToListAsync();

        List<TrafficStatisticsByHourRangeResponseDto> responseDtos;
        responseDtos = new List<TrafficStatisticsByHourRangeResponseDto>();
        List<(string, int, int)> hoursForSessions = new()
        {
            ("Sáng sớm", 4, 7),
            ("Buổi sáng", 7, 11),
            ("Buổi trưa", 11, 13),
            ("Buổi chiều", 13, 17),
            ("Buổi tối", 17, 23),
            ("Ban đêm", 23, 4),
        };
        foreach ((string Name, int FromHour, int ToHour) session in hoursForSessions)
        {
            responseDtos.Add(new TrafficStatisticsByHourRangeResponseDto
            {
                Name = session.Name,
                FromTime = new TimeOnly(session.FromHour, 0, 0),
                ToTime = new TimeOnly(session.ToHour, 0, 0),
                AccessCount = trafficByHours
                    .Where(th =>
                        th.RecordedDateTime.Hour >= session.FromHour &&
                        th.RecordedDateTime.Hour < session.ToHour)
                    .Sum(th => th.AccessCount),
                GuessCount = trafficByHours
                    .Where(th =>
                        th.RecordedDateTime.Hour >= session.FromHour &&
                        th.RecordedDateTime.Hour < session.ToHour)
                    .Sum(th => th.GuestCount),
            });
        }

        return ServiceResult<List<TrafficStatisticsByHourRangeResponseDto>>.Success(responseDtos);
    }

    /// <summary>
    /// Get the traffic statistics by device in the specified number of the last days.
    /// </summary>
    /// <param name="lastDays">The number of the last specific days.</param>
    /// <returns>A list of objects containing the statistics data.</returns>
    public async Task<ServiceResult<List<TrafficStatisticsByDeviceResponseDto>>> GetStatisticsByDeviceAsync(
            int lastDays)
    {
        List<TrafficByHour> trafficByHours;
        trafficByHours = await _context.TrafficByHours
            .Include(td => td.IPAddresses)
            .Where(td =>
                td.RecordedDateTime.Date > DateTime.Today.AddDays(-lastDays) &&
                td.RecordedDateTime.Date <= DateTime.Today)
            .ToListAsync();
        List<TrafficStatisticsByDeviceResponseDto> responseDtos;
        responseDtos = new List<TrafficStatisticsByDeviceResponseDto>();
        foreach (TrafficByHour trafficByHour in trafficByHours)
        {
            foreach (TrafficByHourIpAddress trafficIpAddress in trafficByHour.IPAddresses)
            {
                Parser parser = Parser.GetDefault();
                ClientInfo clientInfo = parser.Parse(trafficIpAddress.LastUserAgent);
                TrafficStatisticsByDeviceResponseDto responseDto;
                responseDto = responseDtos
                    .SingleOrDefault(dto => dto.DeviceName == clientInfo.OS.Family);
                if (responseDto == null)
                {
                    responseDto = new TrafficStatisticsByDeviceResponseDto
                    {
                        DeviceName = clientInfo.OS.Family
                    };
                    responseDtos.Add(responseDto);
                }
                responseDto.AccessCount += 1;
            }
        }

        return ServiceResult<List<TrafficStatisticsByDeviceResponseDto>>.Success(responseDtos);
    }

    /// <summary>
    /// Record the IP address of the current request by hour.
    /// </summary>
    /// <param name="ipAddress">The IP address of the current request.</param>
    /// <param name="userAgent">The User-Agent header of the request.</param>
    /// <param name="pathFirstSegment">The first segment of the url path where the request is sent to.</param>
    /// <returns>The id of the traffic by hour.</returns>
    public async Task RecordAsync(string ipAddress, string userAgent)
    {
        // Fetch the traffic entity from the database.
        TrafficByDate trafficByDate = await _context.TrafficByDates
            .Include(td => td.TrafficByHours)
            .ThenInclude(th => th.IPAddresses)
            .Where(td => td.RecordedDateTime.Date == DateTime.Today)
            .SingleAsync();

        // Fetch current hour's traffic by hour entity
        TrafficByHour trafficByHour = trafficByDate.TrafficByHours
            .Single(th => th.RecordedDateTime.Hour == DateTime.Now.Hour);
        
        // Assign a list if traffic ip address list in the traffic entity is null.
        if (trafficByHour.IPAddresses == null)
        {
            trafficByHour.IPAddresses = new List<TrafficByHourIpAddress>();
        }
        
        // Fetch traffic ip address.
        TrafficByHourIpAddress trafficIPAddress = trafficByHour.IPAddresses!
            .SingleOrDefault(tia => tia.IPAddress == ipAddress);
        
        // Create new traffic ip address entity if it doesn't exist.
        if (trafficIPAddress == null)
        {
            trafficIPAddress = new TrafficByHourIpAddress
            {
                IPAddress = ipAddress,
            };
            trafficByHour.IPAddresses!.Add(trafficIPAddress);
            trafficByHour.GuestCount += 1;
            bool ipAddressRecorded = trafficByDate.TrafficByHours
                .Any(th => th.IPAddresses
                    .Any(ip => ip.IPAddress == ipAddress));
            if (!ipAddressRecorded)
            {
                trafficByDate.GuestCount += 1;
            }
        }
        
        // Update the entities.
        trafficIPAddress.AccessCount += 1;
        trafficIPAddress.LastAccessAt = DateTime.Now;
        trafficIPAddress.LastUserAgent = userAgent;
        trafficByHour.AccessCount += 1;
        trafficByDate.AccessCount += 1;
        
        // Save changes.
        await _context.SaveChangesAsync();
    }
}