using ComputerSales.Application.UseCase.ProductOvetView_UC;
using ComputerSales.Application.UseCaseDTO.Product_DTO.DeleteProduct;
using ComputerSales.Application.UseCaseDTO.Product_DTO.GetByID;
using ComputerSales.Application.UseCaseDTO.Product_DTO.UpdateProduct;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.DeleteDTO;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.GetByIdDTO;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.UpdateDTO;
using ComputerSales.Domain.Entity.EProduct;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductOverViewsController : ControllerBase
    {
        private readonly CreateProductOverView_UC _CreateProductOverView_UC;

        private readonly UpdateProductOverView_UC updateProductOverView_UC;

        private readonly DeleteProductOverView_UC deleteProductOverView_UC;

        private readonly GetByIdProductOverView_UC getByIdProductOverView_UC;

        public ProductOverViewsController(CreateProductOverView_UC createProductOverView_UC, UpdateProductOverView_UC _updateProductOverView_UC, 
            DeleteProductOverView_UC _deleteProductOverView_UC, GetByIdProductOverView_UC _getByIdProductOverView_UC)
        {
            _CreateProductOverView_UC = createProductOverView_UC;
            updateProductOverView_UC = _updateProductOverView_UC;
            deleteProductOverView_UC = _deleteProductOverView_UC;
            getByIdProductOverView_UC=_getByIdProductOverView_UC;

        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductOverViewInput input,CancellationToken ct)
        {
            if(input.Caption == null || input.ImageUrl == null || input.TextContent == null)
            {
                return BadRequest("The Information about Caption Or ImgURl or TextContent need to required");
            }

            if(input.DisplayOrder <= 0)
            {
                return BadRequest("The Information about DisplayOrder for that input need to required and The DisplayOrder must be > 0");
            }

            var req = new ProductOverViewInput(
                input.ProductId,
                input.BlockType,
                input.TextContent,
                input.ImageUrl,
                input.Caption,
                input.DisplayOrder
            );

            var result = await _CreateProductOverView_UC.HandleAsync(input,ct);

            return CreatedAtAction(nameof(GetById), new { id = result.ProductOverviewId}, result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var rs = await getByIdProductOverView_UC.HandleAsync(new GetByIDProductOverViewInput(id), ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInputDTO body, CancellationToken ct)
        {
            if (id != body.ProductOverViewID) return BadRequest("Mismatched id.");
            var rs = await updateProductOverView_UC.HandleAsync(body, ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id,CancellationToken ct)
        {
            var ok = await deleteProductOverView_UC.HandleAsync(new DeleteProductOverViewInput(id),ct);
            return ok != null ? NoContent() : NotFound();
        }
    }
}
