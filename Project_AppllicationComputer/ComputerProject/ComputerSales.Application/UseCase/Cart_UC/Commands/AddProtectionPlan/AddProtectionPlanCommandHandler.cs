using ComputerSales.Application.Interface.Cart_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Domain.Entity.ECart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Cart_UC.Commands.AddProtectionPlan
{
    public sealed class AddProtectionPlanCommandHandler
    {
        private readonly ICartWriteRepository _repo;
        private readonly ICartReadRespository _read;
        private readonly IUnitOfWorkApplication unitOfWorkApplication;
        public AddProtectionPlanCommandHandler(
            ICartWriteRepository repo, 
            ICartReadRespository read, 
            IUnitOfWorkApplication unitOfWorkApplication)
        { 
            _repo = repo; 

            _read = read; 

            this.unitOfWorkApplication= unitOfWorkApplication;
        }

        public async Task Handle(AddProtectionPlanCommand cmd, CancellationToken ct = default)
        {
            var cart = await _repo.GetByIdAsync(cmd.CartId, ct) ?? throw new InvalidOperationException("Cart not found");
            var parent = cart.Items.First(x => x.ID == cmd.ParentItemId);

            var dict = await _read.GetVariantsAsync(new[] { cmd.PlanVariantId }, ct);
            var v = dict[cmd.PlanVariantId];
            var price = v.VariantPrices.OrderByDescending(p => p.ValidFrom ?? DateTime.MinValue)
                                       .FirstOrDefault()?.DiscountPrice ?? 0m;

            cart.Items.Add(new CartItem
            {
                CartID = cart.ID,
                ProductVariantID = cmd.PlanVariantId,
                ParentItemID = parent.ID,
                ItemType = 2, // ProtectionPlan
                SKU = v.SKU,
                Name = "Protection Plan",
                ImageUrl = parent.ImageUrl,
                UnitPrice = price,
                Quantity = 1,
                CreatedAt = DateTime.UtcNow,
                IsSelected = true
            }); // dùng ParentItemID/Children đã có trong CartItem :contentReference[oaicite:7]{index=7}

            cart.Subtotal = cart.Items.Sum(i => i.UnitPrice * i.Quantity);
            cart.RecalculateTotals(); 
            await unitOfWorkApplication.SaveChangesAsync(ct);
        }
    }
}
