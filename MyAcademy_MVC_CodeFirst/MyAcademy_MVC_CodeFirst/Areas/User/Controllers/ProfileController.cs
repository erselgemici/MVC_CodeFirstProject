using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.User.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        AppDbContext db = new AppDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            var email = User.Identity.Name;

            // Müşteri hem AppUsers tablosunda (Login için) 
            // Hem de Customers tablosunda (Veri için) olabilir.
            // Biz Login olan AppUser'ı güncelleyelim.
            var user = db.AppUsers.FirstOrDefault(x => x.Email == email);

            return View(user);
        }

        [HttpPost]
        public ActionResult Index(AppUser p)
        {
            var user = db.AppUsers.Find(p.UserID);

            user.FullName = p.FullName;
            user.Email = p.Email;
            user.Password = p.Password; // Gerçek hayatta hash'lenmeli

            db.SaveChanges();

            // Session'ı güncelle ki sağ üstteki isim hemen değişsin
            Session["FullName"] = user.FullName;

            ViewBag.Success = "Bilgileriniz başarıyla güncellendi.";
            return View(user);
        }
    }
}
