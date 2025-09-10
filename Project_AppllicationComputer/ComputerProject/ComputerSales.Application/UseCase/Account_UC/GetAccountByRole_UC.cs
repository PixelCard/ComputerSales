//// ComputerSales.Application/UseCase/Account_UC/GetAccountByRole_UC.cs
//using ComputerSales.Application.Interface.Account_Interface;
//using ComputerSales.Application.UseCaseDTO.Account_DTO;
//using ComputerSales.Application.UseCaseDTO.Account_DTO.GetAccountByRole;

//namespace ComputerSales.Application.UseCase.Account_UC
//{
//    public class GetAccountByRole_UC
//    {
//        private readonly IAccountRepository _repo;
//        public GetAccountByRole_UC(IAccountRepository repo) => _repo = repo;

//        public async Task<List<AccountOutputDTO>> HandleAsync(GetAccountByRoleOutputDTO input, CancellationToken ct = default)
//        {
//            var entities = await _repo.GetAccountByRole(input.IDRole, ct);
//            return entities.(e => e.ToResult()).ToList();
//        }
//    }
//}
