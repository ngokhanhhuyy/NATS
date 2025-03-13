namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle traffic-related operations.
/// </summary>
public interface ITrafficService
{
    /// <summary>
    /// Gets today's traffic statistics which contains recorded date as today, access count and
    /// guest count.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the statistics data.
    /// </returns>
    Task<TrafficByDateResponseDto> GetTodayTrafficAsync();

    /// <summary>
    /// Gets the traffic average statistics over last specific number of days (including today)
    /// by hour range (morning, noon, afternoon, evening, night).
    /// </summary>
    /// <param name="lastDays">
    /// The number of the last specific days.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs containing the statistics data.
    /// </returns>
    Task<List<TrafficByDateResponseDto>> GetTrafficByDateRangeAsync(int lastDays);

    /// <summary>
    /// Gets the traffic statistics over last specific number of days (including today).
    /// </summary>
    /// <param name="lastDays">
    /// The number of the last specific days.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a list
    /// of DTOs containing the statistics data.
    /// </returns>
    Task<List<TrafficByHourRangeResponseDto>> GetTrafficByHourRangeAsync(int lastDays);

    /// <summary>
    /// Get the traffic statistics by device in the specified number of the last days.
    /// </summary>
    /// <param name="lastDays">
    /// The number of the last specific days.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a list
    /// of DTOs containing the statistics data.
    /// </returns>
    Task<List<TrafficByDeviceResponseDto>> GetStatsByDeviceAsync(int lastDays);

    /// <summary>
    /// Records the IP address of the current request by hour.
    /// </summary>
    /// <param name="ipAddress">
    /// The IP address of the current request.
    /// </param>
    /// <param name="userAgent">
    /// The User-Agent header of the request.
    /// </param>
    /// <param name="pathFirstSegment">
    /// The first segment of the url path where the request is sent to.
    /// </param>
    /// <returns>
    /// The id of the traffic by hour.
    /// </returns>
    Task RecordAsync(string ipAddress, string userAgent);
}