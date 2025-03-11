using NATS.Services.Dtos.ResponseDtos.Traffic;
using UAParser;

namespace NATS.Services;

/// <inheritdoc cref="ITrafficService" />
public class TrafficService : ITrafficService
{
    private readonly DatabaseContext _context;
    
    public TrafficService(DatabaseContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<TrafficByDateResponseDto> GetTodayTrafficAsync()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        return await _context.TrafficByDates
            .Select(td => new TrafficByDateResponseDto(td))
            .SingleAsync(td => td.RecordedDate == today);
    }

    /// <inheritdoc />
    public async Task<List<TrafficByDateResponseDto>> GetTrafficByDateRangeAsync(int lastDays)
    {
        DateOnly endingDate = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        DateOnly startingDate = endingDate.AddDays(-lastDays);

        return await _context.TrafficByDates
            .Where(td => td.RecordedDate > startingDate && td.RecordedDate <= endingDate)
            .OrderBy(td => td.RecordedDate)
            .Select(td => new TrafficByDateResponseDto(td))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<TrafficByHourRangeResponseDto>>
            GetTrafficByHourRangeAsync(int lastDays)
    {
        DateTime endingDateTime = DateTime.UtcNow.ToApplicationTime();
        DateTime startingDateTime = endingDateTime.AddDays(-lastDays);

        List<TrafficByHour> trafficByHours = await _context.TrafficByHours
            .Where(th => 
                th.RecordedDateTime > startingDateTime &&
                th.RecordedDateTime <= endingDateTime)
            .OrderBy(th => th.RecordedDateTime)
            .ToListAsync();

        List<TrafficByHourRangeResponseDto> responseDtos;
        responseDtos = new List<TrafficByHourRangeResponseDto>();
        List<(string, int, int)> periodsOfDay = new()
        {
            ("Sáng sớm", 4, 7),
            ("Buổi sáng", 7, 11),
            ("Buổi trưa", 11, 13),
            ("Buổi chiều", 13, 17),
            ("Buổi tối", 17, 23),
            ("Ban đêm", 23, 4),
        };

        foreach ((string Name, int FromHour, int ToHour) in periodsOfDay)
        {
            responseDtos.Add(new TrafficByHourRangeResponseDto
            {
                PeriodOfDayName = Name,
                FromTime = new TimeOnly(FromHour, 0, 0),
                ToTime = new TimeOnly(ToHour, 0, 0),
                AccessCount = trafficByHours
                    .Where(th =>
                        th.RecordedDateTime.Hour >= FromHour &&
                        th.RecordedDateTime.Hour < ToHour)
                    .Sum(th => th.AccessCount),
                GuessCount = trafficByHours
                    .Where(th =>
                        th.RecordedDateTime.Hour >= FromHour &&
                        th.RecordedDateTime.Hour < ToHour)
                    .Sum(th => th.GuestCount),
            });
        }

        return responseDtos;
    }

    /// <inheritdoc />
    public async Task<List<TrafficStatsByDeviceResponseDto>> GetStatsByDeviceAsync(
            int lastDays)
    {
        List<TrafficByHour> trafficByHours;
        trafficByHours = await _context.TrafficByHours
            .Include(td => td.IPAddresses)
            .Where(td =>
                td.RecordedDateTime.Date > DateTime.Today.AddDays(-lastDays) &&
                td.RecordedDateTime.Date <= DateTime.Today)
            .ToListAsync();

        List<TrafficStatsByDeviceResponseDto> responseDtos;
        responseDtos = new List<TrafficStatsByDeviceResponseDto>();

        foreach (TrafficByHour trafficByHour in trafficByHours)
        {
            foreach (TrafficByHourIpAddress trafficIpAddress in trafficByHour.IPAddresses)
            {
                Parser parser = Parser.GetDefault();
                ClientInfo clientInfo = parser.Parse(trafficIpAddress.LastUserAgent);
                TrafficStatsByDeviceResponseDto responseDto = responseDtos
                    .SingleOrDefault(dto => dto.DeviceName == clientInfo.OS.Family);

                if (responseDto == null)
                {
                    responseDto = new TrafficStatsByDeviceResponseDto
                    {
                        DeviceName = clientInfo.OS.Family
                    };
                    responseDtos.Add(responseDto);
                }

                responseDto.AccessCount += 1;
            }
        }

        return responseDtos;
    }

    /// <inheritdoc />
    public async Task RecordAsync(string ipAddress, string userAgent)
    {
        // Fetch the traffic entity from the database.
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        TrafficByDate trafficByDate = await _context.TrafficByDates
            .Include(td => td.TrafficByHours)
            .ThenInclude(th => th.IPAddresses)
            .Where(td => td.RecordedDate == today)
            .SingleAsync();

        // Fetch current hour's traffic by hour entity
        TrafficByHour trafficByHour = trafficByDate.TrafficByHours
            .Single(th => th.RecordedDateTime.Hour == DateTime.Now.Hour);
        
        // Assign a list if traffic ip address list in the traffic entity is null.
        trafficByHour.IPAddresses ??= new List<TrafficByHourIpAddress>();
        
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