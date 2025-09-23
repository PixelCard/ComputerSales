using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Cart_UC.Commands.RemoveItem
{
    public record RemoveItemCommand(int CartId, int ItemId);
}
