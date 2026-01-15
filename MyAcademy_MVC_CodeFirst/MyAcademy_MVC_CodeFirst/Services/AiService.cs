using MyAcademy_MVC_CodeFirst.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MyAcademy_MVC_CodeFirst.Services
{
    public class AiService
    {
        // HUGGING FACE İLE KATEGORİ TAHMİNİ

        public async Task<string> GetCategoryWithHuggingFace(string text)
        {
            string lowerText = text.ToLower(new System.Globalization.CultureInfo("tr-TR"));

            // Şikayet veya Acil Durum Kelimeleri
            if (lowerText.Contains("şikayet") ||
                lowerText.Contains("sorun") ||
                lowerText.Contains("hata") ||
                lowerText.Contains("geri dön") ||
                lowerText.Contains("ulaşın") ||
                lowerText.Contains("bekliyorum") ||
                lowerText.Contains("memnun değilim") ||
                lowerText.Contains("rezalet"))
            {
                return "Şikayet / Destek Talebi";
            }

            if (lowerText.Contains("teşekkür") || lowerText.Contains("sağol") || lowerText.Contains("harika"))
            {
                return "Teşekkür / Memnuniyet";
            }


            // Eğer yukarıdaki kelimeler yoksa, duygu analizini AI yapsın.

            var apiKey = WebConfigurationManager.AppSettings["HuggingFaceKey"];
            if (string.IsNullOrEmpty(apiKey)) return "Genel (Key Yok)";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                var url = "https://router.huggingface.co/hf-inference/models/lxyuan/distilbert-base-multilingual-cased-sentiments-student";

                var requestBody = new { inputs = text };
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var resultString = await response.Content.ReadAsStringAsync();
                        dynamic result = JsonConvert.DeserializeObject(resultString);

                        string label = result[0][0].label.ToString().ToLower();

                        if (label == "positive") return "Teşekkür / Memnuniyet";
                        if (label == "negative") return "Şikayet / Destek Talebi";

                        return "Bilgi Talebi";
                    }
                }
                catch
                {
                    return "Genel Talep";
                }
            }

            return "Genel Talep";
        }

        // CHATGPT İLE DİL ALGILAMA VE CEVAP YAZMA
        public async Task<AiResponseViewModel> GenerateReplyWithChatGPT(string messageBody, string category)
        {
            var apiKey = WebConfigurationManager.AppSettings["OpenAiKey"];
            var url = "https://api.openai.com/v1/chat/completions";

            if (string.IsNullOrEmpty(apiKey))
            {
                return new AiResponseViewModel
                {
                    EmailReply = "Sistem hatası: API anahtarı bulunamadı.",
                    UiTitle = "Konfigürasyon Hatası",
                    UiText = "Lütfen web.config dosyasındaki 'OpenAiKey' alanını kontrol edin.",
                    UiButton = "Tamam"
                };
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var prompt = $@"
                GÖREV: Aşağıdaki mesaja '{category}' kategorisinde kurumsal bir cevap yaz.
                
                Mesaj: '{messageBody}'

                ÖNEMLİ KURALLAR:
                1. Mesajın dilini algıla (Türkçe, İngilizce, Japonca vb.).
                2. Cevabı O DİLDE yaz.
                3. UI (Web Sitesi) için başlık ve buton metinlerini de O DİLDE hazırla.
                
                ÇIKTI FORMATI (Sadece bu JSON'ı döndür):
                {{
                    ""EmailReply"": ""...mail içeriği..."",
                    ""UiTitle"": ""...ekran başlığı (Örn: Mesaj Alındı)..."",
                    ""UiText"": ""...ekran mesajı..."",
                    ""UiButton"": ""...buton yazısı...""
                }}";

                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                    new { role = "system", content = "Sen JSON formatında yanıt veren, çok dilli bir asistansın." },
                    new { role = "user", content = prompt }
                }
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new AiResponseViewModel
                    {
                        EmailReply = "Sistem yoğunluğu nedeniyle şu an cevap oluşturulamadı.",
                        UiTitle = "API Bağlantı Hatası",
                        UiText = $"Hata Kodu: {response.StatusCode} - Detay: {responseString}",
                        UiButton = "Kapat"
                    };
                }

                try
                {
                    dynamic result = JsonConvert.DeserializeObject(responseString);
                    string aiContent = result.choices[0].message.content.ToString();

                    // Markdown temizliği
                    aiContent = aiContent.Replace("```json", "").Replace("```", "").Trim();

                    return JsonConvert.DeserializeObject<AiResponseViewModel>(aiContent);
                }
                catch (Exception ex)
                {
                    return new AiResponseViewModel
                    {
                        EmailReply = "Mesajınız alındı.",
                        UiTitle = "Format Hatası",
                        UiText = "Yapay zeka cevabı okunamadı: " + ex.Message,
                        UiButton = "Tamam"
                    };
                }
            }
        }
    }
}


