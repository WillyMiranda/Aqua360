using Aqua360.Models;
using Dapper;
using MySqlConnector;

namespace Aqua360.Data;

public sealed class CustomerRepository(string? connectionString) : ICustomerRepository
{
    private const string SelectColumns = "id AS Id, account_number AS AccountNumber, full_name AS FullName, identification AS Identification, address AS Address, phone AS Phone, email AS Email, is_active AS IsActive, created_at AS CreatedAt, updated_at AS UpdatedAt";

    public async Task<CustomerPage> GetPageAsync(string? search, bool? isActive, int page, int pageSize, CancellationToken cancellationToken)
    {
        const string where = "WHERE (@Search IS NULL OR account_number LIKE @Search OR full_name LIKE @Search OR identification LIKE @Search OR phone LIKE @Search OR email LIKE @Search) AND (@IsActive IS NULL OR is_active = @IsActive)";
        var parameters = new { Search = string.IsNullOrWhiteSpace(search) ? null : $"%{search.Trim()}%", IsActive = isActive };

        await using var connection = CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(new CommandDefinition(
            $"SELECT COUNT(*) FROM customers {where}", parameters, cancellationToken: cancellationToken));
        var items = (await connection.QueryAsync<Customer>(new CommandDefinition(
            $"SELECT {SelectColumns} FROM customers {where} ORDER BY full_name, id LIMIT @Limit OFFSET @Offset",
            new { parameters.Search, parameters.IsActive, Limit = pageSize, Offset = (page - 1) * pageSize },
            cancellationToken: cancellationToken))).AsList();

        return new CustomerPage(items, page, pageSize, totalCount);
    }

    public async Task<CustomerSummary> GetSummaryAsync(CancellationToken cancellationToken)
    {
        const string sql = "SELECT COUNT(*) AS Total, COALESCE(SUM(is_active = 1), 0) AS Active, COALESCE(SUM(is_active = 0), 0) AS Inactive FROM customers";
        await using var connection = CreateConnection();
        return await connection.QuerySingleAsync<CustomerSummary>(new CommandDefinition(sql, cancellationToken: cancellationToken));
    }

    public async Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        await using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Customer>(new CommandDefinition(
            $"SELECT {SelectColumns} FROM customers WHERE id = @Id", new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<int> CreateAsync(CustomerSaveRequest request, CancellationToken cancellationToken)
    {
        const string sql = "INSERT INTO customers (account_number, full_name, identification, address, phone, email, is_active) VALUES (@AccountNumber, @FullName, @Identification, @Address, @Phone, @Email, @IsActive); SELECT LAST_INSERT_ID();";
        await using var connection = CreateConnection();
        return await connection.ExecuteScalarAsync<int>(new CommandDefinition(sql, request, cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateAsync(int id, CustomerSaveRequest request, CancellationToken cancellationToken)
    {
        const string sql = "UPDATE customers SET account_number = @AccountNumber, full_name = @FullName, identification = @Identification, address = @Address, phone = @Phone, email = @Email, is_active = @IsActive, updated_at = UTC_TIMESTAMP(6) WHERE id = @Id";
        await using var connection = CreateConnection();
        return await connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            Id = id,
            request.AccountNumber,
            request.FullName,
            request.Identification,
            request.Address,
            request.Phone,
            request.Email,
            request.IsActive
        }, cancellationToken: cancellationToken)) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await using var connection = CreateConnection();
        return await connection.ExecuteAsync(new CommandDefinition(
            "DELETE FROM customers WHERE id = @Id", new { Id = id }, cancellationToken: cancellationToken)) > 0;
    }

    private MySqlConnection CreateConnection()
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Configure ConnectionStrings:MariaDb with .NET User Secrets before using customer data.");
        }

        return new MySqlConnection(connectionString);
    }
}
