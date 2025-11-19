# Dependency Injection trong ASP.NET Core

## 1. Services (Dịch vụ)

**Services** là các lớp chịu trách nhiệm triển khai logic nghiệp vụ cốt lõi của ứng dụng.

### Mục đích chính của Services

- **Đóng gói Business Logic**: Tách biệt logic phức tạp khỏi presentation layer (controllers và views)
- **Tái sử dụng**: Một service có thể được dùng bởi nhiều controllers
- **Dễ kiểm thử**: Services có thể được unit test độc lập
- **Dependency Injection**: Services thường được đăng ký trong DI container

### Nhiệm vụ thường gặp

- **Data Access**: Giao tiếp với database để fetch, insert, update, delete data
- **Business Rules**: Thực thi các quy tắc nghiệp vụ (validation, tính toán, transformations)
- **Integration**: Tương tác với external systems hoặc APIs
- **Notifications**: Gửi emails, SMS
- **Logging**: Ghi lại events và errors

### Ví dụ Code

```csharp
// CitiesService.cs (Service)
namespace Services
{
    public class CitiesService
    {
        private List<string> _cities;

        public CitiesService() 
        {
            _cities = new List<string>() { "London", "Paris", "New York", "Tokyo", "Rome" };
        }
 
        public List<string> GetCities()
        {
            return _cities;
        }
    }
}

// HomeController.cs (Controller)
public class HomeController : Controller
{
    private readonly CitiesService _citiesService;

    public HomeController() 
    {
        _citiesService = new CitiesService();
    }
 
    [Route("/")]
    public IActionResult Index()
    {
        List<string> cities = _citiesService.GetCities();
        return View(cities);
    }
}
```

> **Lưu ý**: Ví dụ trên chưa sử dụng dependency injection. Trong dự án thực tế, nên đăng ký services với DI container của ASP.NET Core.

---

## 2. Dependency Inversion Principle (DIP)

**DIP** là nguyên tắc thiết kế giúp code linh hoạt và ít phụ thuộc:

- **High-level modules** không nên phụ thuộc vào **low-level modules**. Cả hai nên phụ thuộc vào **abstractions** (interfaces)
- **Abstractions** không nên phụ thuộc vào **details**. Details nên phụ thuộc vào abstractions

### Lợi ích

✅ Loose Coupling (Giảm sự phụ thuộc giữa các classes)  
✅ Flexibility (Dễ dàng thay đổi implementations)  
✅ Testability (Dễ unit testing với mock dependencies)  
✅ Maintainability (Code dễ bảo trì hơn)

### Ví dụ Code

```csharp
// ServiceContracts (Interface)
namespace ServiceContracts
{
    public interface ICitiesService
    {
        List<string> GetCities();
    }
}

// Services (Implementation)
namespace Services
{
    public class CitiesService : ICitiesService
    {
        private List<string> _cities;

        public CitiesService() 
        {
            _cities = new List<string>() { "London", "Paris", "New York", "Tokyo", "Rome" };
        }
 
        public List<string> GetCities()
        {
            return _cities;
        }
    }
}
```

---

## 3. Inversion of Control (IoC)

**IoC** là nguyên tắc chuyển giao quyền kiểm soát việc tạo và quản lý objects từ code của bạn sang một framework hoặc container.

---

## 4. Dependency Injection (DI)

**DI** là cách triển khai cụ thể của IoC. Dependencies được cung cấp cho class từ bên ngoài, thường qua constructor.

### Đăng ký Service

```csharp
// Program.cs
builder.Services.Add(new ServiceDescriptor(
    typeof(ICitiesService),      // Interface
    typeof(CitiesService),       // Implementation
    ServiceLifetime.Transient    // Lifetime
));
```

### Constructor Injection

```csharp
public class HomeController : Controller
{
    private readonly ICitiesService _citiesService;

    public HomeController(ICitiesService citiesService) 
    {
        _citiesService = citiesService; 
    }
 
    [Route("/")]
    public IActionResult Index()
    {
        List<string> cities = _citiesService.GetCities();
        return View(cities);
    }
}
```

---

## 5. Service Lifetimes (Vòng đời Service)

Khi đăng ký service, bạn cần chọn lifetime phù hợp:

### **Transient**
- ✨ Tạo instance mới **mỗi lần** request
- 📦 Phù hợp với: Lightweight, stateless services
- 🔧 Ví dụ: Helper classes, Logger

