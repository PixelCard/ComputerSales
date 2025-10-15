using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO;
using ComputerSales.Domain.Entity.EAccount;

public static class AccountBlockMapping
{
    // InputDTO -> Entity
    public static AccountBlock ToEntity(this AccountBlockInputDTO input)
    {
        var e = AccountBlock.Create(
            input.IDAccount,
            input.BlockFromUtc,
            input.BlockToUtc,
            input.ReasonBlock ?? string.Empty
        );

        e.UpdateStatusByTime(); // tự cập nhật Active/Inactive dựa vào ngày
        return e;
    }

    // Entity -> OutputDTO
    public static AccountBlockOutputDTO ToResult(this AccountBlock e)
    {
        e.UpdateStatusByTime(); // đảm bảo trạng thái mới nhất

        bool isBlock = e.IsBlock == true;


        return new AccountBlockOutputDTO(
            e.IdBlock,
            e.IDAccount,
            e.BlockFromUtc,
            e.BlockToUtc,
            isBlock,
            e.ReasonBlock ?? string.Empty,
            e.IsActiveNowUtc
        );
    }
}
