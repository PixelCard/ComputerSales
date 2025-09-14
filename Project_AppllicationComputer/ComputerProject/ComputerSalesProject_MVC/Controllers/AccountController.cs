using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.Account_DTO.GetAccountByEmail;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using ComputerSalesProject_MVC.Models;
using Microsoft.AspNetCore.Mvc;
namespace ComputerSalesProject_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly CreateAccount_UC _createAccount;

        private readonly CreateCustomer_UC createCustomer_UC;

        private readonly GetAccountByEmail_UC _getAccount;

        private readonly IUnitOfWorkApplication _uow;

        public AccountController(
            CreateAccount_UC createAccount, 
            CreateCustomer_UC _createCustomer_UC,
            GetAccountByEmail_UC getAccount,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _createAccount = createAccount;
            createCustomer_UC = _createCustomer_UC;
            _getAccount = getAccount;
            _uow = unitOfWorkApplication;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm, CancellationToken ct)
        {
            // Map VM -> DTO Account
            var accountInput = new AccountDTOInput(
                Email: vm.Email,
                Pass: vm.PasswordHash,      
                IDRole: 1      
            );

            await _uow.BeginTransactionAsync(ct);

            try
            {
                // 1) Tạo ACCOUNT -> lấy IDAccount
                var accountOut = await _createAccount.HandleAsync(accountInput, ct);

                // 2) Tạo CUSTOMER gắn với IDAccount vừa có
                var customerInput = new CustomerInputDTO(
                    IMG: null,                                  
                    Name: vm.UserName,
                    Description: vm.Description_User,
                    Date: vm.Date ?? DateTime.Today,
                    IDAccount: accountOut.IDAccount             
                );

                await createCustomer_UC.HandleAsync(customerInput, ct);

                await _uow.CommitAsync(ct);

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync(ct);

                ViewBag.Error = "Đăng ký thất bại: " + ex.Message;

                return View(vm);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm,CancellationToken ct)
        {
            var inputDTO = new getAccountByEmailInput(vm.Email);


            var accounts = _getAccount.HandleAsync(inputDTO, ct);

            try
            {
                return RedirectToAction("Index", "Home");
            }

            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }
    }
}




