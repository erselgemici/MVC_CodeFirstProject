using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using MyAcademy_MVC_CodeFirst.Filters;
using PagedList;
using Serilog;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        AppDbContext db = new AppDbContext();

        [LogAction(ActionDescription = "Müşteri Listesine Erişim Sağladı")]
        public ActionResult Index(string search, int page = 1)
        {
            // PERFORMANS: db.Customers.ToList() demiyoruz
            // AsQueryable() diyerek sorguyu henüz veritabanına göndermiyoruz.
            // Sadece "Ben bu tabloyla çalışacağım" diye hazırlık yapıyoruz.
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
            string name = value.FirstName + " " + value.LastName; 

            db.Customers.Remove(value);
            db.SaveChanges();

            Log.Warning("Kullanıcı: {UserEmail} | İşlem: DeleteCustomer | Mesaj: {Message} | Detay: {Data}",
                Session["Email"],
                $"{name} isimli müşteri silindi.",
                Newtonsoft.Json.JsonConvert.SerializeObject(new { ID = id, Name = name }));

            return RedirectToAction("Index");
        }

        [HttpGet]
        [LogAction(ActionDescription = "Müşteri Bilgilerini Güncelledi")]
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
