using Aqua360.Models;

namespace Aqua360.Services;

public interface ICustomersService
{
    Task<CustomerPage> GetPageAsync(string? search, bool? isActive, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<CustomerSummary> GetSummaryAsync(CancellationToken cancellationToken = default);
    Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task SaveAsync(int? id, CustomerSaveRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
