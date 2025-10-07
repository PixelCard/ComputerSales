using ComputerSales.Application.UseCase.VariantOptionValue_UC;
using ComputerSales.Application.UseCaseDTO.VariantOptionValue_DTO;
using ComputerSales.Application.UseCaseDTO.VariantOptionValue_DTO.DeleteVariantOptionValue;
using ComputerSales.Application.UseCaseDTO.VariantOptionValue_DTO.getVariantOptionValue;
using Microsoft.AspNetCore.Mvc;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantOptionValuesController : ControllerBase
    {
        private readonly CreateVariantOptionValue_UC _create;
        private readonly GetByIdVariantOptionValue_UC _get;
        private readonly UpdateVariantOptionValue_UC _update;
        private readonly DeleteVariantOptionValue_UC _delete;

        public VariantOptionValuesController(
            CreateVariantOptionValue_UC create,
            GetByIdVariantOptionValue_UC get,
            UpdateVariantOptionValue_UC update,
            DeleteVariantOptionValue_UC delete)
        {
            _create = create;
            _get = get;
            _update = update;
            _delete = delete;
        }

        // POST: api/variantoptionvalues
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VariantOptionValueInput_DTO req, CancellationToken ct)
        {
            if (req == null) return BadRequest("Request is null.");
            var result = await _create.HandleAsync(req, ct);
            return result == null ? BadRequest("Create failed.") : Ok(result);
        }

        // GET: api/variantoptionvalues/{variantId}/{optionalValueId}
        [HttpGet("{variantId:int}/{optionalValueId:int}")]
        public async Task<IActionResult> Get(int variantId, int optionalValueId, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(new getVariantOptionValueByID_Input(variantId, optionalValueId), ct);
            return rs == null ? NotFound() : Ok(rs);
        }

        // PUT: api/variantoptionvalues
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] VariantOptionValueInput_DTO body, CancellationToken ct)
        {
            var rs = await _update.HandleAsync(body, ct);
            return rs == null ? NotFound() : Ok(rs);
        }

        // DELETE: api/variantoptionvalues/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await _delete.HandleAsync(new DeleteVariantOptionalValue_Input(id), ct);
            return ok ? NoContent() : NotFound();
        }
    }
}
