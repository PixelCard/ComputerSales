using ComputerSales.Application.Interface.InterfaceVNPAYMENT;
using ComputerSales.Application.UseCaseDTO.VNPAYMENT_DTO;
using ComputerSales.Domain.Entity.EPaymentVNPAYTransaction;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComputerSales.Infrastructure.Repositories.VNPAYMENTRespo
{
    public class VnPaySessionServiceRespo : IVnPaySessionService
    {
        private readonly AppDbContext _db;

        public VnPaySessionServiceRespo(AppDbContext db)
        {
            _db = db;
        }

        public async Task CompleteAsync(string sessionId, int orderId, VnPayCallbackDataDTO data, CancellationToken ct)
        {
            var session = await _db.vNPAYPaymentSessions.FirstOrDefaultAsync(x => x.Id == sessionId, ct);

            if (session == null) return;

            // idempotent
            if (session.Status == "Completed" && session.OrderId.HasValue)
                return;

            // log giao dịch
            _db.vNPAYPaymentTransactions.Add(new VNPAYPaymentTransaction
            {
                SessionId = sessionId,
                OrderId = orderId,
                Gateway = "VNPAY",
                TransactionId = data.TransactionId,
                ResponseCode = data.ResponseCode,
                Amount = data.Amount
            });

            session.Status = "Completed";
            session.OrderId = orderId;

            await _db.SaveChangesAsync(ct);
        }

        public async Task<VNPAYPaymentSession> CreatePendingAsync(int userId, decimal amount, CancellationToken ct)
        {
            var s = new VNPAYPaymentSession
            {
                UserId = userId,
                Amount = amount,
                Status = "Pending",
                TxnRef = null // <-- để NULL lúc insert
            };

            _db.vNPAYPaymentSessions.Add(s);
            await _db.SaveChangesAsync(ct);       // lúc này SeqId đã có

            s.TxnRef = s.SeqId.ToString("D12");   // chuỗi số 12 chữ số
            await _db.SaveChangesAsync(ct);       // cập nhật TxnRef (unique ok vì đã có filter)

            return s;
        }

        public Task<VNPAYPaymentSession?> GetByTxnRefAsync(string txnRef, CancellationToken ct)
            => _db.vNPAYPaymentSessions.FirstOrDefaultAsync(x => x.TxnRef == txnRef, ct);
    }
}
