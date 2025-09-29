using ComputerSales.Application.UseCaseDTO.VNPAYMENT_DTO;
using ComputerSales.Domain.Entity.EPaymentVNPAYTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.Interface.InterfaceVNPAYMENT
{
    public interface IVnPaySessionService
    {
        Task<VNPAYPaymentSession> CreatePendingAsync(int userId, decimal amount, CancellationToken ct);
        Task<VNPAYPaymentSession?> GetByTxnRefAsync(string txnRef, CancellationToken ct);
        Task CompleteAsync(string sessionId, int orderId, VnPayCallbackDataDTO data, CancellationToken ct);
    }
}
