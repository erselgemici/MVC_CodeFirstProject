using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.Staff.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            var email = User.Identity.Name;
            var user = db.AppUsers.FirstOrDefault(x => x.Email == email);

            if (user == null) return RedirectToAction("Login", "Account", new { area = "" });

            // İSTATİSTİKLER (Sadece bu personelin verileri)
            // Eğer Sales tablosunda "PersonelID" yoksa şimdilik genel toplamı gösteririz.
            // Ama doğrusu satışları personele bağlamaktır. Şimdilik genel akışı bozmuyoruz.

            ViewBag.TotalSales = db.Sales.Count(); // Toplam Satış Adedi
            ViewBag.TotalRevenue = db.Sales.Any() ? db.Sales.Sum(x => x.Amount) : 0; // Toplam Ciro

            // Son 5 Satışı da çekelim (Dashboard altında listeletmek için)
            var lastSales = db.Sales.OrderByDescending(x => x.SaleID).Take(5).ToList();
            ViewBag.LastSales = lastSales;

            return View(user);
        }
    }
}
