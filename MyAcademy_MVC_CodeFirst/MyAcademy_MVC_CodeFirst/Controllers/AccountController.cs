using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

public class AccountController : Controller
{
    AppDbContext db = new AppDbContext();

    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Login(LoginViewModel p)
    {
        var user = db.AppUsers.FirstOrDefault(x => x.Email == p.Email && x.Password == p.Password);

        if (user != null)
        {
            FormsAuthentication.SetAuthCookie(user.Email, false);
            Session["UserID"] = user.UserID;
            Session["FullName"] = user.FullName;
            Session["Role"] = user.Role;

            if (user.Role == "Admin" || user.Role == "Manager")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else if (user.Role == "Staff")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Staff" });
            }
        }

        ViewBag.Error = "E-posta veya şifre hatalı!";
        return View();
    }


    public ActionResult Logout()
    {
        FormsAuthentication.SignOut();
        Session.Abandon();
        return RedirectToAction("Login");
    }
}
