# Zimmetly API

Zimmetly temel, kullanıcı dostu bir zimmet dosyası oluşturma programıdır. Kullanıcının, dilediği platformdan erişebilmesi için API Olarak tasarlanmıştır.

## Özellikler

- Ürün eklemesi, güncellenmesi ve silinmesi.
- Ürüne ait fotoğrafların eklenmesi, silinmesi ve güncellenmesi.
- İlgili ürüne ait, çeşitli alanlardan oluşan zimmet belgesinin oluşturulması.
- Arama ve filtreleme.

## Kurulum
### Gereksinimler
- .NET 7 Geliştirme ortamı.
- ORM Tabanlı geliştirme için uygun bir veritabanı sunucusu (SQL, PostgreSQL, vb.)
- Deployment için uygun hosting, web sunucusu.

### Adımlar
Projeyi klonla :
```bash
git clone https://github.com/maksatgw/Zimmetly.git
cd Zimmetly
```
Veritabanı ayarlarını yap :

```bash
dotnet user-secrets init
```

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=myServerAddress;Database=myDataBase;User Id=myUsername;"
```

Program.cs :
```csharp
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
});
```
Migrationlar :
```bash
dotnet ef database update
```
Projeyi çalıştır:
```bash
dotnet run
```
## Kullanım

- Development ortamında Swagger dokümantasyonundan yararlanabilirsiniz.

## Katkıda Bulunmak

Pull requestler memnuniyetle karşılanır. Hata durumunda ve major değişiklik talebi için issue açabilirsiniz.


## License

[MIT](https://choosealicense.com/licenses/mit/)
