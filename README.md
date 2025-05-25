# BarcodeGeneratorSystem API

BarcodeGeneratorSystem, GS1 uyumlu 12 haneli barkod üretimi, doğrulaması ve yönetimi için geliştirilmiş bir .NET 8 Web API servisidir.  
Bu servis, barkodların otomatik olarak üretilmesi, veritabanına kaydedilmesi ve doğrulanmasını sağlar.

---

## Özellikler

- **Barkod Üretimi:** Rastgele geçerli GTIN-12 barkod üretir.  
- **Barkod Doğrulama:** GS1 standardına göre barkod doğrulaması yapar.  
- **Barkod Listeleme:** Tüm kayıtlı barkodların listelenmesi.  
- **Barkod Detayı:** ID bazında barkod bilgisi alınması.  
- **Token Doğrulama:** Gönderilen JWT token'ın geçerliliğinin kontrolü.  
- **Mock ERP Ürünleri:** Örnek ERP ürün listesini döner.  
- **ERP Ürünlerini İçe Aktarma:** ERP'den ürünleri çekip veritabanına kaydeder.

---

## 🔧 Kurulum ve Bağımlılıklar

### Kullanılan Teknolojiler
- **Backend:** ASP.NET Core, Dapper
- **Frontend:** Vue.js (Vuetify ile)
- **Veritabanı:** SQL Server

### Gereksinimler

- **[.NET 8 SDK]**
- **SQL Server (System.Data.SqlClient ile uyumlu)**
- **Vue.js 3.5.8 veya üzeri**

---

### 📦 Kullanılan Kişiselleştirilmiş NuGet Paketleri

| Paket | Açıklama |
|-------|----------|
| `Moonlight.ExceptionHandling (1.0.0)` | Merkezileştirilmiş hata yönetimi(CoreException("Record Not Found")) |
| `Moonlight.Response (1.0.0)` | Standart API yanıt yapısı (CoreResponse) |


---

## API Endpoints

### Barcode Servisi

#### 1. POST `/api/barcode/codes`

**Açıklama:** Yeni bir barkod üretir ve veritabanına kaydeder.  
**Yetkilendirme:** Gerekli (Authorize).  
**Request Body:** Yok.  
**Response:** Barkod bilgileri (`Barcodes`) ve işlem sonucu.

#### 2. GET `/api/barcode/codes`

**Açıklama:** Tüm barkodları listeler.  
**Yetkilendirme:** Yok.  
**Response:** Barkodların listesi (`IEnumerable<Barcodes>`).

#### 3. GET `/api/barcode/codes/{id}`

**Açıklama:** Verilen ID'ye sahip barkodu getirir.  
**Yetkilendirme:** Yok.  
**Response:** İlgili barkod bilgisi (`Barcodes`).

#### 4. POST `/api/barcode/validate`

**Açıklama:** Gönderilen barkodun GS1 uyumlu olup olmadığını kontrol eder.  
**Yetkilendirme:** Gerekli (Authorize).  
**Request Body:** `BarcodeRequest` (örnek: `{ "Gtin12": "123456789012" }`)  
**Response:** Doğrulama sonucu (`BarcodeValidateResponse`).

---

### Auth Servisi

#### 5. POST `/api/auth/tokenVerify`

**Açıklama:** Gönderilen JWT token'ın geçerliliğini kontrol eder.  
**Yetkilendirme:** Yok (Token kendisi doğrulanacak).  
**Request Body:** `ValidateRequest` (örnek: `{ "Token": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." }`)  
**Response:** Token geçerliyse `true`, değilse `false` ve hata mesajları.

---

### Mock ERP Servisi

#### 6. GET `/api/erpMockData`

**Açıklama:** Örnek ERP ürün listesini döner.  
**Yetkilendirme:** Yok.  
**Response:** `CoreResponse<IEnumerable<ErpProductResponse>>`

---

### Product Servisi
#### 7. GET `/api/products/external`

**Açıklama:**  
ERP servisinden ürün verilerini alır, yeni ürünleri veritabanına kaydeder ve işlem sonucunu döner.

**Yetkilendirme:** Gerekli (Authorize).

**Response:**  
`CoreResponse<IEnumerable<ErpProductResponse>>`  
- ERP’den alınan ürünlerin listesi  
- İşlem sonucu mesajı  
- Hata mesajları (varsa)

**Başarı Durumu:**  
- ERP’den ürün alınamazsa `NoData` kodu ve uygun mesaj döner.  
- Yeni veri yoksa, "Aktarılacak yeni veri yok" mesajı ile başarı durumu döner.  
- Yeni ürünler başarıyla kaydedilirse, kaydedilen ürün sayısı ile birlikte başarı mesajı döner.

---

### User Servisi

#### 8. Kullanıcı İşlemleri

##### GET `/api/user`

**Açıklama:**  
Kayıtlı tüm kullanıcıların listesini döner.  
**Yetkilendirme:** Yok.  
**Response:**  
`CoreResponse<IEnumerable<Users>>`  
- Kullanıcı listesi  
- Eğer kullanıcı yoksa `NoData` kodu ve uygun mesaj döner.

---

##### POST `/api/user/register`

**Açıklama:**  
Yeni kullanıcı kaydı oluşturur.  
**Yetkilendirme:** Yok.  
**Request Body:** `Users` modeli (örnek: `{ "UserName": "...", "Password": "...", ... }`)  
**Response:**  
`CoreResponse<Users>`  
- Oluşturulan kullanıcı bilgisi  
- Başarı mesajı.

---

##### PUT `/api/user/{id}`

**Açıklama:**  
Belirtilen ID’ye sahip kullanıcının bilgilerini günceller.  
**Yetkilendirme:** Gerekli (Authorize).  
**Request Body:** Güncellenmiş `Users` modeli.  
**Response:**  
`CoreResponse<Users>`  
- Güncellenmiş kullanıcı bilgisi  
- Başarı mesajı.

---

##### PUT `/api/user/delete/{id}`

**Açıklama:**  
Belirtilen ID’ye sahip kullanıcıyı siler.  
**Yetkilendirme:** Gerekli (Authorize).  
**Response:**  
`CoreResponse<bool>`  
- Silme işleminin sonucu (true/false)  
- Başarı mesajı.

---

##### POST `/api/user/login`

**Açıklama:**  
Kullanıcı adı ve şifre ile giriş yapar, JWT token üretir.  
**Yetkilendirme:** Yok.  
**Request Body:** `LoginRequest` (örnek: `{ "UserName": "kullanici", "Password": "sifre" }`)  
**Response:**  
`CoreResponse<string>`  
- JWT token (başarılı girişte)  
- Başarısızsa hata mesajları.




