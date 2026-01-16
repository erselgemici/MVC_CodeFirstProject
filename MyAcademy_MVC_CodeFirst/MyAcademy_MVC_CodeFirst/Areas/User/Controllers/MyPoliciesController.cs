using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;

namespace MyAcademy_MVC_CodeFirst.Areas.User.Controllers
{
    [Authorize]
    public class MyPoliciesController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            var email = User.Identity.Name;
            var customer = db.Customers.FirstOrDefault(x => x.Email == email);

            if (customer == null) return RedirectToAction("Index", "Dashboard");

            // Müşterinin satın aldığı poliçeleri (Sales tablosundan) çekiyoruz
            var myPolicies = db.Sales
                               .Where(x => x.CustomerID == customer.CustomerID)
                               .OrderByDescending(x => x.SaleDate)
                               .ToList();

            return View(myPolicies);
        }
    }
}
