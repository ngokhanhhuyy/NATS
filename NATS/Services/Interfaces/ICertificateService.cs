namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle certificate-related operations.
/// </summary>
public interface ICertificateService
{
    /// <summary>
    /// Gets a list of all certificates.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs, containing the information of the certificates.
    /// </returns>
    Task<List<CertificateResponseDto>> GetListAsync();

    /// <summary>
    /// Gets a single existing certificate by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the certificate to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is A DTO,
    /// containing the information of the certificate.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the certificate specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task<CertificateResponseDto> GetSingleAsync(int id);

    /// <summary>
    /// Creates a new certificate.
    /// </summary>
    /// <param name="upsertRequestDto">
    /// A DTO containing for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the id
    /// of the created certificate.
    /// </returns>
    Task<int> CreateAsync(CertificateUpsertRequestDto upsertRequestDto);

    /// <summary>
    /// Updates an existing certificate, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the certificate to update.
    /// </param>
    /// <param name="upsertRequestDto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the certificate specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception>
    Task UpdateAsync(int id, CertificateUpsertRequestDto upsertRequestDto);

    /// <summary>
    /// Deletes an existing certificate, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the certificate to delete.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the certificate specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception>
    Task DeleteAsync(int id);
}