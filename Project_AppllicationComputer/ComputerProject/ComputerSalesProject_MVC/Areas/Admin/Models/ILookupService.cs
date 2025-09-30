namespace ComputerSalesProject_MVC.Areas.Admin.Models;

public interface ILookupService
{
    Task<List<(long Id, string Name)>> GetAccessoriesAsync(CancellationToken ct);
    Task<List<(long Id, string Name)>> GetProvidersAsync(CancellationToken ct);
    Task<List<(int Id, string Name)>> GetOptionTypesAsync(CancellationToken ct);
}
