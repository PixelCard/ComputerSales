using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Cart_UC.Commands.AddProtectionPlan
{
    public record AddProtectionPlanCommand(int CartId, int ParentItemId, int PlanVariantId);
}
