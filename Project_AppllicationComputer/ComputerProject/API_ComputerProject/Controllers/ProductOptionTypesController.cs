using ComputerSales.Application.UseCase.ProductOptionalType_UC;
using ComputerSales.Application.UseCaseDTO.ProductOptionalType_DTO;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProductOptionTypesController : ControllerBase
{
    private readonly CreateProductOptionalType_UC _create;
    private readonly GetByIdProductOptionalType_UC _get;
    private readonly UpdateProductOptionalType_UC _update;
    private readonly DeleteProductOptionalType_UC _delete;

    public ProductOptionTypesController(
        CreateProductOptionalType_UC create,
        GetByIdProductOptionalType_UC get,
        UpdateProductOptionalType_UC update,
        DeleteProductOptionalType_UC delete)
    {
        _create = create;
        _get = get;
        _update = update;
        _delete = delete;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProducyOptionalTypeInput req, CancellationToken ct)
    {
        var result = await _create.HandleAsync(req, ct);
        return result is null ? BadRequest() : Ok(result);
    }

    [HttpGet("{productId:long}/{optionTypeId:int}")]
    public async Task<IActionResult> Get(long productId, int optionTypeId, CancellationToken ct)
    {
        var result = await _get.HandleAsync(new ProducyOptionalTypeInput(productId, optionTypeId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProducyOptionalTypeInput req, CancellationToken ct)
    {
        var result = await _update.HandleAsync(req, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{productId:long}/{optionTypeId:int}")]
    public async Task<IActionResult> Delete(long productId, int optionTypeId, CancellationToken ct)
    {
        var result = await _delete.HandleAsync(new ProducyOptionalTypeInput(productId, optionTypeId), ct);
        return result is null ? NotFound() : Ok(result);
    }
}
