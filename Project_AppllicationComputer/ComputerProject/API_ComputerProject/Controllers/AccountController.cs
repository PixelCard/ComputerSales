using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.Account_DTO.DeleteAccount;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using ComputerSales.Application.UseCaseDTO.Account_DTO.GetAccountByID;
using ComputerSales.Application.UseCaseDTO.Account_DTO.UpdateAccount;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly CreateAccount_UC _create;
        private readonly GetAccount_UC _get;
        private readonly UpdateAccount_UC _update;
        private readonly DeleteAccount_UC _delete;

        public AccountsController(
            CreateAccount_UC create,
            GetAccount_UC get,
            UpdateAccount_UC update,
            DeleteAccount_UC delete)
        {
            _create = create;
            _get = get;
            _update = update;
            _delete = delete;
        }

        // POST: api/accounts
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountDTOInput req, CancellationToken ct)
        {
            if (req is null) return BadRequest();
            if (string.IsNullOrWhiteSpace(req.Email)) return BadRequest("Email is required.");
            if (string.IsNullOrWhiteSpace(req.Pass)) return BadRequest("Pass is required.");
            if (req.IDRole <= 0) return BadRequest("IDRole is invalid.");

            var result = await _create.HandleAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.IDAccount }, result);
        }

        // GET: api/accounts/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int IDAccount, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(new getAccountByID(IDAccount), ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        // PUT: api/accounts/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int IDAccount, [FromBody] UpdateAccountDTO body, CancellationToken ct)
        {
            if (body is null) return BadRequest();
            //if (IDAccount != body) return BadRequest("Mismatched id.");
            if (string.IsNullOrWhiteSpace(body.Email)) return BadRequest("Email is required.");
            if (string.IsNullOrWhiteSpace(body.Pass)) return BadRequest("Pass is required.");
            if (body.IDRole <= 0) return BadRequest("IDRole is invalid.");

            var rs = await _update.HandleAsync(body, ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        // DELETE: api/accounts/5?rowVersionBase64=xxxx
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await _delete.HandleAsync(new DeleteAccountOutputDTO(id), ct);
            return ok ? NoContent() : NotFound();
        }
    }
}
