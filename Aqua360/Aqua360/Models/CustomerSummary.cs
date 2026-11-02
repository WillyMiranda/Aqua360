namespace Aqua360.Models;

public sealed record CustomerSummary
{
    public int Total { get; init; }
    public int Active { get; init; }
    public int Inactive { get; init; }
}
