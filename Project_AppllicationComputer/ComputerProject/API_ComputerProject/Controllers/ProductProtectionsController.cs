using ComputerSales.Application.UseCase.ProductProtection_UC;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO.DeleteDTO;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO.GetByIdDTO;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO.UpdateDTO;
using Microsoft.AspNetCore.Mvc;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductProtectionsController : ControllerBase
    {
        private readonly CreateProductProtection_UC createProductProtection_UC;
        private readonly DeleteProductProtection_UC deleteProductProtection_UC;
        private readonly GetByIdProductProtection_UC getByIdProductProtection_UC;
        private readonly UpdateProductProtection_UC updateProductProtection_UC;

        public ProductProtectionsController(CreateProductProtection_UC createProductProtection_UC, 
            DeleteProductProtection_UC deleteProductProtection_UC, 
            GetByIdProductProtection_UC getByIdProductProtection_UC, 
            UpdateProductProtection_UC updateProductProtection_UC)
        {
            this.createProductProtection_UC = createProductProtection_UC;
            this.deleteProductProtection_UC = deleteProductProtection_UC;
            this.getByIdProductProtection_UC = getByIdProductProtection_UC;
            this.updateProductProtection_UC = updateProductProtection_UC;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductProtectionInputcs input, CancellationToken ct)
        {
            if (input.DateEnd.ToString() == "string" || input.DateEnd.ToString() == "string"
                || input.DateEnd.ToString() == "" || input.DateEnd.ToString() == "")
            {
                return BadRequest("The Information about Caption Or ImgURl or TextContent need to required");
            }

            var req = new ProductProtectionInputcs(
                input.DateBuy,
                input.DateEnd,
                input.Status,
                input.ProductId
            );

            var result = await createProductProtection_UC.HandleAsync(input, ct);

            return CreatedAtAction(nameof(GetById), new { id = result.ProtectionProductId }, result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id, CancellationToken ct)
        {
            var rs = await getByIdProductProtection_UC.HandleAsync(new ProductProtectionGetByIDInput(id), ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] ProductProtectionUpdateInput body, CancellationToken ct)
        {
            if (id != body.ProtectionProductId) return BadRequest("Mismatched id.");
            var rs = await updateProductProtection_UC.HandleAsync(body, ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            var ok = await deleteProductProtection_UC.HandleAsync(new ProductProtectionDeleteInput(id), ct);
            return ok != null ? NoContent() : NotFound();
        }

    }
}
