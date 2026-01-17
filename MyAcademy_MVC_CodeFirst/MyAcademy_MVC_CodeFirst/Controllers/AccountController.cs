using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Filters; 
using MyAcademy_MVC_CodeFirst.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security; 

namespace MyAcademy_MVC_CodeFirst.Controllers
{
    [LogAction(ActionDescription = "Oturum İşlemleri")]
    public class AccountController : Controller
    {
        AppDbContext db = new AppDbContext();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [LogAction(ActionDescription = "Giriş Denemesi")]
        public ActionResult Login(LoginViewModel p)
        {
            // Formdan gelen Email ve Şifre ile eşleşen ilk kaydı bulmaya çalışıyoruz.
            var user = db.AppUsers.FirstOrDefault(x => x.Email == p.Email && x.Password == p.Password);

            // Eğer veritabanından bir kullanıcı dönerse (user null değilse), giriş bilgileri doğrudur.
            if (user != null)
            {
                // Tarayıcıya "Bu kullanıcı güvenlidir" damgasını vuruyoruz.
                // 'false' parametresi "Beni Hatırla" kapalı demek (tarayıcı kapanınca oturum biter).
                FormsAuthentication.SetAuthCookie(user.Email, false);

                // Yazdığımız Loglama sistemi (LogActionAttribute) verileri buradan okuyacak.
                // Kullanıcı her sayfa değiştirdiğinde veritabanına tekrar gitmemek için bu bilgileri sunucu RAM'ine (Session) atıyoruz.
                Session["UserID"] = user.UserID;      
                Session["FullName"] = user.FullName;  
                Session["Role"] = user.Role;          
                Session["Email"] = user.Email;     

                if (user.Role == "Admin" || user.Role == "Manager")
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                else if (user.Role == "Staff")
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Staff" });
                }
                else if (user.Role == "Customer")
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "User" });
                }
            }

            ViewBag.Error = "E-posta veya şifre hatalı!";
            return View();
        }

        [LogAction(ActionDescription = "Oturum Kapatıldı")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            // Sunucu hafızasındaki (Session) verileri temizliyoruz.
            Session.Abandon();

            return RedirectToAction("Login");
        }
    }
}
