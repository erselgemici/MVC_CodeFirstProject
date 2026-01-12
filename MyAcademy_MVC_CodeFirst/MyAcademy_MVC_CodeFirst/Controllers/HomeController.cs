using System.Linq;
using System.Web.Mvc;
using MyAcademy_MVC_CodeFirst.Data.Context; // Context yolunuzu kontrol edin

namespace MyAcademy_MVC_CodeFirst.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult HeaderCarouselPartial()
        {
            var values = db.Sliders.ToList();
            return PartialView("~/Views/Home/Partials/_HeaderCarouselPartial.cshtml", values);
        }

        public PartialViewResult FeaturePartial()
        {
            var values = db.Features.ToList();
            return PartialView("~/Views/Home/Partials/_FeaturePartial.cshtml", values);
        }

        public PartialViewResult AboutPartial()
        {
            ViewBag.PolicyCount = db.InsurancePolicies.Count();
            ViewBag.CustomerCount = db.Customers.Count();
            ViewBag.AgentCount = db.TeamMembers.Count();
            ViewBag.AwardCount = 99;

            var value = db.Abouts.FirstOrDefault();
            return PartialView("~/Views/Home/Partials/_AboutPartial.cshtml", value);
        }

        public PartialViewResult ServicePartial()
        {
            var values = db.InsurancePolicies.Include("InsuranceCategory").Take(4).ToList();
            return PartialView("~/Views/Home/Partials/_ServicePartial.cshtml", values);
        }

        public PartialViewResult FaqPartial()
        {
            var values = db.Faqs.ToList();
            return PartialView("~/Views/Home/Partials/_FaqPartial.cshtml", values);
        }

        public PartialViewResult BlogPartial()
        {
            var values = db.BlogPosts.OrderByDescending(x => x.PublishedDate).Take(3).ToList();
            return PartialView("~/Views/Home/Partials/_BlogPartial.cshtml", values);
        }

        public PartialViewResult TeamPartial()
        {
            var values = db.TeamMembers.ToList();
            return PartialView("~/Views/Home/Partials/_TeamPartial.cshtml", values);
        }

        public PartialViewResult TestimonialPartial()
        {
            var values = db.Testimonials.ToList();
            return PartialView("~/Views/Home/Partials/_TestimonialPartial.cshtml", values);
        }
        
        public PartialViewResult FooterPartial()
        {
            var value = db.CompanySettings.FirstOrDefault();
            return PartialView("~/Views/Home/Partials/_FooterPartial.cshtml", value);
        }

        public PartialViewResult TopbarPartial()
        {
            var value = db.CompanySettings.FirstOrDefault();
            return PartialView("~/Views/Home/Partials/_TopbarPartial.cshtml", value);
        }
    }
}
