using MyAcademy_MVC_CodeFirst.Data.Context;
using System.Linq;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            ViewBag.TotalSales = db.Sales.Any() ? db.Sales.Sum(x => x.Amount) : 0;
            ViewBag.TotalCustomers = db.Customers.Count();
            ViewBag.TotalPolicies = db.InsurancePolicies.Count();
            ViewBag.TotalMessages = db.ContactMessages.Count();

            var lastSales = db.Sales.Include("Customer").Include("InsurancePolicy")
                                    .OrderByDescending(x => x.SaleDate)
                                    .Take(5)
                                    .ToList();

            return View(lastSales);
        }
    }
}
