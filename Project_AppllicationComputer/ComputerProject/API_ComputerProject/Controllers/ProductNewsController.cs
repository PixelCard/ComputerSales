using ComputerSales.Application.UseCase.ProductNews_UC;
using ComputerSales.Application.UseCaseDTO.ProductNews_DTO;
using Microsoft.AspNetCore.Mvc;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductNewsController : ControllerBase
    {
        private readonly CreateProductNews_UC _create;
        private readonly GetByIdProductNews_UC _get;
        private readonly UpdateProductNews_UC _update;
        private readonly DeleteProductNews_UC _delete;

        public ProductNewsController(
            CreateProductNews_UC create,
            GetByIdProductNews_UC get,
            UpdateProductNews_UC update,
            DeleteProductNews_UC delete)
        {
            _create = create;
            _get = get;
            _update = update;
            _delete = delete;
        }

        //=========   CREATE   =========//
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductNewsInputDTO req, CancellationToken ct)
        {
            if (req is null) return BadRequest();
            if (string.IsNullOrWhiteSpace(req.BlockType))
                return BadRequest("BlockType is required.");

            var result = await _create.HandleAsync(req, ct);

            return CreatedAtAction(nameof(GetById), new { id = result.ProductNewsID }, result);
        }

        //=========   GET BY ID   =========//
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(id, ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        //=========   UPDATE   =========//
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductNewsOutputDTO body, CancellationToken ct)
        {
            if (body is null) return BadRequest();
            if (id != body.ProductNewsID) return BadRequest("Mismatched id.");
            if (string.IsNullOrWhiteSpace(body.BlockType))
                return BadRequest("BlockType is required.");

            var rs = await _update.HandleAsync(body, ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        //=========   DELETE   =========//
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var rs = await _delete.HandleAsync(id, ct);
            return rs is null ? NotFound() : Ok(rs);
        }
    }
}
