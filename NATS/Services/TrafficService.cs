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
            .SingleOrDefaultAsync(td => td.RecordedDate == today)
            ?? new TrafficByDateResponseDto(today);
    }

    /// <inheritdoc />
    public async Task<List<TrafficByDateResponseDto>> GetTrafficByDateRangeAsync(int lastDays)
    {
        DateOnly endingDate = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        DateOnly startingDate = endingDate.AddDays(-lastDays);

        // Fetch all traffic by date entities available from the database.
        List<TrafficByDate> trafficByDates = await _context.TrafficByDates
            .Where(td => td.RecordedDate > startingDate && td.RecordedDate <= endingDate)
            .OrderBy(td => td.RecordedDate)
            .ToListAsync();

        // Check and generate empty response DTOs if the corresponding entities don't exist.
        List<TrafficByDateResponseDto> responseDtos = new List<TrafficByDateResponseDto>();
        DateOnly evaluatingDate = startingDate;
        while (evaluatingDate <= endingDate)
        {
            TrafficByDateResponseDto responseDto = trafficByDates
                .Select(td => new TrafficByDateResponseDto(td))
                .FirstOrDefault(td => td.RecordedDate == evaluatingDate)
                ?? new TrafficByDateResponseDto(evaluatingDate);

            evaluatingDate = evaluatingDate.AddDays(1);
        }

        return responseDtos;
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
                GuestCount = trafficByHours
                    .Where(th =>
                        th.RecordedDateTime.Hour >= FromHour &&
                        th.RecordedDateTime.Hour < ToHour)
                    .Sum(th => th.GuestCount),
            });
        }

        return responseDtos;
    }

    /// <inheritdoc />
    public async Task<List<TrafficByDeviceResponseDto>> GetStatsByDeviceAsync(
            int lastDays)
    {
        List<TrafficByHour> trafficByHours;
        trafficByHours = await _context.TrafficByHours
            .Include(td => td.IpAddresses)
            .Where(td =>
                td.RecordedDateTime.Date > DateTime.Today.AddDays(-lastDays) &&
                td.RecordedDateTime.Date <= DateTime.Today)
            .ToListAsync();

        List<TrafficByDeviceResponseDto> responseDtos;
        responseDtos = new List<TrafficByDeviceResponseDto>();

        foreach (TrafficByHour trafficByHour in trafficByHours)
        {
            foreach (TrafficByHourIpAddress trafficIpAddress in trafficByHour.IpAddresses)
            {
                Parser parser = Parser.GetDefault();
                ClientInfo clientInfo = parser.Parse(trafficIpAddress.LastUserAgent);
                TrafficByDeviceResponseDto responseDto = responseDtos
                    .SingleOrDefault(dto => dto.DeviceName == clientInfo.OS.Family);

                if (responseDto == null)
                {
                    responseDto = new TrafficByDeviceResponseDto
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
        DateTime now = DateTime.UtcNow.ToApplicationTime();
        DateOnly today = DateOnly.FromDateTime(now);
        TrafficByDate trafficByDate = await _context.TrafficByDates
            .Include(td => td.TrafficByHours).ThenInclude(th => th.IpAddresses)
            .Where(td => td.RecordedDate == today)
            .SingleOrDefaultAsync();

        // Create a new entity if not exists.
        if (trafficByDate == null)
        {
            trafficByDate = new TrafficByDate
            {
                RecordedDate = today,
                TrafficByHours = new List<TrafficByHour>()
            };

            _context.TrafficByDates.Add(trafficByDate);
        }

        // Fetch current hour's traffic by hour entity.
        TrafficByHour trafficByHour = trafficByDate.TrafficByHours
            .SingleOrDefault(th => th.RecordedDateTime.Hour == DateTime.Now.Hour);
        
        // Create a new hour entity if not exists.
        if (trafficByHour == null)
        {
            trafficByHour = new TrafficByHour
            {
                RecordedDateTime = now,
                IpAddresses = new List<TrafficByHourIpAddress>()
            };

            trafficByDate.TrafficByHours.Add(trafficByHour);
            _context.TrafficByHours.Add(trafficByHour);
        }
        
        // Assign a list if traffic ip address list in the traffic entity is null.
        trafficByHour.IpAddresses ??= new List<TrafficByHourIpAddress>();
        
        // Fetch traffic ip address.
        TrafficByHourIpAddress trafficIpAddress = trafficByHour.IpAddresses
            .SingleOrDefault(tia => tia.IPAddress == ipAddress);
        
        // Create new traffic ip address entity if it doesn't exist.
        if (trafficIpAddress == null)
        {
            trafficIpAddress = new TrafficByHourIpAddress
            {
                IPAddress = ipAddress,
            };

            trafficByHour.IpAddresses.Add(trafficIpAddress);
            trafficByHour.GuestCount += 1;

            bool ipAddressRecorded = trafficByDate.TrafficByHours
                .Any(th => th.IpAddresses.Any(ip => ip.IPAddress == ipAddress));

            if (!ipAddressRecorded)
            {
                trafficByDate.GuestCount += 1;
            }
        }
        
        // Update the entities.
        trafficIpAddress.AccessCount += 1;
        trafficIpAddress.LastAccessAt = DateTime.Now;
        trafficIpAddress.LastUserAgent = userAgent;
        trafficByHour.AccessCount += 1;
        trafficByDate.AccessCount += 1;
        
        // Save changes.
        await _context.SaveChangesAsync();
    }
}