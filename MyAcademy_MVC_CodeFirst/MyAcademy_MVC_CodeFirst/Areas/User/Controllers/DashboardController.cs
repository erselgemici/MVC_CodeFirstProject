using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.User.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            var email = User.Identity.Name;

            // Giriş yapan müşteriyi buluyoruz
            // NOT: AppUser tablosunda müşteriler yoksa, Customers tablosunda mail eşleşmesi yapacağız.
            // Şimdilik sistemimizde login olanlar AppUser tablosunda.
            // SENARYO: Müşteri login olunca AppUser tablosundan ID alır, 
            // ama Sales tablosunda 'CustomerID' var. Bu ikisini bağlamamız gerekecek.
            // *Geçici Çözüm:* Müşterinin email adresini Customers tablosunda aratacağız.

            var customer = db.Customers.FirstOrDefault(x => x.Email == email);

            if (customer == null)
            {
                // Eğer bu mail ile Customers tablosunda kayıt yoksa,
                // Demek ki bu bir Staff/Admin ama User paneline bakmak istiyor veya yeni üye.
                // Boş model gönderelim hata vermesin.
                ViewBag.PolicyCount = 0;
                ViewBag.TotalCoverage = 0;
                ViewBag.LastRenewDate = "-";
            }
            else
            {
                // Müşterinin İstatistikleri
                var mySales = db.Sales.Where(x => x.CustomerID == customer.CustomerID).ToList();

                ViewBag.PolicyCount = mySales.Count();
                ViewBag.TotalAmount = mySales.Sum(x => x.Amount);
                // Son alınan poliçe tarihi
                var lastSale = mySales.OrderByDescending(x => x.SaleDate).FirstOrDefault();
                ViewBag.LastActionDate = lastSale != null ? lastSale.SaleDate.ToString("dd MMM yyyy") : "İşlem Yok";
            }

            return View();
        }
    }
}
