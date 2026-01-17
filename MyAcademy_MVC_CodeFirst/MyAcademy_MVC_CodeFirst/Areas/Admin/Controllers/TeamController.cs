using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.Filters;
using System.Linq;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [LogAction(ActionDescription = "Ekip Üyeleri Yönetimi")]
    public class TeamController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            var values = db.TeamMembers.ToList();
            return View(values);
        }

        [HttpGet]
        public ActionResult CreateTeam()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTeam(TeamMember teamMember)
        {
            if (ModelState.IsValid)
            {
                db.TeamMembers.Add(teamMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teamMember);
        }

        public ActionResult DeleteTeam(int id)
        {
            var value = db.TeamMembers.Find(id);
            db.TeamMembers.Remove(value);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateTeam(int id)
        {
            var value = db.TeamMembers.Find(id);
            return View(value);
        }

        [HttpPost]
        public ActionResult UpdateTeam(TeamMember teamMember)
        {
            var value = db.TeamMembers.Find(teamMember.TeamMemberID);

            value.FullName = teamMember.FullName;
            value.Title = teamMember.Title;
            value.ImageUrl = teamMember.ImageUrl;
            value.TwitterUrl = teamMember.TwitterUrl;
            value.LinkedInUrl = teamMember.LinkedInUrl; 

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
