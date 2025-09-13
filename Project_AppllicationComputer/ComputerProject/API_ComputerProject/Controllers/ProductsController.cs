using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCaseDTO.Product_DTO;
using ComputerSales.Application.UseCaseDTO.Product_DTO.DeleteProduct;
using ComputerSales.Application.UseCaseDTO.Product_DTO.GetByID;
using ComputerSales.Application.UseCaseDTO.Product_DTO.UpdateProduct;
using Microsoft.AspNetCore.Mvc;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CreateProduct_UC _create;
        private readonly DeleteProduct_UC _delete;
        private readonly GetProduct_UC _get;
        private readonly UpdateProduct_UC _update;

        public ProductsController(CreateProduct_UC create,DeleteProduct_UC delete,UpdateProduct_UC update,GetProduct_UC getProduct)
        {
            _create = create;
            _delete = delete;
            _get = getProduct;
            _update = update;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTOInput req, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(req.ShortDescription))
                return BadRequest("ShortDescription is required.");

            var input = new ProductDTOInput(
                req.ShortDescription,
                req.Status,
                req.AccessoriesID,
                req.ProviderID,
                req.Slug,
                req.SKU
            );

            var result = await _create.HandleAsync(input, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.ProductID }, result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(new ProductGetByIDInput(id), ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateProductInput body, CancellationToken ct)
        {
            if (id != body.ProductID) return BadRequest("Mismatched id.");
            var rs = await _update.HandleAsync(body, ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, [FromQuery] string? rowVersionBase64, CancellationToken ct)
        {
            var ok = await _delete.HandleAsync(new DeleteProductInput(id, rowVersionBase64), ct);
            return ok ? NoContent() : NotFound();
        }
    }
}
