using ComputerSales.Application.UseCase.Product.GetByID;
using ComputerSales.Application.UseCaseDTO.Product.GetByID;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly GetProductByIdUseCase _useCase;
        public ProductController(GetProductByIdUseCase useCase) => _useCase = useCase;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var dto = await _useCase.ExcuteAsync(new GetProductByIdInput(id), ct);
            return dto is null ? NotFound() : Ok(dto);
        }
    }
}
