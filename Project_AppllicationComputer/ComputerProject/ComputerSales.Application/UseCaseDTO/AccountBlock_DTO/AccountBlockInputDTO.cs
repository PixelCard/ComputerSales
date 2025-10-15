namespace ComputerSales.Application.UseCaseDTO.AccountBlock_DTO
{
    public sealed record AccountBlockInputDTO(
        int IDAccount,
    DateTime BlockFromUtc,
    DateTime? BlockToUtc,
    string ReasonBlock);
}
