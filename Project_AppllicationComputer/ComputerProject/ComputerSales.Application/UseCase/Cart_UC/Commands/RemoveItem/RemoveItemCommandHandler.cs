using ComputerSales.Application.Interface.Cart_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Cart_UC.Commands.RemoveItem
{
    public sealed class RemoveItemCommandHandler
    {
        private readonly ICartWriteRepository _repo;
        private readonly IUnitOfWorkApplication unitOfWorkApplication;
        public RemoveItemCommandHandler(ICartWriteRepository repo, IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repo = repo;
            this.unitOfWorkApplication= unitOfWorkApplication;
        }

        public async Task Handle(RemoveItemCommand cmd, CancellationToken ct = default)
        {
            var cart = await _repo.GetByIdAsync(cmd.CartId, ct) ?? throw new InvalidOperationException("Cart not found");
            var item = cart.Items.First(x => x.ID == cmd.ItemId);
            cart.Items.Remove(item);

            cart.Subtotal = cart.Items.Sum(i => i.UnitPrice * i.Quantity);
            cart.RecalculateTotals(); // :contentReference[oaicite:6]{index=6}
            await unitOfWorkApplication.SaveChangesAsync(ct);
        }
    }
}
