using Serilog;
using System;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Filters
{
    public class LogActionAttribute : ActionFilterAttribute
    {
        public string ActionDescription { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            // Sadece giriş yapmış kullanıcı hareketlerini loglayalım
            //if (session["UserID"] != null)
            //{
                // Log verilerini hazırla
                string userEmail = session["Email"]?.ToString() ?? "Anonim";
                string role = session["Role"]?.ToString() ?? "Yok";
                string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string action = filterContext.ActionDescriptor.ActionName;
                string ip = filterContext.HttpContext.Request.UserHostAddress;

                // Mesaj
                string message = string.IsNullOrEmpty(ActionDescription)
                    ? $"{controller} üzerindeki {action} işlemi tamamlandı."
                    : ActionDescription;

                // ELASTICSEARCH'E GÖNDER
                Log.Information("Kullanıcı: {UserEmail} | Rol: {Role} | İşlem: {ActionType} | Detay: {Message} | IP: {IP}",
                    userEmail, role, action, message, ip);
            //}

            base.OnActionExecuted(filterContext);
        }
    }
}
