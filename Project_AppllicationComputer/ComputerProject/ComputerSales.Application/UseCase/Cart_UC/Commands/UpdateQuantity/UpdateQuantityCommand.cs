using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Cart_UC.Commands.UpdateQuantity
{
    public record UpdateQuantityCommand(int CartId, int ItemId, int Quantity);
}
