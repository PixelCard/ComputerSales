//using ComputerSales.Application.UseCase.AccountBlockDTO;
//using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO;
//using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO.DeleteAccountBlock;
//using ComputerSales.Application.UseCaseDTO.AccountBlock_DTO.GetAccountBlock;
//using Microsoft.AspNetCore.Mvc;

//namespace API_ComputerProject.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccountBlocksController : ControllerBase
//    {
//        private readonly CreateAccountBlock_UC _create;
//        private readonly getAccountBlockByID_UC _getById;
//        private readonly DeleteAccountBlock_UC _delete;

//        public AccountBlocksController(
//            CreateAccountBlock_UC create,
//            getAccountBlockByID_UC getById,
//            DeleteAccountBlock_UC delete)
//        {
//            _create = create;
//            _getById = getById;
//            _delete = delete;
//        }

//        // POST: api/accountblocks
//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] AccountBlockInputDTO req, CancellationToken ct)
//        {
//            if (req == null || req.IDAccount <= 0)
//                return BadRequest("Invalid request data.");

//            var result = await _create.HandleAsync(req, ct);
//            return CreatedAtAction(nameof(GetById), new { id = result.IdBlock }, result);
//        }

//        // GET: api/accountblocks/{id}
//        [HttpGet("{id:int}")]
 
//        public async Task<IActionResult> GetById(int id, CancellationToken ct)
//        {
//            var result = await _getById.HandleAsync(new GetAccountBlockByID_InputDTO(id), ct);
//            return result is null ? NotFound() : Ok(result);
//        }

//        [HttpDelete("{id:int}")]
//        public async Task<IActionResult> Delete(int id, CancellationToken ct)
//        {
//            var ok = await _delete.HandleAsync(new DeleteAccountBlockInput_DTO(id), ct);
//            return ok ? NoContent() : NotFound();
//        }

//    }
//}