```csharp
builder.Services.AddTransient<ITransientService, TransientService>();
```

### **Scoped** ⭐ (Khuyên dùng cho Web Apps)
- ✨ Tạo **một instance** cho mỗi HTTP request
- 📦 Instance được shared trong suốt request
- 🔧 Ví dụ: Database context, User-specific data

```csharp
builder.Services.AddScoped<IScopedService, ScopedService>();
```

### **Singleton**
- ✨ Tạo **một instance duy nhất** cho cả application
- 📦 Instance được shared cho tất cả requests
- 🔧 Ví dụ: Configuration settings, Shared caches

```csharp
builder.Services.AddSingleton<ISingletonService, SingletonService>();
```

### Lưu ý quan trọng

⚠️ **Tránh Captive Dependencies**: Không inject service có lifetime ngắn hơn (Transient) vào service có lifetime dài hơn (Singleton)

---

## 6. Các kỹ thuật Dependency Injection

### **Constructor Injection** ⭐ (Khuyên dùng)

```csharp
public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
}
```

**Ưu điểm**: Dễ hiểu, dễ test, đảm bảo dependencies luôn có sẵn

### **Property Injection**

```csharp
public class MyMiddleware
{
    [FromServices]
    public ILogger<MyMiddleware> Logger { get; set; }
}
```

**Khi nào dùng**: Optional dependencies hoặc tránh constructor quá nhiều parameters

### **Method Injection**

```csharp
public IActionResult Index([FromServices] IUserService userService)
{
    // Sử dụng userService trong method này
}
```

**Khi nào dùng**: Chỉ cần service trong một action method cụ thể

---

## 7. Best Practices (Thực hành tốt)

### ✅ Ưu tiên Constructor Injection
Đảm bảo class có đủ dependencies trước khi sử dụng.

### ✅ Sử dụng Interfaces
Tạo loose coupling, dễ test với mock objects.

```csharp
public interface IProductRepository { /* ... */ }
public class ProductRepository : IProductRepository { /* ... */ }
```

### ✅ Đăng ký Dependencies ở Composition Root
Tập trung cấu hình DI trong `Program.cs`.

### ✅ Chọn đúng Service Lifetime
- **Transient**: Stateless services
- **Scoped**: Per-request services (Web apps)
- **Singleton**: Application-wide services

### ❌ Tránh Service Locator Anti-Pattern
Không gọi `IServiceProvider.GetService()` trực tiếp trong classes.

### ❌ Tránh Captive Dependencies
Không inject Transient vào Singleton.

---

## 8. Autofac (IoC Container nâng cao)

**Autofac** là một IoC container mạnh mẽ, cung cấp nhiều tính năng nâng cao hơn DI container có sẵn.

### Cài đặt

```bash
Install-Package Autofac.Extensions.DependencyInjection
```

### Cấu hình

```csharp
// Program.cs
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<CitiesService>()
        .As<ICitiesService>()
        .InstancePerLifetimeScope(); // Scoped
});
```

### Lifetime Scopes trong Autofac

- `InstancePerDependency()` → Transient
- `InstancePerLifetimeScope()` → Scoped
- `SingleInstance()` → Singleton

---

## 9. Tổng kết

### Các khái niệm chính

| Khái niệm | Giải thích |
|-----------|------------|
| **DIP** | Phụ thuộc vào abstractions (interfaces), không phụ thuộc vào implementations |
| **IoC** | Framework quản lý việc tạo objects thay vì code của bạn |
| **DI** | Inject dependencies từ bên ngoài vào class |
| **Service Lifetimes** | Transient, Scoped, Singleton |

### Khi nào dùng gì?

- **Constructor Injection**: Mandatory dependencies (khuyên dùng)
- **Property Injection**: Optional dependencies
- **Method Injection**: Dependencies chỉ dùng trong một method
- **Scoped Lifetime**: Khuyên dùng cho web applications
- **Autofac**: Khi cần tính năng nâng cao

### Tips phỏng vấn

💡 **Hiểu khái niệm**: Giải thích rõ DIP, IoC, và DI  
💡 **Chọn đúng Lifetime**: Biết khi nào dùng Transient/Scoped/Singleton  
💡 **Best Practices**: Dùng interfaces và constructor injection  
💡 **Troubleshooting**: Xử lý circular dependencies, incorrect lifetimes

---

> **Ghi nhớ**: Dependency Injection giúp code **dễ bảo trì**, **dễ test**, và **linh hoạt** hơn!

