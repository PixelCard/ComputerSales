namespace ComputerSales.Application.UseCaseDTO.AccountBlock_DTO.UpdateAccountBlock
{
    public sealed record UpdateAccountBlock_DTO(
        int AccountID,
        DateTime BlockFromUtc,
        DateTime? BlockToUtc,               // true: khóa, false: không khóa
        string? ReasonBlock
    );
}
