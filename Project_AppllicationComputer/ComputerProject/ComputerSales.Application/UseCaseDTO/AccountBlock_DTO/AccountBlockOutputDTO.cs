namespace ComputerSales.Application.UseCaseDTO.AccountBlock_DTO
{
    public sealed record AccountBlockOutputDTO(
        int BlockId,
        int IDAccount,
        DateTime BlockFromUtc,
        DateTime? BlockToUtc,
        bool IsBlock,           // trạng thái lưu trong DB (có thể do job nền cập nhật)
        string ReasonBlock,
        bool IsActiveNowUtc     // tính động từ from/to
    ); // ← true nếu nằm trong khoảng thời gian khóa);
}
