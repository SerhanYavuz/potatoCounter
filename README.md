[Mikroservis 1. Hafta](craftdocs://open?blockId=ACCD0898-80DA-4EB5-B148-F4A078D80B79&spaceId=2a34d323-6d73-18d7-716e-dc3fdcf98182)

[Mikroservis 2. Hafta ](craftdocs://open?blockId=5EE01C71-5EE5-47D5-8C24-6E2B559FE490&spaceId=2a34d323-6d73-18d7-716e-dc3fdcf98182)
# HAFTA 1
> Microservices - also known as the microservice architecture - is an architectural style that structures an application as a collection of services that are

> - Highly maintainable and testable
> - Loosely coupled
> - Independently deployable
> - Organized around business capabilities
> - Owned by a small team
> The microservice architecture enables the rapid, frequent and reliable delivery of large, complex applications. It also enables an organization to evolve its technology stack.


Mikroservislerin [microservices.io](https://microservices.io)'daki tanımı bu. Mikroservisler konusunda hiç birşey bilmediğimi farkedince ilk baktığım yer burası oldu. Temel buradaki tanımda geçen:  


🛠️ 🥼Hig**hly maintainable and testable:  **Bir yazılımda yapılacak değişikliklerin zorluğu/kolaylığı ve bu değişikliklerin tek başlarına test edilebilmesi ile belirlenir. Burada işi, kolaylaştıran bakım ve testlerin ayrı ayrı (bileşen bazında) yapılabilmesidir.

🧵L**oosely Coupled:**  Bileşenler (bir işi gerçekleştiren kod parçaları) arası bağın zayıf yani kolaylıkla ayrılabilmesi. Bir bileşendeki değişiklikler diğer bir bileşenin performansını veya içeriğini en az derecede etkilemeli. 

🛫I**ndependently Deployable:**   Bir servisin diğerlerinden bağımsız olarak tek başına "canlıya alınabilmesi". Monolith bir yapıda herşeyi çok doğruda yapsak değişikliklerimizi diğer servislerden bağımsız olarak canlıya alamıyoruz (Load balancer arkasında makina makina yapılan değişiklikler HARİÇ :D ). 

🎯O**rganized Around Business Capabilities:** Ürünün teknik yeteneklerinden daha çok ortaya çıkan işe odaklanarak cross-functional takımlarla çalışmayı kolaylaştırır. (ben sonuçta çıkan işe bakıyorum kardeşim!)

⚛️ O**wned By A Small Team:** Servislerin sahipliğinin takımlara verilmesiyle o servisin uzmanlarının oluşmasına olanak verir.

Bütün bu özetten sonra ilk servisi .Net 6 kullanarak geliştirmeye başladım. Bu servisi geliştirmek için bir uygulama aklıma gelmeyincede dünya üzerindeki tüm patateslerin teker teker kaydedileceği bir sistem yapmaya karar verdik. 

Bu örnek projeyi oluşturup çalıştırırken izlediğim adımlar şöyle :

  
1 - .NET SDK'sını indirdim

2 - Yeni bir .NET API projesi oluşturdum 

```other
dotnet new webapi -o PotatoApi
```


3 - Yeni bir controller ekleyerek /potato adresine GET isteği atıldığında patates sayısını 1 arttırıp kullanıcıya sonucu gösterdim. 

```other
using Microsoft.AspNetCore.Mvc;

namespace PotatoCounter.Controllers;

[ApiController]
[Route("[controller]")]
public class PotatoController : ControllerBase
{
    static int potatoCounter = 0;
    private readonly ILogger<PotatoController> _logger;

    public PotatoController(ILogger<PotatoController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public int Get()
    {
        potatoCounter++;
        return potatoCounter;
    }
    
}

```


4 - DigitalOcean üzerinden bir ubuntu sunucu kiralayarak aşağıdaki gibi ayarlarını yaptım.

![image](https://res.craft.do/user/full/2a34d323-6d73-18d7-716e-dc3fdcf98182/doc/05AEB025-272F-40C4-8CD4-B2E24377E79C/1F649284-BC5F-4113-8483-DB9FF599A3B3_2/K2y9MlJJxacvezj1DPlPQF2lcXhxC6XOEyZFx1yxvd8z/ubuntu_comands.png)

5 - FTP kullanarak projenin yayınlanmış halini sunucuya yükleyip, çalıştırdım 

# HAFTA 2


Geçen hafta yayına aldığımız PotatoCounter isimli projenin web arayüzüne herkesin erişmesini engellemek için bir authentication ve authorization metodu uygulamaya karar verdik. 

Bu iş için hazır bir çözüm var mı diye araştırırkende IdentityServer4'e denk geldik ve yapılmış işi tekrar yapmamak için onu kullanmaya karar verdik.

Bu arada Authentication kimlik doğrulamak manasına gelirken, authorization yetki/yetkilendirme demek.   
Önce biraz IdentityServer nedir bundan bahsetmek gerekiyor diye düşünüyorum. 

[https://identityserver4.readthedocs.io/en/latest/](https://identityserver4.readthedocs.io/en/latest/)

identityserver4'ün web sitesindeki tanımı aynen şöyle :   
`IdentityServer4 is an OpenID Connect and OAuth 2.0 framework for [ASP.NET](http://ASP.NET) Core.`

Bu tanımda geçen bilmediğim kelimelere bakmak gerektiğini düşünerek OAuth2.0 ve OpenID Connect nedir araştırmaya başladım . 

**OAuth2.0** 
> TL;DR Vale anahtarı gibi çalışan bir yetkilendirme frameweok'ü 


[OAuth2.0 web sitesi](https://oauth.net/2/)ndeki tanım aynen şöyle.
> OAuth 2.0 is the industry-standard protocol for authorization. OAuth 2.0 focuses on client developer simplicity while providing specific authorization flows for web applications, desktop applications, mobile phones, and living room devices. This specification and its extensions are being developed within the [IETF OAuth Working Group](https://www.ietf.org/mailman/listinfo/oauth).


Yani kabaca özetlemek gerekirse bir yetkilendirme framework'ü. Herhangi bir kaynağa erişiminizin olup olmadığını belirlerken kullanılabileceği manasına geliyor.

OAuth2.0 Yetkilendirme framework'ü, üçüncü parti bir uygulamanın HTTP servislere erişim sağlamasına yarıyor. Bunu yapmanın 2 yolu var :

 1.   Resource owner (kaynağın sahibi, erişim yetkisi olan kişi, kullanıcı) namına HTTP servise gidip RO ile HTTP servis arasında bir yetkilendirme akışı yani login ekranı vb. açarak 


2. Üçüncü parti uygulamanın kendi adına bir erişim metodu (bkz. Api Key) elde etmesini sağlayarak  

Bildiğimiz client-server yetkilendirme modelinde, client erişimi kısıtlı bir kaynağa erişmek için kullanıcının erişim bilgilerini (kullanıcı adı - şifre veya access token) kullanır. Kullanıcı başka bir uygulamaya erişim yetkisi vermek istediğinde kendi erişim bilgilerini bu uygulamayla paylaşır buda birden fazla soruna neden olur. 


- Bu üçüncü parti uygulama benim kullanıcı adı ve şifremi saklamak zorunda cleartext falan tutuyorsa ne olacak?
- Bu üçüncü parti uygulama benim eriştiğim herşeye süresiz yetki kazanmış oluyor ya erişmesini istemediğim bir şeye (facebooktaki eski fotoğraflarım) erişirse.
- Gıcık olduğum bir üçüncü parti uygulamaya yetki vermeyi kesmek için şifremi falan değiştirsem sevdiğim diğer üçüncü parti uygulamalarında erişimini durdurmuş olacağım. Gidip onlara tek tek yeniden şifremi vermek zorunda kalacağım.
- Bu üçüncü parti uygulama bir şekilde hacklenirse(yemeksepeti) bütün hesabım hackleniyor(bkz. I. Dünya harbinde almanya malup olunca bizde ...)

OAuth bu sorunları çözmek için uygulamamızın dışında bir yetkilendirme katmanı oluşturuyor. OAuth varsa üçüncü parti uygulama korunan kaynağa kullanıcınınkinden farklı bir şifre/token ile erişim istediğinden kullanıcıyı koruyoruz. 

Kullanıcının namına hangi kaynağa ne süreyle erişebileceğininde kontrol edilmesini sağlayan bu token içerisinde scope, lifetime ve diğer erişim parametreleri var bunların detaylarını dokümanın devamında. 

**OAuth'un hedefi HTTP servisler olduğundan diğer türler (bkz. socket) ilgi alanına girmiyor 🙂

**OAUTH ROLLERİ (rolleeri amman ammaaaan )**

OAuth toplamda dört rol tanımlıyor : 


1. Resource Owner  
Korunan bir kaynağa erişim yetkisi bahşetmeye yetkili bir varlık (eğer bir insansa end-user olarak isimlendirilir)
2. Resource Server  
Korunan kaynağı (HTTP) barındıran sunucudur. Access token'a bakarak korunan kaynağa erişmek maksadıyla yollanan isteklere cevap verebilir.
3. Client  
Kullanıcı namına korunan kaynağa erişim isteği yapan uygulama. sunucudada çalışabilir kullanıcının elindeki bi cihazdada, farketmez.  
4. Authorization Server  
Resource owner'ın müsadesiyle ve yetkiler uygunsa bir client'a access token veren sunucu.

 PROTOKOL AKIŞI

```other
+--------+                               +---------------+  
|        |--(A)- Authorization Request ->|   Resource    |  
|        |                               |     Owner     |  
|        |<-(B)-- Authorization Grant ---|               |  
|        |                               +---------------+  
|        |  
|        |                               +---------------+  
|        |--(C)-- Authorization Grant -->| Authorization |  
| Client |                               |     Server    |  
|        |<-(D)----- Access Token -------|               |  
|        |                               +---------------+  
|        |  
|        |                               +---------------+  
|        |--(E)----- Access Token ------>|    Resource   |  
|        |                               |     Server    |  
|        |<-(F)--- Protected Resource ---|               |  
+--------+                               +---------------+

```


	(A) Client, resource ownerdan icazet alır. Burda authorization isteği authorization server'a yapılabilir.

	(B) Client, bir authorization grant yani kendisine erişim bahşedildiğini gösterir bir kimlik alır. Bu kimlikte bilinen dört grant türünün (detayları dokümanın devamında) yanı sıra bizim tanımladığımız grant türleride kullanılabilir. 
