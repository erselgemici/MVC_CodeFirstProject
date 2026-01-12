using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        AppDbContext db = new AppDbContext();

        public ActionResult Index(string search, int page = 1)
        {
            var values = db.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                values = values.Where(x => x.FirstName.Contains(search) ||
                                           x.LastName.Contains(search) ||
                                           x.Email.Contains(search));
            }

            ViewBag.Search = search;

            return View(values.OrderBy(x => x.CustomerID).ToPagedList(page, 10));
        }

        [HttpGet]
        public ActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CreatedAt = DateTime.Now; 
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        public ActionResult DeleteCustomer(int id)
        {
            var value = db.Customers.Find(id);
            if (value != null)
            {
                db.Customers.Remove(value);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateCustomer(int id)
        {
            var value = db.Customers.Find(id);
            return View(value);
        }

        [HttpPost]
        public ActionResult UpdateCustomer(Customer customer)
        {
            var value = db.Customers.Find(customer.CustomerID);
            value.FirstName = customer.FirstName;
            value.LastName = customer.LastName;
            value.Email = customer.Email;
            value.Phone = customer.Phone;
            value.City = customer.City;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
