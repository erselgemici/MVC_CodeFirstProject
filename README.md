# LifeSure - AI & ML Powered Insurance Platform 🛡️🤖

**LifeSure**, klasik sigortacılık işlemlerini **Yapay Zeka (AI)**, **Makine Öğrenimi (ML)** ve **Büyük Veri (Big Data)** teknolojileriyle birleştiren modern bir yönetim panelidir. ASP.NET Core 8.0 mimarisi üzerine inşa edilmiş olup, Docker üzerinde koşan Elasticsearch altyapısı ile gelişmiş loglama yeteneklerine sahiptir.

## 🚀 Öne Çıkan Özellikler

### 1. 🌍 Multi-Language & AI Smart Support
- **Localization:** Yabancı dil desteği.
- **Cross-Language AI Response:** İletişim formundan gelen mesajın dili (Örn: Almanca) AI tarafından algılanır.
- **Auto-Reply:** ChatGPT entegrasyonu sayesinde, müşteriye **kendi dilinde** ve bağlama uygun profesyonel bir yanıt oluşturulup otomatik olarak e-posta ile gönderilir.

### 2. 📊 Machine Learning (ML.NET)
- **Sales Prediction Modeli:** Geçmiş verileri analiz ederek gelecek dönem satış tahminlemesi (Regression) yapar.
- Yönetim paneli üzerinden tahmin sonuçları grafiksel olarak izlenebilir.

### 3. 🐳 Big Data & Monitoring
- **Elasticsearch & Kibana:** Tüm sistem logları Docker üzerinde çalışan NoSQL veritabanında tutulur.
- **Advanced Logging:** `[LogAction]` ve `IExceptionFilter` yapılarıyla; kimin, ne zaman, hangi veriyi değiştirdiği (Audit Log) JSON formatında saklanır.

### 4. 🏗️ Mimari ve Tasarım (Architecture)
Proje **ASP.NET MVC 5** framework'ü üzerinde geliştirilmiştir:
- **Code First Approach:** Veritabanı tabloları C# sınıfları (Entities) üzerinden oluşturulmuştur (`Migration` yapısı kullanılmıştır).
- **Modular Design:** `Areas` (Admin, Staff, User) yapısı ile modülerlik sağlanmıştır.
- **Service Layer:** İş mantığı `Services` klasöründe soyutlanmıştır.
- **Custom Filters:** `ActionFilterAttribute` ve `IExceptionFilter` ile merkezi hata ve log yönetimi yapılmıştır.

## 🛠️ Teknoloji Stack'i

| Kategori | Teknolojiler |
|----------|--------------|
| **Backend** | .NET Framework, ASP.NET MVC 5 |
| **Veritabanı** | MSSQL (Entity Framework 6 Code First), Elasticsearch |
| **AI & ML** | ML.NET, OpenAI API, HuggingFace API |
| **DevOps** | Docker (Elastic Stack için) |
| **Logging** | Serilog, Kibana |
| **Frontend** | Bootstrap 5, HTML5, jQuery, SweetAlert2 |
| **Config** | Web.config |

