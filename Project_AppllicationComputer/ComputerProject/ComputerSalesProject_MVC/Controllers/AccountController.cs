using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSalesProject_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSalesProject_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly CreateAccount_UC _createAccount;

        public AccountController(CreateAccount_UC createAccount)
        {
            _createAccount = createAccount;
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
        public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var input = new AccountDTOInput(
                    Email: model.Email,
                    Pass: model.Password,
                    IDRole: model.RoleId //Auto là Customer Role
                );

                var result = await _createAccount.HandleAsync(input, ct);

                TempData["RegisterSuccess"] = $"Đăng ký thành công cho {result.Email}";

                return RedirectToAction(nameof(Register));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }
    }
}
