namespace NATS.Services.Interfaces;

public interface ITrafficService
{
     Task<ServiceResult<TrafficStatisticsByDateResponseDto>> GetTodayStatisticsAsync();

     Task<ServiceResult<List<TrafficStatisticsByHourRangeResponseDto>>> GetStatisticsByHourRangeAsync(
          int lastDays);

     Task<ServiceResult<List<TrafficStatisticsByDateResponseDto>>> GetStatisticsByDateRangeAsync(
          int lastDays);

     Task<ServiceResult<List<TrafficStatisticsByDeviceResponseDto>>> GetStatisticsByDeviceAsync(
          int lastDays);

     Task RecordAsync(string ipAddress, string userAgent);
}