using ComputerSales.Application.UseCase.VariantPrice_UC;
using ComputerSales.Application.UseCaseDTO.VariantPrice_DTO.VariantPriceInput_Output;
using Microsoft.AspNetCore.Mvc;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantPricesController : ControllerBase
    {
        private readonly CreateVariantPrice_UC _create;
        private readonly DeleteVariantPrice_UC _delete;
        private readonly GetByIdVariantPrice_UC _get;
        private readonly UpdateVariantPrice_UC _update;

        public VariantPricesController(
            CreateVariantPrice_UC create,
            DeleteVariantPrice_UC delete,
            UpdateVariantPrice_UC update,
            GetByIdVariantPrice_UC get)
        {
            _create = create;
            _delete = delete;
            _update = update;
            _get = get;
        }

        // POST: api/variantprices
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VariantPriceInputDTO req, CancellationToken ct)
        {
            if (req == null) return BadRequest("Request is null.");
            if (req.VariantId <= 0) return BadRequest("VariantId is required.");
            if (req.Price <= 0) return BadRequest("Price must be greater than zero.");

            var result = await _create.HandleAsync(req, ct);
            if (result == null) return BadRequest("Create failed.");

            // result.Id có sẵn trong OutputDTO
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }


        // GET: api/variantprices/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(id, ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        // PUT: api/variantprices/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] VariantPriceInputDTO body, CancellationToken ct)
        {
            if (body == null) return BadRequest("Body is null.");
            if (body.Price <= 0) return BadRequest("Price must be greater than zero.");

            // Gán Id từ route vào DTO
            var updateDto = new VariantPriceInputDTO(
                      body.VariantId,
                      body.Currency,
                      body.Price,
                      body.DiscountPrice,
                      body.Status,
                      body.ValidFrom,
                      body.ValidTo
             );


            var rs = await _update.HandleAsync(updateDto, ct);
            return rs is null ? NotFound() : Ok(rs);
        }


        // DELETE: api/variantprices/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var rs = await _delete.HandleAsync(id, ct);
            return rs is null ? NotFound() : Ok(rs);
        }
    }
}
