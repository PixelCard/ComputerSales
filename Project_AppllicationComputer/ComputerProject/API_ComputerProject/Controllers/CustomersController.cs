using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using ComputerSales.Application.UseCaseDTO.Customer_DTO.getCustomerByID;
using Microsoft.AspNetCore.Mvc;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CreateCustomer_UC _create;
        private readonly getCustomerByID _getById;
        private readonly DeleteCustomer_UC _delete;

        public CustomersController(
            CreateCustomer_UC create,
            getCustomerByID getById,
            DeleteCustomer_UC delete)
        {
            _create = create;
            _getById = getById;
            _delete = delete;
        }

        // POST: api/customers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerInputDTO req, CancellationToken ct)
        {
            if (req is null) return BadRequest("Request body is null");
            if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest("Name is required.");

            var result = await _create.HandleAsync(req, ct);
            return result is null
                ? BadRequest("Could not create customer.")
                : CreatedAtAction(nameof(GetById), new { id = result.IDCustomer }, result);
        }

        // GET: api/customers/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _getById.HandleAsync(new InputGetCustomerByID(id), ct);
            return result is null ? NotFound() : Ok(result);
        }
    }
}
