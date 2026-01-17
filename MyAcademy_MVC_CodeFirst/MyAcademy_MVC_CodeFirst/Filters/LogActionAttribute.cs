using System;
using System.Web.Mvc;
using Serilog;
using Newtonsoft.Json;

namespace MyAcademy_MVC_CodeFirst.Filters
{
    // ActionFilterAttribute'tan miras alarak "Metot çalışmadan önce/sonra" araya girme yeteneği kazanıyoruz.
    // IExceptionFilter'dan miras alarak "Hata olduğunda" devreye girme yeteneği kazanıyoruz.
    // Bu yapı sayesinde her Controller'a tek tek kod yazmak yerine, merkezi bir "Güvenlik Kamerası" kurmuş oluyoruz.
    public class LogActionAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public string ActionDescription { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // O anki kullanıcının Session (Oturum) bilgilerine ulaşıyoruz.
            var session = filterContext.HttpContext.Session;

            // Logu kimin oluşturduğunu bilmemiz lazım.
            string userEmail = session["Email"]?.ToString() ?? session["Mail"]?.ToString() ?? "Anonim";
            string role = session["Role"]?.ToString() ?? "Yok";

            // İsteğin geldiği IP adresini yakalıyoruz.
            string ip = filterContext.HttpContext.Request.UserHostAddress;

            // Hangi Controller ve Hangi Action çalışıyor?
            string action = filterContext.ActionDescriptor.ActionName;

            // Metoda gönderilen parametreleri (Form verileri, ID'ler vs.) yakalıyoruz.
            var parameters = filterContext.ActionParameters;
            string dataDetail = "Veri Yok";

            // Eğer metoda gelen bir veri varsa (Örn: Kayıt formu doluysa)
            if (parameters != null && parameters.Count > 0)
            {
                try
                {
                    // C# Nesnesini (Object) JSON formatına (String) çeviriyoruz.
                    // ReferenceLoopHandling: Birbiriyle ilişkili tablolarda sonsuz döngüye girip hata vermesini engelliyoruz.
                    // "Kullanıcı tam olarak hangi veriyi girmeye çalıştı?" sorusunun cevabı burada saklı.
                    dataDetail = JsonConvert.SerializeObject(parameters, Formatting.None,
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }
                catch
                {
                    dataDetail = "Veri okunamadı (Serialization Hatası)";
                }
            }

            // Serilog kütüphanesini kullanarak Elasticsearch'e "Info" seviyesinde log atıyoruz.
            Log.Information("Kullanıcı: {UserEmail} | Rol: {Role} | İşlem: {ActionType} | Mesaj: {Message} | Detay: {Data} | IP: {IP}",
                userEmail,
                role,
                action,
                ActionDescription, 
                dataDetail,        
                ip);

            // İşlemi devam ettir (Controller'daki asıl metodu çalıştır).
            base.OnActionExecuting(filterContext);
        }

        // SİSTEMDE BİR HATA (EXCEPTION) OLDUĞUNDA BURASI TETİKLENİR
        public void OnException(ExceptionContext filterContext)
        {
            // Hatanın teknik mesajını (Örn: NullReferenceException) alıyoruz.
            string errorMessage = filterContext.Exception.Message;

            // StackTrace, hatanın kodun hangi dosyasında ve kaçıncı satırında olduğunu söyler.
            string stackTrace = filterContext.Exception.StackTrace;

            // Hata anında da kullanıcı bilgilerini tekrar çekiyoruz (Kim hata aldı?).
            var session = filterContext.HttpContext.Session;
            string userEmail = session["Email"]?.ToString() ?? "Anonim";
            string role = session["Role"]?.ToString() ?? "Yok";
            string ip = filterContext.HttpContext.Request.UserHostAddress;

            // Hangi sayfada hata alındı?
            string action = filterContext.RouteData.Values["action"].ToString();

            Log.Error("Kullanıcı: {UserEmail} | Rol: {Role} | İşlem: {ActionType} | Mesaj: {Message} | Detay: {Data} | IP: {IP}",
                userEmail,
                role,
                action,
                "SİSTEM HATASI: " + errorMessage, 
                stackTrace,                       
                ip);

        }
    }
}
