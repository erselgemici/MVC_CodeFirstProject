using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.Filters;
using System.Linq;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.User.Controllers
{
    [Authorize]
    [LogAction(ActionDescription = "Kullanıcı Dashboard")]
    public class DashboardController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            var email = User.Identity.Name;


            var customer = db.Customers.FirstOrDefault(x => x.Email == email);

            if (customer == null)
            {             
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
