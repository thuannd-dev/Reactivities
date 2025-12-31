 <img src="https://drive.usercontent.google.com/download?id=1GZk0-Ur7bn0NLGEs-hUJvQhnY5fkepbX"/>
 <img src="https://drive.usercontent.google.com/download?id=1Wd8gj3ooQG5nxesPWHqxD7SqLe-ueLV3"/>

### Dependency Inversion: High-level dependent on Abstraction, not Implementation

1️. Application layer là nơi chứa toàn bộ business logic (use cases)

Đây là trái tim của ứng dụng.

Nó không được biết gì về:

Web API

Authentication

Token / Claims

HttpContext

Framework

Database cụ thể

➡ Application chỉ biết logic nghiệp vụ.

2️. Nhưng trong logic, đôi khi cần biết username của user hiện tại đang thực hiện hành động.

Ví dụ: User tham gia Activity
→ cần lấy username để thêm vào danh sách attendees.

Nhưng Application layer không thể truy cập token hoặc HttpContext.

3️. Giải pháp: Dependency Inversion

Bước 1 — Đặt interface trong Application layer

(Vì Application chỉ được phụ thuộc vào trừu tượng, không phụ thuộc framework)

```
public interface IUserAccessor
{
string GetUsername();
}
```

Application sẽ gọi IUserAccessor.GetUsername(), nhưng không biết nó được implement thế nào.

Bước 2 — Viết implementation trong Infrastructure layer

Infrastructure layer được phép dùng:

HttpContext

ClaimsPrincipal

JWT token

Ví dụ:

```
public class UserAccessor : IUserAccessor
{
private readonly IHttpContextAccessor _context;

    public string GetUsername()
    {
        return _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

}
```

➡ Đây là nơi giải mã token, đọc claim, lấy username.

4️. API Layer sẽ đăng ký DI của implementation này

API project có Startup/Program.cs → DI Container ở đây.

Nó tham chiếu Infrastructure để đăng ký:

```
services.AddScoped<IUserAccessor, UserAccessor>();
```

→ Application layer chỉ sử dụng interface, nhưng DI sẽ nhét đúng implementation vào.

5️. Luật phụ thuộc: đi từ ngoài → vào trong

Infrastructure → Application (để implement interface)

API → Infrastructure (để gọi các service)

Application không phụ thuộc Infrastructure
Application không phụ thuộc API

➡ Điều này đảm bảo Application layer không bao giờ bị thay đổi khi thay đổi framework hoặc cách authentication.

6️. Tại sao làm vậy?

Vì nếu một ngày:

Bạn đổi JWT sang Cookie

Đổi từ Web API sang Console app

Không còn HttpContext

Không còn token

Bạn chỉ đổi implementation của IUserAccessor trong Infrastructure.

Application layer không thay đổi 1 dòng nào.

Tóm lại

Application layer cần username, nhưng không được biết authentication hoạt động thế nào.

Application tạo một interface IUserAccessor.

Infrastructure implement nó bằng cách đọc token trong HttpContext, còn API đăng ký DI.