<img width="946" height="317" alt="ss0" src="https://github.com/user-attachments/assets/830eda4c-6c65-459b-b63d-05d53efa4636" />
<img width="944" height="473" alt="ss1" src="https://github.com/user-attachments/assets/be8523d5-c38f-4f9a-8f87-bb9eed7b2c68" />
<img width="947" height="475" alt="ss2" src="https://github.com/user-attachments/assets/95f1207c-72ce-4b04-8efe-74a606345948" />
<img width="947" height="402" alt="ss3" src="https://github.com/user-attachments/assets/1ac94898-b47a-497f-841b-7482af00a1f5" />
<img width="947" height="473" alt="ss4" src="https://github.com/user-attachments/assets/85539e59-fbeb-4d43-929d-f3a0e00780c7" />
<img width="944" height="266" alt="ss5" src="https://github.com/user-attachments/assets/bd7b45f7-4655-4994-895f-5e14c71fb6dd" />
<img width="947" height="113" alt="ss6" src="https://github.com/user-attachments/assets/312ddb39-47b1-4f9a-a535-42d831a16f11" />
<img width="947" height="377" alt="ss7" src="https://github.com/user-attachments/assets/bf05acb4-44a1-45c3-bf79-c6c172ad4cf0" />
<img width="948" height="459" alt="ss8" src="https://github.com/user-attachments/assets/f7dccba8-0555-430d-bd35-0ad24c7047b1" />
<img width="149" height="140" alt="ss10" src="https://github.com/user-attachments/assets/a8964ddc-d3f8-4c32-bd9e-34eeee9866c0" />
<img width="945" height="474" alt="ss11" src="https://github.com/user-attachments/assets/0f145037-e607-433a-a278-1974c47bc11b" />
<img width="946" height="474" alt="ss12" src="https://github.com/user-attachments/assets/47cd6baa-ee11-4be1-b1da-6fab237cdb66" />
<img width="953" height="473" alt="ss13" src="https://github.com/user-attachments/assets/5f1144bb-2759-47c1-bf90-32eb550f72aa" />
<img width="915" height="401" alt="ss14" src="https://github.com/user-attachments/assets/28ad2935-3385-4f27-abd6-4625f807adfb" />
<img width="944" height="457" alt="ss15" src="https://github.com/user-attachments/assets/bc89ce50-369b-487f-b8db-e8f045290885" />
<img width="953" height="458" alt="ss16" src="https://github.com/user-attachments/assets/06cfc4b2-d4c0-47e6-a9f1-a3255fffb722" />
<img width="920" height="435" alt="ss17" src="https://github.com/user-attachments/assets/8308b0da-aa68-469a-882a-33c9fd0bb4fc" />
<img width="945" height="458" alt="ss18" src="https://github.com/user-attachments/assets/ffffaf99-2240-4a7b-a157-ada58787e033" />
<img width="953" height="457" alt="ss19" src="https://github.com/user-attachments/assets/20c1e25d-fa32-4814-9ab4-3d21518ab36b" />
<img width="716" height="245" alt="ss20" src="https://github.com/user-attachments/assets/aa1b9e09-7866-4456-975f-7dbe4ae368cc" />
<img width="149" height="168" alt="ss21" src="https://github.com/user-attachments/assets/7b6615c2-e274-4678-bc24-1e935cdd95aa" />
<img width="948" height="418" alt="ss22" src="https://github.com/user-attachments/assets/67d692f2-f151-47ce-ad73-71ee4c211d92" />
<img width="949" height="450" alt="ss23" src="https://github.com/user-attachments/assets/573a9752-1334-41f6-b507-0a4427257a67" />
<img width="955" height="460" alt="ss24" src="https://github.com/user-attachments/assets/2330bb9d-ce6a-4081-ab0c-7ba44a47fda7" />
<img width="953" height="446" alt="ss25" src="https://github.com/user-attachments/assets/0a0d8bdd-6f54-4de2-8b94-96fe0f829c01" />
<img width="956" height="451" alt="ss26" src="https://github.com/user-attachments/assets/c2d9caac-116d-4031-9caf-ea1c6b7b78f5" />
<img width="947" height="473" alt="ss27" src="https://github.com/user-attachments/assets/710ba06a-ecb4-4d53-a720-b2268ccce84a" />
<img width="955" height="475" alt="ss28" src="https://github.com/user-attachments/assets/cbc1b628-91a2-48a6-bf27-f5e647af6825" />
<img width="247" height="98" alt="ss29" src="https://github.com/user-attachments/assets/03613b7c-b45e-46c9-99bb-2ed6fef843af" />
<img width="952" height="297" alt="ss30" src="https://github.com/user-attachments/assets/27c82848-ba70-4023-b4b6-fbe4c3443313" />
<img width="952" height="288" alt="ss31" src="https://github.com/user-attachments/assets/8fcb3044-b369-4aa8-bb5e-852a1ae98c7b" />
<img width="950" height="473" alt="ss32" src="https://github.com/user-attachments/assets/5d779285-4adf-4e9d-aef6-bbbdd5ce8cf1" />
<img width="953" height="461" alt="ss33" src="https://github.com/user-attachments/assets/9bcf0174-2dac-4c97-a729-6054df38c2fb" />
<img width="944" height="472" alt="ss34" src="https://github.com/user-attachments/assets/97111a67-b88f-48fc-a67c-63905917164c" />
<img width="956" height="516" alt="ss35" src="https://github.com/user-attachments/assets/eff1be30-7985-48a7-ad68-dd8d5d1e5fff" />
<img width="946" height="470" alt="ss36" src="https://github.com/user-attachments/assets/be44bcef-4ebb-4efe-bf10-1a6948ad2677" />
<img width="953" height="472" alt="ss37" src="https://github.com/user-attachments/assets/9d26687c-3b22-435b-8667-66d815f63ed2" />



