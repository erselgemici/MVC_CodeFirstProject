using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.Filters;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace MyAcademy_MVC_CodeFirst.Areas.Staff.Controllers
{
    [Authorize]
    [LogAction(ActionDescription = "Personel Dashboard")]
    public class DashboardController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            var email = User.Identity.Name;
            var user = db.AppUsers.FirstOrDefault(x => x.Email == email);

            if (user == null) return RedirectToAction("Login", "Account", new { area = "" });

            // İSTATİSTİKLER (Sadece bu personelin verileri)
            ViewBag.TotalSales = db.Sales.Count(); // Toplam Satış Adedi
            ViewBag.TotalRevenue = db.Sales.Any() ? db.Sales.Sum(x => x.Amount) : 0; // Toplam Ciro

            var lastSales = db.Sales.OrderByDescending(x => x.SaleID).Take(5).ToList();
            ViewBag.LastSales = lastSales;

            return View(user);
        }
    }
}
