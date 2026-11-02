using Aqua360.Data;
using Aqua360.Models;
using FluentValidation;
using FluentValidation.Results;
using MySqlConnector;

namespace Aqua360.Services;

public sealed class CustomersService(
    ICustomerRepository customers,
    IValidator<CustomerSaveRequest> validator) : ICustomersService
{
    public Task<CustomerPage> GetPageAsync(string? search, bool? isActive, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return customers.GetPageAsync(search, isActive, Math.Max(1, page), Math.Clamp(pageSize, 1, 100), cancellationToken);
    }

    public Task<CustomerSummary> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        return customers.GetSummaryAsync(cancellationToken);
    }

    public Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return customers.GetByIdAsync(id, cancellationToken);
    }

    public async Task SaveAsync(int? id, CustomerSaveRequest request, CancellationToken cancellationToken = default)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation.Errors);
        }

        try
        {
            if (id.HasValue)
            {
                if (!await customers.UpdateAsync(id.Value, request, cancellationToken))
                {
                    throw new KeyNotFoundException("Customer was not found.");
                }
            }
            else
            {
                await customers.CreateAsync(request, cancellationToken);
            }
        }
        catch (MySqlException exception) when (exception.Number == 1062)
        {
            throw new ValidationException(
            [new ValidationFailure(nameof(request.AccountNumber), "Account number already exists.")]);
        }
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return customers.DeleteAsync(id, cancellationToken);
    }
}
