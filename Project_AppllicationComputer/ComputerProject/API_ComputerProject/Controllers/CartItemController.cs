
using ComputerSales.Application.UseCaseDTO.CartItem_DTO;
using ComputerSales.Application.UseCaseDTO.CartItem_DTO.DeleteCartItem;
using ComputerSales.Application.UseCaseDTO.CartItem_DTO.GetCartItemById;
using ComputerSales.Application.UseCaseDTO.CartItem_DTO.UpdateCartItem;
using Microsoft.AspNetCore.Mvc;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly CreateCartItem_UC _create;
        private readonly GetCartItemById_UC _get;
        private readonly UpdateCartItem_UC _update;
        private readonly DeleteCartItem_UC _delete;

        public CartItemController(
            CreateCartItem_UC create,
            GetCartItemById_UC get,
            UpdateCartItem_UC update,
            DeleteCartItem_UC delete)
        {
            _create = create;
            _get = get;
            _update = update;
            _delete = delete;
        }

        //=========   CREATE   =========//
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CartItemInputDTO req, CancellationToken ct)
        {
            if (req is null) return BadRequest();

            var result = await _create.HandleAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.ID, cartId = result.CartID }, result);
        }

        //=========   GET BY ID   =========//
        [HttpGet("{id:int}/{cartId:int}")]
        public async Task<IActionResult> GetById(int id, int cartId, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(new InputGetCartItemByID(id, cartId), ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        //=========   UPDATE   =========//
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] InputUpdateCartItem body, CancellationToken ct)
        {
            if (body is null) return BadRequest();
            if (id != body.ID) return BadRequest("Mismatched id.");

            var rs = await _update.HandleAsync(body, ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        //=========   DELETE   =========//
        [HttpDelete("{id:int}/{cartId:int}")]
        public async Task<IActionResult> Delete(int id, int cartId, CancellationToken ct)
        {
            var ok = await _delete.HandleAsync(new InputDeleteCartItem(id, cartId), ct);
            return ok ? NoContent() : NotFound();
        }
    }
}
