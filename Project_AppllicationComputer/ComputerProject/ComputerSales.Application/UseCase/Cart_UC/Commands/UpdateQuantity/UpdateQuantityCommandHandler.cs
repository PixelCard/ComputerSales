using ComputerSales.Application.Interface.Cart_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Cart_UC.Commands.UpdateQuantity
{
    public sealed class UpdateQuantityCommandHandler
    {
        private readonly ICartWriteRepository _repo;
        private readonly IUnitOfWorkApplication unitOfWorkApplication;
        const int LIMIT = 3;
        public UpdateQuantityCommandHandler(ICartWriteRepository repo,IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repo = repo;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }
            
        public async Task Handle(UpdateQuantityCommand cmd, CancellationToken ct = default)
        {
            var cart = await _repo.GetByIdAsync(cmd.CartId, ct) ?? throw new InvalidOperationException("Cart not found");
            var item = cart.Items.First(x => x.ID == cmd.ItemId);
            item.Quantity = Math.Clamp(cmd.Quantity, 1, LIMIT); 

            // domain rule: tính Subtotal theo UnitPrice ; Cart tự tính GrandTotal
            cart.Subtotal = cart.Items.Sum(i => i.UnitPrice * i.Quantity);
            cart.RecalculateTotals(); // cập nhật GrandTotal/UpdateAt 
            await unitOfWorkApplication.SaveChangesAsync(ct);
        }
    }
}
