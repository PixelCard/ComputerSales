using API_ComputerProject.DTO.ProductAPI_DTO;
using ComputerSales.Application.UseCase.ProductUC.CreateProduct;
using ComputerSales.Application.UseCase.ProductUC.GetByID;
using ComputerSales.Application.UseCaseDTO.ProductDTO.GetByID;
using API_ComputerProject.DTO.ProductAPI_DTO.CreateProduct;
using ComputerSales.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using static ComputerSales.Application.UseCaseDTO.ProductDTO.CreateProduct.CreateProductDTO;
using CreateProductResult = API_ComputerProject.DTO.ProductAPI_DTO.CreateProduct.CreateProductResult;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly GetProductByIdUseCase _useCaseGetProductByID;
        private readonly CreateProduct_UC _usecaseCreateProduct;
        public ProductController(GetProductByIdUseCase _useCaseGetProductByID, CreateProduct_UC _usecaseCreateProduct)
        {
            this._useCaseGetProductByID = _useCaseGetProductByID;
            this._usecaseCreateProduct  = _usecaseCreateProduct;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var dto = await _useCaseGetProductByID.ExcuteAsync(new GetProductByIdInput(id), ct);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest req,CancellationToken ct)
        {
            var result = await _usecaseCreateProduct.ExecuteAsync(new CreateProductInput(req.Name, req.Description), ct);
            var resp = new CreateProductResult { Id = result.Id };
            return CreatedAtAction(nameof(GetById), new { id = resp.Id }, resp);
        }
    }
}
