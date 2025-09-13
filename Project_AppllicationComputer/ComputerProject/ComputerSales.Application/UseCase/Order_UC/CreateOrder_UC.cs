using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Order_DTO;
using ComputerSales.Domain.Entity.E_Order;

namespace ComputerSales.Application.UseCase.Order_UC
{
    public class CreateOrder_UC
    {
        private readonly IRespository<Order> _repo;
        private readonly IUnitOfWorkApplication _unitOfWorkApplication;

        public CreateOrder_UC(IRespository<Order> repo,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repo = repo;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<OrderOutputDTO> HandleAsync(OrderInputDTO input, CancellationToken ct = default)
        {
            var entity = input.ToEntity();            // map DTO -> Order
            await _repo.AddAsync(entity, ct);         // ✅ Add entity, KHÔNG phải DTO
            await _unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();                 // ✅ gọi extension ToResult()
        }
    }
}
