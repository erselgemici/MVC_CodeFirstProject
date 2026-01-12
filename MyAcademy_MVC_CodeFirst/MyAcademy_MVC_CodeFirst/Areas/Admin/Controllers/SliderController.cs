using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            return View(db.Sliders.ToList());
        }

        [HttpGet]
        public ActionResult CreateSlider()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSlider(Slider slider)
        {
            db.Sliders.Add(slider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteSlider(int id)
        {
            var value = db.Sliders.Find(id);
            db.Sliders.Remove(value);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateSlider(int id)
        {
            var value = db.Sliders.Find(id);
            return View(value);
        }

        [HttpPost]
        public ActionResult UpdateSlider(Slider slider)
        {
            var value = db.Sliders.Find(slider.SliderID);
            value.Title = slider.Title;
            value.Description = slider.Description;
            value.ImageUrl = slider.ImageUrl;
            value.VideoUrl = slider.VideoUrl;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
