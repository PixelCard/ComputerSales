using ComputerSales.Application.UseCaseDTO.Product_DTO;
using ComputerSales.Application.UseCaseDTO.Product_DTO.UpdateProduct;
using ComputerSales.Application.UseCaseDTO.Role_DTO.UpdateRole;
using ComputerSales.Domain.Entity.EAccount;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ComputerSales.Application.UseCaseDTO.Role_DTO
{
    public static class RoleMapping
    {
        public static Role ToEnity(this RoleDTOInput input)
        {
            return Role.Create(
               input.TenRole
            );
        }
        public static RoleOutputDTO ToResult(this Role e)
        {
            return new RoleOutputDTO(
                e.IDRole,
                e.TenRole
             );
        }

        public static void ApplyUpdate(this Role e, UpdateRoleDTO dto)
        {
            e.TenRole = dto.TenRole;
        }

    }
}
   

