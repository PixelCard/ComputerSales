using ComputerSales.Application.UseCase.Cart_UC;
using ComputerSales.Application.UseCaseDTO.Cart_DTO;
using ComputerSales.Application.UseCaseDTO.Cart_DTO.DeleteCart;
using ComputerSales.Application.UseCaseDTO.Cart_DTO.GetCartById;
using ComputerSales.Application.UseCaseDTO.Cart_DTO.UpdateCart;
using Microsoft.AspNetCore.Mvc;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CreateCart_UC _create;
        private readonly GetCartById_UC _get;
        private readonly UpdateCart_UC _update;
        private readonly DeleteCart_UC _delete;

        public CartController(
            CreateCart_UC create,
            GetCartById_UC get,
            UpdateCart_UC update,
            DeleteCart_UC delete)
        {
            _create = create;
            _get = get;
            _update = update;
            _delete = delete;
        }

        //=========   CREATE   =========//
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CartInputDTO req, CancellationToken ct)
        {
            if (req is null) return BadRequest();

            var result = await _create.HandleAsync(req, ct);

            return CreatedAtAction(nameof(GetById), new { id = result.ID, userId = result.UserID }, result);
        }

        //=========   GET BY ID   =========//
        [HttpGet("{id:int}/{userId:int}")]
        public async Task<IActionResult> GetById(int id, int userId, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(new InputGetCartByID(id, userId), ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        //=========   UPDATE   =========//
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] InputUpdateCart body, CancellationToken ct)
        {
            if (body is null) return BadRequest();
            if (id != body.IDCart) return BadRequest("Mismatched id.");

            var rs = await _update.HandleAsync(body, ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        //=========   DELETE   =========//
        [HttpDelete("{id:int}/{userId:int}")]
        public async Task<IActionResult> Delete(int id, int userId, CancellationToken ct)
        {
            var ok = await _delete.HandleAsync(new InputDeleteCart(id, userId), ct);
            return ok ? NoContent() : NotFound();
        }
    }
}
