# Các gói NuGet làm việc với Authentication

## Microsoft.AspNetCore.Authentication.JwtBearer
- Tích hợp xác thực dựa trên JWT trong ASP.NET Core
- Cho phép ứng dụng nhận và xác thực token JWT gửi từ client
- Hỗ trợ thiết lập tùy chỉnh như:
    - Issuer (người phát hành)
    - Audience (khán giả)
    - Khóa bảo mật
    - Thời gian hết hạn
    - ...v.v

**➜ Khi nào sử dụng:** Khi bạn muốn bảo vệ API hoặc các endpoint trong ASP.NET Core bằng JWT.

---

## Microsoft.IdentityModel.Tokens
- Thư viện cốt lõi để làm việc với các token bảo mật như JWT
- Cung cấp các công cụ để tạo, xác thực, mã hóa, giải mã token
- Hỗ trợ các loại thuật toán mã hóa và chữ ký
- Quản lý các khóa (keys) dùng để ký hoặc xác thực token
- Giúp bạn xác thực tính hợp lệ của token dựa trên các tiêu chí như chữ ký, thời gian, issuer, audience...

**➜ Khi nào sử dụng:** Khi bạn cần xử lý chi tiết về token (không nhất thiết phải trong ASP.NET Core, có thể dùng trong các ứng dụng .NET khác).

---

## System.IdentityModel.Tokens.Jwt
- Thư viện để tạo và xử lý JWT một cách trực tiếp
- Giúp phân tích (parse), tạo, ký và xác thực JWT
- Cho phép bạn đọc payload, header của token
- Tạo token mới với các claim, thời gian hết hạn...
- Kết hợp với Microsoft.IdentityModel.Tokens để kiểm tra tính hợp lệ

**➜ Khi nào sử dụng:** Khi bạn muốn thao tác chi tiết với JWT trong ứng dụng .NET.

---

## Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Phần mở rộng của ASP.NET Core Identity
- Tích hợp với Entity Framework Core để lưu trữ và quản lý người dùng (user), vai trò (role) trong cơ sở dữ liệu
- Cung cấp các model mặc định như User, Role, UserClaims... để quản lý xác thực và phân quyền
- Tích hợp với database thông qua EF Core để dễ dàng lưu trữ thông tin người dùng

**➜ Khi nào sử dụng:** Khi bạn cần triển khai hệ thống đăng nhập, đăng ký, quản lý user và role trong ứng dụng ASP.NET Core sử dụng EF Core.
