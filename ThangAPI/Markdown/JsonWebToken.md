# JSON Web Token (JWT) là gì?

JSON Web Token (JWT) là một chuẩn mở (RFC 7519) định nghĩa một cách nhỏ gọn và khép kín để truyền an toàn thông tin giữa các bên dưới dạng đối tượng JSON.

Thông tin trong JWT có thể được **xác minh** và **đáng tin cậy** vì nó có chứa chữ ký số.

JWT có thể được ký bằng:
- Thuật toán bí mật HMAC (HS256)
- Public/private key dùng RSA

JWT gồm 3 phần (ngăn cách bởi dấu `.`):
1. **Header** (JSON, Base64)
2. **Payload** (JSON, Base64)
3. **Signature** (Base64)

---

## Khi nào nên dùng JSON Web Token?

### 1. Authentication
Khi user đăng nhập thành công, server trả về JWT. Token được lưu ở:
- LocalStorage
- Hoặc Cookies

Mỗi request tiếp theo gửi token trong header dạng:
```
Authorization: Bearer <token>
```

### 2. Information Exchange
JWT là cách bảo mật để trao đổi thông tin giữa nhiều ứng dụng.

---

## Quy trình hoạt động của JWT

1. User gửi username + password lên server.
2. Server xác thực thành công → tạo JWT gửi về client.
3. Client lưu JWT.
4. Khi truy cập các route cần bảo vệ → client gửi token vào Authorization Header.

---

## Các thành phần của JWT

### 1. Header
Gồm:
- Type token (JWT)
- Thuật toán (HS256 hoặc RSA)

Ví dụ:
```json
{
  "alg": "HS256",
  "typ": "JWT"
}
```

### 2. Payload
Payload chứa claims – thông tin về user + metadata.

**3 loại claims:**
- Reserved claims (chuẩn bắt buộc hoặc nên có):
  - `iss` — issuer
  - `iat` — issued at
  - `exp` — expiration
  - `sub` — subject
  - `aud` — audience
  - `jti` — unique id (chống replay token)
- Public claims: Do cộng đồng định nghĩa.
- Private claims: Tự định nghĩa, dùng nội bộ giữa 2 hệ thống.

### 3. Signature
Được tạo từ:
- Header
- Payload
- Secret key

---

## JWT trong ASP.NET Core API

### Bước 1 — Cài đặt thư viện
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.AspNetCore.Authentication.JwtBearer

### Bước 2 — Cấu hình `appsettings.json`
```json
"JWT": {
  "ValidAudience": "http://localhost:4200",
  "ValidIssuer": "http://localhost:5000",
  "Secret": "JWTRefreshTokenHIGHsecuredPasswordVVVp1OH7Xzyr",
  "TokenValidityInMinutes": 1,
  "RefreshTokenValidityInDays": 7
}
```

### Bước 3 — Thêm Authentication vào `Program.cs`
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

app.UseAuthentication();
app.UseAuthorization();
```

### Bước 4 — Tạo Token và trả về Client
**Claims:**
```csharp
var authClaims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.UserName),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
};
```

**Hàm tạo Token:**
```csharp
private JwtSecurityToken CreateToken(List<Claim> authClaims)
{
    var authSigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

    _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

    var token = new JwtSecurityToken(
        issuer: _configuration["JWT:ValidIssuer"],
        audience: _configuration["JWT:ValidAudience"],
        expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
        claims: authClaims,
        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
    );

    return token;
}
```

**Viết token:**
```csharp
token = new JwtSecurityTokenHandler().WriteToken(newToken);
```

### Bước 5 — Kiểm tra token bằng `TypeFilterAttribute`
**AuthorizeAttribute tự định nghĩa:**
```csharp
public class AuthorizeAttribute : TypeFilterAttribute
{
    public AuthorizeAttribute() : base(typeof(DemoAuthorizeActionFilter))
    {
    }
}
```

**Action Filter:**
```csharp
public class DemoAuthorizeActionFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var identity = context.HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userClaims = identity.Claims;
            var list1 = new UserModel
            {
                Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                ID = Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value)
            };
        }
    }
}
```

---

> **Lưu ý:**
> - Để hiển thị ảnh trong markdown trên GitHub, bạn nên đặt ảnh trong thư mục dự án (ví dụ: `Images/`) và sử dụng đường dẫn tương đối như ví dụ ở trên.
> - Nếu ảnh nằm ngoài dự án (ví dụ: ổ D hoặc thư mục Picture), GitHub sẽ không hiển thị được ảnh đó.
> - Hãy copy ảnh vào thư mục dự án để đảm bảo hiển thị trên GitHub.
