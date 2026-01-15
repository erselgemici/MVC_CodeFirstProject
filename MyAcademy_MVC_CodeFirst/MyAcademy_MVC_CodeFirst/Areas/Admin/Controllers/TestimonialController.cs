using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class TestimonialController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            return View(db.Testimonials.ToList());
        }

        [HttpGet]
        public ActionResult CreateTestimonial()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTestimonial(Testimonial testimonial)
        {
            db.Testimonials.Add(testimonial);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteTestimonial(int id)
        {
            var value = db.Testimonials.Find(id);
            db.Testimonials.Remove(value);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateTestimonial(int id)
        {
            var value = db.Testimonials.Find(id);
            return View(value);
        }

        [HttpPost]
        public ActionResult UpdateTestimonial(Testimonial testimonial)
        {
            var value = db.Testimonials.Find(testimonial.TestimonialID);
            value.ClientName = testimonial.ClientName;
            value.Profession = testimonial.Profession;
            value.Comment = testimonial.Comment;
            value.ImageUrl = testimonial.ImageUrl;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
