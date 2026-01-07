 <img src="https://drive.usercontent.google.com/download?id=1GZk0-Ur7bn0NLGEs-hUJvQhnY5fkepbX"/>

 <img src="https://drive.usercontent.google.com/download?id=1Wd8gj3ooQG5nxesPWHqxD7SqLe-ueLV3"/>

###### I'll be using our infrastructure project for anything that's kind of outside of our application logic. So our application layer needs to know nothing about authentication to be able to get a user's username. I've decoupled everything in our business logic layer, the application layer from any outside concerns such as how does the user authenticate and our application layer doesn't need to worry about any of that. So i'll use that infrastructure project for other things as we come across them that are not directly related to our business logic.

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

→ Application chỉ biết logic nghiệp vụ.

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

→ Đây là nơi giải mã token, đọc claim, lấy username.

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

→ Điều này đảm bảo Application layer không bao giờ bị thay đổi khi thay đổi framework hoặc cách authentication.

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

# Why you shoiuld left column UserId is nullable in Photo table?

- It means when i delete a User, the related Photo will not be deleted automatically. Because UserId is nullable, so EF Core will just set UserId to null in Photo table.
- Because if we did delete the images from our database automatically when we deleted a user, then why might that not be a good idea? Well, if we think about where we're storing our images, then they're being stored in cloudinary. And if we delete a user is they're going to be any process in our application to then automatically go and delete those images from Cloudinary.
- I will delete the photo by cron job or background service later.
- It help increase performance of application. Because if we make delete cascade, when we delete a User, it will delete all related Photos in a single transaction. It may take long time and lock the database tables. And if a error occurs, the whole transaction will be rolled back.
- You will able to implement feature rollback the delete User operation if needed.
- Clean batch is make less requests to database and cloudinary service when delete User.
- you will using hangfire to implement background job to delete the photos in cloudinary and related records in database.
- The last one is i'm not going to let users delete their user accounts anyway, and we're not going to have thousands of orphaned photos in Cloudinary so i don't need to worry about that scenario. And i going to implement cascade delete in this case =)))) It just appropriate  for this application.
