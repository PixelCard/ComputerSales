using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCase.Role_UC;
using ComputerSales.Application.UseCaseDTO.Role_DTO;
using ComputerSales.Application.UseCaseDTO.Role_DTO.DeleteRole;
using ComputerSales.Application.UseCaseDTO.Role_DTO.GetRoleByID;
using ComputerSales.Application.UseCaseDTO.Role_DTO.UpdateRole;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_ComputerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly CreateRole_UC _create;
        private readonly DeleteRole_UC _delete;
        private readonly GetRole_UC _get;
        private readonly UpdateRole_UC _update;
        public RoleController( CreateRole_UC create,DeleteRole_UC delete,UpdateRole_UC update, GetRole_UC get)
        {
            _create = create;
            _delete = delete;
            _update = update;
            _get = get;
        }
        // POST: api/roles
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleDTOInput req, CancellationToken ct)
        {
            if (req is null) return BadRequest();
            if (string.IsNullOrWhiteSpace(req.TenRole))
                return BadRequest("TenRole is required.");

            // Nếu bạn dùng mapping trong UC thì có thể truyền thẳng req
            var result = await _create.HandleAsync(req, ct);

            // result giả định có IDRole
            return CreatedAtAction(nameof(GetById), new { id = result.IDRole }, result);
        }
        //============      GetByID     =========//
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var rs = await _get.HandleAsync(new GetRoleByID(id), ct);
            return rs is null ? NotFound() : Ok(rs);
        }


        //===========   Update  =============//
        // PUT: api/roles/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDTO body, CancellationToken ct)
        {
            if (body is null) return BadRequest();
            if (id != body.IDRole) return BadRequest("Mismatched id.");
            if (string.IsNullOrWhiteSpace(body.TenRole))
                return BadRequest("TenRole is required.");

            var rs = await _update.HandleAsync(body, ct);
            return rs is null ? NotFound() : Ok(rs);
        }

        //=========     Delete  ===========//
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await _delete.HandleAsync(new DeleteRoleInput(id), ct);
            return ok ? NoContent() : NotFound();
        }


    }
}
