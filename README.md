# 🛒 S-Retail: AI Destekli Stratejik Perakende Analiz Paneli

![License](https://img.shields.io/badge/License-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![Dapper](https://img.shields.io/badge/ORM-Dapper-red)
![AI](https://img.shields.io/badge/AI%20Copilot-Google%20Gemini-4285F4)
![Data](https://img.shields.io/badge/Data-AI%20Generated-success)

**S-Retail**, büyük ölçekli bir perakende zincirinin (Online + Fiziksel Mağaza) satış verilerini analiz etmek için geliştirilmiş, **Yapay Zeka destekli** ve performans odaklı bir yönetim panelidir.

Proje, klasik Entity Framework yavaşlığından kaçınarak **Dapper Micro-ORM** mimarisiyle, **1 Milyon+ satırlık** büyük veri setleri üzerinde milisaniyeler içinde sorgulama ve raporlama yapabilir.

---


### Süreç Detayları

1.  **🧠 Senaryo Tasarımı & Veri Mühendisliği:**
    * AI modeline (Gemini) Türkiye'deki gerçek perakende alışkanlıkları ve demografik yapı öğretildi.
    * *Sonuç:* İstanbul, Ankara gibi büyükşehirlerde satış yoğunluğunun artırılması, "Yaz" aylarında dondurma/giyim satışlarının artması gibi **Mevsimsellik ve Lokasyon** mantığı kurgulandı.

2.  **⚡ Sentetik Büyük Veri Üretimi (Synthetic Data Generation):**
    * Manuel veri girişi yerine, AI tarafından yazılan karmaşık **T-SQL Scriptleri** kullanıldı.
    * Bu scriptler sayesinde **1.000.000+** anlamlı, ilişkili ve analiz edilebilir satış kaydı saniyeler içinde veritabanına basıldı.

3.  **🚀 Backend Optimizasyonu & SQL Tuning:**
    * AI, performans darboğazı oluşturabilecek sorguları analiz ederek en optimize SQL sorgularını (Computed Columns, Indexing) önerdi.
    * Veri erişim katmanında **Dapper** kullanılarak EF Core'a göre **%400'e varan hız artışı** sağlandı.

4.  **🎨 UI/UX & Dashboard Tasarımı:**
    * Verilerin anlamlı bir hikayeye dönüşmesi için Google GeoChart ve Chart.js kütüphaneleri, AI desteğiyle modern "Admin Paneli" standartlarına göre uyarlandı.

---

## 📸 Proje Ekran Görüntüleri

| 🌍 Stratejik Dashboard | 🔎 Detaylı Veri Madenciliği |
|------------------------|-----------------------------|
|<img width="1890" height="896" alt="Dashboard Genel Bakış" src="https://github.com/user-attachments/assets/97791396-a5cb-4eef-8754-02b445e7d1a1" /> | <img width="1910" height="899" alt="Filtreleme Ekranı" src="https://github.com/user-attachments/assets/ac8014b3-2262-4e71-b46d-a2e77c1d0efd" /> 
| <img width="1898" height="883" alt="Grafikler ve Analiz" src="https://github.com/user-attachments/assets/3018631a-41d7-4b6e-8426-8542a3cfcbe4" /> | <img width="1882" height="468" alt="Türkiye Haritası" src="https://github.com/user-attachments/assets/b74141b8-1428-40a5-a2b2-b5b5aaf85da5" /> |



---

## ✨ Temel Özellikler

### 1. Büyük Veri Yönetimi (Big Data Handling)
* Veritabanında **1 Milyon+** satır olmasına rağmen, "Server-Side Pagination" (Sunucu Taraflı Sayfalama) sayesinde sayfalar **0.2 saniyede** yüklenir.

### 2. Akıllı Perakende Metrikleri
* **Omnichannel Analizi:** Online (S-Online) ve Fiziksel Mağaza (Mega, Express) satışlarının karşılaştırılması.
* **Coğrafi Isı Haritası:** Türkiye haritası üzerinde il bazlı ciro yoğunluğu (İstanbul'un koyu turuncu yanması vb.).
* **Finansal KPI'lar:** Toplam Ciro, Sepet Ortalaması, Tahmini Net Kâr.

### 3. Gelişmiş Filtreleme Motoru
* Kullanıcılar milyonlarca kayıt arasından; **Fiş No, Ürün Adı, Fiyat Aralığı, Tarih ve Mağaza Formatına** göre anlık arama yapabilir.

---

## 🛠️ Teknoloji Yığını (Tech Stack)

* **Backend:** ASP.NET Core 6.0 MVC
* **Veritabanı:** MSSQL Server (SQL Scripts & Stored Procedures)
* **ORM:** Dapper (Micro-ORM)
* **Frontend:** Bootstrap 5, JavaScript, jQuery
* **Veri Görselleştirme:** Google Charts (GeoChart), Chart.js
* **Raporlama:** EPPlus (Excel Export)
* **AI Tools:** Google Gemini (Prompt Engineering & Data Simulation)

## 🚀 Kurulum Adımları

1.  Repoyu klonlayın.
2.  `appsettings.json` içindeki bağlantı cümlesini düzenleyin.
3.  **Database** klasöründeki `Generate_BigData.sql` dosyasını çalıştırın (Bu script AI tarafından üretilen 1 milyon veriyi basacaktır).
4.  Projeyi başlatın.

## 👤 Geliştirici & Katkıda Bulunanlar

**Merve Yıldız** - *Lead Developer*
* [LinkedIn](https://www.linkedin.com/in/aysemerveyildiz/)
* [GitHub](https://github.com/AyseMerveYildiz/DapperRetailSalesProject)

**Google Gemini** - *AI Pair Programmer*
* *Rolü:* Veri simülasyonu, SQL optimizasyonu ve kod refactoring desteği.
## 🤖 AI Destekli Geliştirme İş Akışı (Human-AI Collaboration)

Bu proje, **Developer (Merve Yıldız)** ve **AI Pair Programmer (Google Gemini)** işbirliği ile geliştirilmiştir. Kodlamanın ötesinde, bir veri simülasyonu ve iş zekası senaryosu kurgulanmıştır.

### 🧠 Nasıl Çalışır? (Teknik Mimari & AI Akışı)

Projenin veri üretiminden son kullanıcıya ulaşan teknik yolculuğu aşağıdaki gibidir:

```mermaid
graph TD
    subgraph "AI & Veri Mühendisliği"
        A[👨‍💻 Merve Yıldız<br/>Senaryo Tasarımı] -->|1. Prompt Engineering| B(🤖 Google Gemini AI)
        B -->|2. T-SQL Script Üretimi| C{📜 Generate_BigData.sql}
    end

    subgraph "Backend & Data Layer"
        C -->|3. Veri Enjeksiyonu| D[(🗄️ MSSQL Server<br/>1 Milyon+ Kayıt)]
        E[⚡ ASP.NET Core MVC<br/>Dapper Micro-ORM] -->|4. Optimize Sorgu| D
        D -->|5. Sonuç Dönüşü<br/>Latency: < 200ms| E
    end

    subgraph "Frontend & UI"
        E -->|6. JSON Data| F[📊 S-Retail Dashboard<br/>Chart.js & GeoChart]
    end
```

---
*Bu proje, yapay zeka destekli sentetik veri üretimi ve yüksek performanslı veri işleme yeteneklerini sergilemek amacıyla geliştirilmiştir.*
