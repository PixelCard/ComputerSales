using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCaseDTO.Product_DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CreateProduct_UC _create;

        public ProductsController(CreateProduct_UC create)
        {
            _create = create;
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

        // stub để CreatedAtAction có điểm đến
        [HttpGet("{id:long}")]
        public IActionResult GetById([FromRoute] long id)
        {
            // Tuỳ bạn hiện thực sau
            return Ok(new { id });
        }
    }
}
