using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using ComputerSales.Application.UseCaseDTO.Customer_DTO.UpdateCustomerDTO;
using ComputerSales.Domain.Entity.ECustomer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Customer_UC
{
    public class UpdateCustomer_UC
    {
        private readonly IRespository<Customer> _repo;
        private readonly IUnitOfWorkApplication _uow;

        public UpdateCustomer_UC(IRespository<Customer> repo, IUnitOfWorkApplication uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<CustomerOutputDTO?> HandleAsync(int idCustomer, InputUpdateCustomerDTO input, CancellationToken ct = default)
        {
            Customer? updatedEntity = null;

            await _repo.UpdateByIdAsync(idCustomer, entity =>
            {
                entity.IMG = input.IMG;
                entity.Name = input.Name.Trim();
                entity.Description = input.Description;
                entity.sdt = input.sdt;
                entity.address = input.address;
                entity.Date = input.Date;

                updatedEntity = entity; // để sau khi update còn trả về
            }, ct);

            await _uow.SaveChangesAsync(ct);

            return updatedEntity?.ToResult();
        }

    }
}
