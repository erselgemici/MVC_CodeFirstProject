using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class AboutController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            var values = db.Abouts.ToList();
            return View(values);
        }

        [HttpGet]
        public ActionResult CreateAbout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAbout(About about)
        {
            if (ModelState.IsValid)
            {
                db.Abouts.Add(about);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(about);
        }

        public ActionResult DeleteAbout(int id)
        {
            var value = db.Abouts.Find(id);
            db.Abouts.Remove(value);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateAbout(int id)
        {
            var value = db.Abouts.Find(id);
            return View(value);
        }

        [HttpPost]
        public ActionResult UpdateAbout(About about)
        {
            var value = db.Abouts.Find(about.AboutID);
            value.Title = about.Title;
            value.Description = about.Description;
            value.ImageUrl = about.ImageUrl;
            value.Features = about.Features;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
