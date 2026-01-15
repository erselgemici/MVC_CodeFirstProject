using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class ContactController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            var values = db.ContactMessages.OrderByDescending(x => x.SentDate).ToList();
            return View(values);
        }

        public ActionResult DeleteMessage(int id)
        {
            var value = db.ContactMessages.Find(id);
            db.ContactMessages.Remove(value);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult MessageDetails(int id)
        {
            var value = db.ContactMessages.Find(id);

            if (!value.IsRead)
            {
                value.IsRead = true;
                db.SaveChanges();
            }

            return View(value);
        }
    }
}
