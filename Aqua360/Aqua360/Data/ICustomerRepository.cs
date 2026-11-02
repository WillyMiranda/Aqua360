using Aqua360.Models;

namespace Aqua360.Data;

public interface ICustomerRepository
{
    Task<CustomerPage> GetPageAsync(string? search, bool? isActive, int page, int pageSize, CancellationToken cancellationToken);
    Task<CustomerSummary> GetSummaryAsync(CancellationToken cancellationToken);
    Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> CreateAsync(CustomerSaveRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(int id, CustomerSaveRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
