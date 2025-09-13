using ComputerSales.Application.UseCaseDTO.Account_DTO.UpdateAccount;
using ComputerSales.Domain.Entity;

namespace ComputerSales.Application.UseCaseDTO.Account_DTO
{
    public static class AccountMapping
    {
        public static Account ToEntity(this AccountDTOInput input)
        {
            return Account.Create(
                input.Email,
                input.Pass,
                input.IDRole
            );
        }

        public static AccountOutputDTO ToResult(this Account e)
        {
            return new AccountOutputDTO(
                e.IDAccount,
                e.Email,
                e.IDRole,
                e.Role?.TenRole ?? "" // lấy TenRole từ Role
            );
        }

        public static void ApplyUpdate(this Account e, UpdateAccountDTO dto)
        {
            e.Email = dto.Email;
            e.Pass = dto.Pass;
            e.IDRole = dto.IDRole;
        }
    }
}
