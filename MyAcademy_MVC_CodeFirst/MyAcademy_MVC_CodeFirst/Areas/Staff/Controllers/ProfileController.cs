using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.Staff.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        AppDbContext db = new AppDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            // Giriş yapan kullanıcıyı bul
            var email = User.Identity.Name;
            var user = db.AppUsers.FirstOrDefault(x => x.Email == email);
            return View(user);
        }

        [HttpPost]
        public ActionResult Index(AppUser p)
        {
            var user = db.AppUsers.Find(p.UserID);

            // Bilgileri Güncelle
            user.FullName = p.FullName;
            user.Email = p.Email;
            user.Password = p.Password; // Gerçek projede şifre hash'lenmeli!

            // Rolü değiştirmesine izin verme (Güvenlik)
            // user.Role = p.Role; <- BU SATIR ASLA OLMAMALI

            db.SaveChanges();

            // Session'ı da güncelle ki isim sağ üstte değişsin
            Session["FullName"] = user.FullName;

            ViewBag.Success = "Profil bilgileriniz başarıyla güncellendi.";
            return View(user);
        }
    }
}
