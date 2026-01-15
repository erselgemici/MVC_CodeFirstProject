using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.Staff.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index(string search)
        {
            // Veritabanındaki müşterileri alıyoruz
            var values = from x in db.Customers select x;

            // Eğer arama kutusuna bir şey yazıldıysa filtrele
            if (!string.IsNullOrEmpty(search))
            {
                values = values.Where(y => y.FirstName.Contains(search) ||
                                           y.LastName.Contains(search) ||
                                           y.City.Contains(search));
            }

            // Son 100 müşteriyi, en yeniden eskiye doğru getir
            var result = values.OrderByDescending(x => x.CustomerID).Take(100).ToList();

            return View(result);
        }
    }
}
