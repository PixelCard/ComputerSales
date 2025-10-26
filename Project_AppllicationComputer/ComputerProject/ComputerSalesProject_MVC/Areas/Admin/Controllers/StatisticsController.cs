using ComputerSales.Domain.Entity.E_Order;
using ComputerSales.Infrastructure.Persistence;
using ComputerSalesProject_MVC.Areas.Admin.Models.StatictistViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerSalesProject_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class StatisticsController : Controller
    {
        private readonly AppDbContext _db;

        public StatisticsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult StatictistHome()
        {
            var vm = new StatisticFilterVM
            {
                Type = "month",
                Year = DateTime.Now.Year,
                Month = DateTime.Now.Month
            };
            ViewData["Title"] = "Thống kê doanh thu";
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StatictistHome(StatisticFilterVM vm)
        {
            var query = _db.Orders
                .AsNoTracking()
                .Where(o => o.Status && o.OrderStatus == OrderStatus.DaGiaoThanhCong);

            List<StatisticResultVM> result = new();

            // ==== 1️⃣ Thống kê theo ngày ====
            if (vm.Type == "day" && vm.FromDate.HasValue && vm.ToDate.HasValue)
            {
                DateTime start = vm.FromDate.Value.Date;
                DateTime end = vm.ToDate.Value.Date.AddDays(1).AddTicks(-1);

                var data = await query
                    .Where(o => o.OrderTime >= start && o.OrderTime <= end)
                    .GroupBy(o => o.OrderTime.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        TotalRevenue = g.Sum(x => x.GrandTotal),
                        OrderCount = g.Count()
                    })
                    .ToListAsync();

                result = data
                    .Select(r => new StatisticResultVM
                    {
                        Label = r.Date.ToString("dd/MM/yyyy"),
                        TotalRevenue = r.TotalRevenue,
                        OrderCount = r.OrderCount
                    })
                    .OrderBy(x => x.Label)
                    .ToList();

                vm.FromDate = start;
                vm.ToDate = end;
            }

            // ==== 2️⃣ Thống kê theo tháng ====
            else if (vm.Type == "month" && vm.Year.HasValue)
            {
                var data = await query
                    .Where(o => o.OrderTime.Year == vm.Year.Value && o.OrderTime.Month == vm.Month.Value)
                    .GroupBy(o => o.OrderTime.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        TotalRevenue = g.Sum(x => x.GrandTotal),
                        OrderCount = g.Count()
                    })
                    .ToListAsync();

                result = data
                    .Select(r => new StatisticResultVM
                    {
                        Label = r.Date.ToString("dd/MM/yyyy"),
                        TotalRevenue = r.TotalRevenue,
                        OrderCount = r.OrderCount
                    })
                    .OrderBy(x => x.Label)
                    .ToList();
            }

            // ==== 3️⃣ Thống kê theo năm ====
            else if (vm.Type == "year")
            {
                var data = await query
                    .GroupBy(o => o.OrderTime.Year)
                    .Select(g => new
                    {
                        Year = g.Key,
                        TotalRevenue = g.Sum(x => x.GrandTotal),
                        OrderCount = g.Count()
                    })
                    .ToListAsync();

                result = data
                    .Select(r => new StatisticResultVM
                    {
                        Label = "Năm " + r.Year,
                        TotalRevenue = r.TotalRevenue,
                        OrderCount = r.OrderCount
                    })
                    .OrderBy(x => x.Label)
                    .ToList();
            }

            // ==== 4️⃣ Thống kê theo tuần ====
            else if (vm.Type == "week")
            {
                DateTime today = DateTime.Now;
                int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                DateTime startOfWeek = today.AddDays(-diff).Date;
                DateTime endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);

                var data = await query
                    .Where(o => o.OrderTime >= startOfWeek && o.OrderTime <= endOfWeek)
                    .GroupBy(o => o.OrderTime.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        TotalRevenue = g.Sum(x => x.GrandTotal),
                        OrderCount = g.Count()
                    })
                    .ToListAsync();

                result = data
                    .Select(r => new StatisticResultVM
                    {
                        Label = r.Date.ToString("dd/MM/yyyy"),
                        TotalRevenue = r.TotalRevenue,
                        OrderCount = r.OrderCount
                    })
                    .OrderBy(x => x.Label)
                    .ToList();

                vm.FromDate = startOfWeek;
                vm.ToDate = endOfWeek;
            }

            // ✅ Nếu không có dữ liệu
            if (result.Count == 0)
            {
                TempData["InfoMessage"] = "Không có đơn hàng nào trong khoảng thời gian đã chọn.";
            }

            vm.Results = result;
            ViewData["Title"] = "Thống kê doanh thu";
            return View(vm);
        }
    }
}
