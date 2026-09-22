# 🚀 Microservices Architecture - ToDo Application

Bu proje, **Mikroservis Mimarisi (Microservices Architecture)** prensiplerini ve modern backend/frontend teknolojilerini öğrenmek/uygulamak amacıyla geliştirilmekte olan uçtan uca bir ToDo uygulamasıdır.

Dağıtık sistemler, servisler arası bağımsızlık, veritabanı ayrımı (Database-per-service), frontend entegrasyonu ve container orkestrasyonu gibi kavramlar uygulamalı olarak projenin mimarisine dahil edilmektedir.

---

## 🛠️ Mimari ve Teknolojik Altyapı

### **Mevcut Durum (Aşama 1)**
- **UserService:** Kullanıcı kaydı, girişi ve kimlik doğrulama işlemleri.
- **Database (PostgreSQL):** Servise özel izole veritabanı yapısı.
- **Redis Cache/Store:** Access ve Refresh Token yönetimi, hızlı oturum ve oturum sonlandırma süreçleri için entegre edildi.
- **Docker & Docker Compose:** Tüm bağımlılıklar ve servisler containerized olarak tek komutla ayağa kalkmaktadır.

### **Geliştirme Yol Haritası (Roadmap)**
- [x] UserService & Veritabanı entegrasyonu
- [x] Redis ile Access/Refresh Token yönetimi
- [x] Dockerfile ve Docker Compose yapılandırması
- [ ] **TodoService & Veritabanı:** Görev yönetimi ve CRUD operasyonları
- [ ] **RabbitMQ Entegrasyonu:** Servisler arası asenkron iletişim ve event-driven mimari
- [ ] **API Gateway:** Tüm servislere tek bir giriş noktası üzerinden güvenli erişim
- [ ] **Angular Frontend:** Kullanıcı dostu arayüz tasarımı ve servis entegrasyonları
- [ ] **Kubernetes (k8s):** Container orkestrasyonu ve deployment süreçleri

---

## 🏗️ Proje Mimarisi

```
                    +-------------------+
                    |    Angular UI     | (Planlanıyor)
                    +---------+---------+
                              |
                              v
                    +-------------------+
                    |    API Gateway    | (Planlanıyor)
                    +---------+---------+
                              |
            +-----------------+-----------------+
            |                                   |
            v                                   v
  +-------------------+               +-------------------+
  |    UserService    |               |    TodoService    | (Geliştiriliyor)
  +---------+---------+               +---------+---------+
            |                                   |
     +------+------+                     +------+------+
     |             |                     |             |
     v             v                     v             v
+----------+ +-----------+          +----------+ +-----------+
| PostgreSQL| |   Redis   |          | PostgreSQL| | RabbitMQ  |
|   (User) | |  (Token)  |          |  (Todo)  | |  (Events) |
+----------+ +-----------+          +----------+ +-----------+
```

---

## 🚀 Kurulum ve Çalıştırma

Projeyi yerel ortamınızda çalıştırmak için **Docker** ve **Docker Compose** yüklü olması yeterlidir.

1. **Repoyu klonlayın:**
   ```bash
   git clone https://github.com/kullanici-adiniz/todo-microservices.git
   cd todo-microservices
   ```

2. **Docker Compose ile servisleri başlatın:**
   ```bash
   docker-compose up -d --build
   ```

3. Servislerin durumunu kontrol etmek için:
   ```bash
   docker-compose ps
   ```

---

## 🔒 Güvenlik ve Token Yönetimi

- **Access Token:** Kısa ömürlü JWT.
- **Refresh Token:** Redis üzerinde saklanır ve güvenli oturum yenileme mekanizması sağlar.
- **Logout:** Redis üzerinden ilgili token silinerek/karalisteye alınarak anında geçersiz kılınır.

---

## 🤝 İletişim & Katkı

Projeyi geliştirmeye devam ediyorum. Öneri, eleştiri ve PR'lara açıktır!

- **LinkedIn:** [Mehmet GÜNDÜZ](https://www.linkedin.com/in/mehmet-g%C3%BCnd%C3%BCz00/)
- **GitHub:** [Spacend00](https://github.com/Spacend00)