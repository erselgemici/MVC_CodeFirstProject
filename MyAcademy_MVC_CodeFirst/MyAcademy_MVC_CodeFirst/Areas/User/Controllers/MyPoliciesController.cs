using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Filters;
using System.Linq;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.User.Controllers
{
    [Authorize]
    [LogAction(ActionDescription = "Kullanıcı Poliçelerini Görüntüledi")]
    public class MyPoliciesController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            var email = User.Identity.Name;
            var customer = db.Customers.FirstOrDefault(x => x.Email == email);

            if (customer == null) return RedirectToAction("Index", "Dashboard");

            var myPolicies = db.Sales
                               .Where(x => x.CustomerID == customer.CustomerID)
                               .OrderByDescending(x => x.SaleDate)
                               .ToList();

            return View(myPolicies);
        }
    }
}
