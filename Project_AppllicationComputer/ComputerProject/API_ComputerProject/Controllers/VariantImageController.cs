using ComputerSales.Application.UseCase.VariantImage_UC;
using ComputerSales.Application.UseCaseDTO.NewFolder;
using ComputerSales.Application.UseCaseDTO.VariantImage;
using ComputerSales.Application.UseCaseDTO.VariantImage.DeleteVariantImage;
using ComputerSales.Application.UseCaseDTO.VariantImageDTO;
using Microsoft.AspNetCore.Mvc;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantImagesController : ControllerBase
    {
        private readonly CreateVariantImage_UC _create;
        private readonly DeleteVariantImage_UC _delete;
        private readonly getVariantImageById_UC _get;
        private readonly UpdateVariantImage_UC _update;

        public VariantImagesController(
            CreateVariantImage_UC create,
            DeleteVariantImage_UC delete,
            UpdateVariantImage_UC update,
            getVariantImageById_UC get)
        {
            _create = create;
            _delete = delete;
            _update = update;
            _get = get;
        }

        // POST: api/variantimages
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VariantImageInputDTO req, CancellationToken ct)
        {
            if (req == null) return BadRequest("Request is null.");
            if (req.VariantId <= 0) return BadRequest("VariantId is required.");
            if (string.IsNullOrWhiteSpace(req.Url)) return BadRequest("Url is required.");

            var result = await _create.HandleAsync(req, ct);
            if (result == null) return BadRequest("Create failed.");

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // GET: api/variantimages/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(id, ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        // PUT: api/variantimages/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] VariantImageOutputDTO body, CancellationToken ct)
        {
            if (body == null) return BadRequest("Body is null.");
            if (id != body.Id) return BadRequest("Mismatched id.");

            var rs = await _update.HandleAsync(body, ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        // DELETE: api/variantimages/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await _delete.HandleAsync(new DeleteVariantImageInput(id), ct);
            return ok ? NoContent() : NotFound();
        }

    }
}
