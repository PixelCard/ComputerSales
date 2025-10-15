namespace ComputerSales.Application.UseCaseDTO.AccountBlock_DTO
{
    public sealed record AccountBlockOutputDTO(
        int IdBlock,
        int IDAccount,
        DateTime BlockFromUtc,
        DateTime? BlockToUtc,
        bool IsBlock,          // ← trạng thái hiện tại (true = đang bị khóa)
        string ReasonBlock,
        bool IsActiveNowUtc);  // ← true nếu nằm trong khoảng thời gian khóa);
}
