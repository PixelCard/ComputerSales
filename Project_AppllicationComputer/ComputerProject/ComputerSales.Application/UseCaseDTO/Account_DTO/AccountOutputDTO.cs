namespace ComputerSales.Application.UseCaseDTO.Account_DTO
{
    public sealed record AccountOutputDTO(int IDAccount, string Email, int IDRole, string TenRole);


    public class AccountOutputDTO2
    {
        public AccountOutputDTO2()
        {
        }

        public int IDAccount { get; set; }
        public string Email { get; set; }
        public int IDRole { get; set; }
        public string TenRole { get; set; }
    }
}
