using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO;
using ComputerSales.Domain.Entity.EAccount;

public static class AccountBlockMapping
{
    // InputDTO -> Entity (CREATE)
    public static AccountBlock ToEntity(this AccountBlockInputDTO input)
    {
        var fromUtc = DateTime.SpecifyKind(input.BlockFromUtc, DateTimeKind.Utc);
        DateTime? toUtc = input.BlockToUtc.HasValue
            ? DateTime.SpecifyKind(input.BlockToUtc.Value, DateTimeKind.Utc)
            : null;

        // Create() đã tự UpdateStatusByTime()
        return AccountBlock.Create(
            input.IDAccount,
            fromUtc,
            toUtc,
            input.ReasonBlock?.Trim() ?? string.Empty
        );
    }

    // Entity -> OutputDTO (READ) — KHÔNG mutate entity ở đây
    public static AccountBlockOutputDTO ToResult(this AccountBlock e)
    {
        var now = DateTime.UtcNow;
        bool activeNow = now >= e.BlockFromUtc &&
                         (!e.BlockToUtc.HasValue || now < e.BlockToUtc.Value);

        return new AccountBlockOutputDTO(
            e.BlockId,
            e.IDAccount,
            e.BlockFromUtc,
            e.BlockToUtc,
            e.IsBlock,                 // cờ lưu trong DB (lần cập nhật gần nhất)
            e.ReasonBlock ?? string.Empty,
            activeNow                  // trạng thái động theo đồng hồ
        );
    }
}
