🛒 HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI (MINISUPERMARKET SYSTEM)

Môn học: Lập trình Ứng dụng .NET Core (Mã môn: 229162) Buổi thực hành: Buổi 2 - Bảo mật Web API bằng JWT (JSON Web Token), phân quyền theo vai trò (Role-based Authorization) và tích hợp màn hình Đăng nhập cho WinForms Client

🏗️ 1. Mô hình Kiến trúc Hệ thống (Client - Server)

Dự án tiếp tục phát triển từ Buổi 1, giữ nguyên mô hình phân tầng, tách biệt hoàn toàn giữa Backend và Frontend, đồng thời bổ sung lớp bảo mật:

MiniSupermarket.API (Backend): Dự án ASP.NET Core Web API chịu trách nhiệm xử lý nghiệp vụ, quản lý dữ liệu, xác thực người dùng và cấp phát JWT Token. Các endpoint được bảo vệ bằng [Authorize], có phân quyền theo vai trò Admin / Cashier.
MiniSupermarket.WinForms (Frontend Client): Ứng dụng Windows Forms đóng vai trò máy trạm POS tại quầy. Người dùng phải đăng nhập trước; token nhận được sẽ lưu trong SessionManager và được đính kèm vào header Authorization: Bearer <token> ở mọi request gọi API.
🔐 Luồng xác thực (Authentication Flow)
text
WinForms (FormLogin)                      Web API
       │                                     │
       │  1. POST /api/auth/login            │
       │     { username, password }          │
       │ ──────────────────────────────────► │
       │                                     │  2. Kiểm tra tài khoản
       │                                     │     → tạo JWT (Claims: Name, Role)
       │  3. { token, role }                 │
       │ ◄────────────────────────────────── │
       │                                     │
       │  4. Lưu vào SessionManager          │
       │                                     │
       │  5. GET/POST/DELETE /api/categories │
       │     Header: Bearer <token>          │
       │ ──────────────────────────────────► │
       │                                     │  6. Xác thực token + kiểm tra Role
       │  7. 200 OK / 401 / 403              │
       │ ◄────────────────────────────────── │
🛠️ 2. Công nghệ Sử dụng
Ngôn ngữ: C# (.NET 8.0)
Backend: ASP.NET Core Web API, Controllers, In-Memory Data, LINQ
Bảo mật: JWT Bearer Authentication, Role-based Authorization
Microsoft.AspNetCore.Authentication.JwtBearer
System.IdentityModel.Tokens.Jwt
Frontend: Windows Forms (.NET 8.0), System.Net.Http.Json, HttpClient + AuthenticationHeaderValue
Công cụ kiểm thử: Swagger UI (có nút Authorize)
📂 3. Cấu trúc Solution
text
MiniSupermarket/
│
├── MiniSupermarket.sln
│
├── MiniSupermarket.API/                  # Dự án Web API (Backend)
│   ├── Controllers/
│   │   ├── AuthController.cs             # Đăng nhập, sinh JWT Token   [MỚI - Buổi 2]
│   │   └── CategoriesController.cs       # CRUD danh mục + [Authorize] [CẬP NHẬT]
│   ├── Properties/
│   │   └── launchSettings.json           # Cổng chạy: https://localhost:7123
│   ├── appsettings.json                  # Chứa JwtSettings:Secret
│   └── Program.cs                        # Cấu hình JWT, Swagger, Middleware [CẬP NHẬT]
│
└── MiniSupermarket.WinForms/             # Dự án Windows Forms (Frontend Client)
    ├── FormLogin.cs                      # Màn hình đăng nhập             [MỚI - Buổi 2]
    ├── FormCategoryManagement.cs         # Quản lý danh mục (gửi kèm Token) [CẬP NHẬT]
    ├── SessionManager.cs                 # Lưu JwtToken, CurrentRole      [MỚI - Buổi 2]
    ├── CategoryDto.cs                    # Lớp dữ liệu danh mục
    └── Program.cs                        # Khởi chạy FormLogin đầu tiên   [CẬP NHẬT]
👥 4. Tài khoản và Phân quyền
Tài khoản	Mật khẩu	Vai trò	Quyền hạn
admin	123456	Admin	Toàn quyền: xem, thêm, sửa, xóa danh mục, truy cập trang quản trị
cashier	123456	Cashier	Xem, thêm, sửa danh mục, dùng màn hình POS. Không được xóa
Bảng endpoint
Method	Endpoint	Yêu cầu	Ghi chú
POST	/api/auth/login	Không cần token	Trả về token và role
GET	/api/categories	Đã đăng nhập	Lấy danh sách danh mục
GET	/api/categories/{id}	Đã đăng nhập	Lấy 1 danh mục
POST	/api/categories	Đã đăng nhập	Thêm mới
PUT	/api/categories/{id}	Đã đăng nhập	Cập nhật
DELETE	/api/categories/{id}	Admin	Xóa danh mục
GET	/api/categories/admin-dashboard	Admin	Endpoint test phân quyền
GET	/api/categories/staff-pos	Admin, Cashier	Endpoint test phân quyền
🚀 5. Hướng dẫn Chạy và Kiểm thử Dự án
Bước 1: Chạy phía Backend (Web API)
Mở MiniSupermarket.sln bằng Visual Studio 2022 (đợi NuGet khôi phục gói xong).
Nhấp chuột phải vào project MiniSupermarket.API → chọn Set as Startup Project.
Nhấn F5 để chạy. Trình duyệt tự động mở Swagger UI tại https://localhost:7123/swagger.
Bước 2: Kiểm thử bảo mật trên Swagger
Gọi GET /api/categories khi chưa đăng nhập → kết quả 401 Unauthorized.
Gọi POST /api/auth/login với body:
json
   { "username": "cashier", "password": "123456" }

→ sao chép chuỗi token trong kết quả. 3. Bấm nút Authorize (góc trên bên phải), nhập Bearer <token> rồi xác nhận. 4. Thử phân quyền bằng tài khoản cashier:

GET /api/categories/staff-pos → 200 OK
GET /api/categories/admin-dashboard → 403 Forbidden
DELETE /api/categories/1 → 403 Forbidden
Đăng nhập lại bằng admin, lặp lại các bước trên → cả admin-dashboard và DELETE đều trả về thành công.
Bước 3: Chạy phía Frontend (WinForms Client)
Đảm bảo cổng trong SessionManager.ApiBaseUrl khớp với cổng API đang chạy (mặc định https://localhost:7123/api/).
Nhấp chuột phải vào project MiniSupermarket.WinForms → Debug → Start New Instance.
Đăng nhập tại FormLogin bằng admin / 123456 hoặc cashier / 123456.
Tại màn hình quản lý danh mục, thử nghiệm: Tải lại, Thêm, Xóa.
Đăng nhập cashier rồi bấm Xóa → hiện thông báo 403 Forbidden: Chỉ Admin mới có quyền xóa.
Đăng nhập admin → xóa thành công.
⚠️ 6. Lưu ý khi Chạy
Phải chạy API trước, sau đó mới chạy WinForms.
Nếu API chạy cổng khác 7123, cập nhật lại ApiBaseUrl trong SessionManager.cs.
Nếu gặp lỗi chứng chỉ SSL, chạy lệnh: dotnet dev-certs https --trust
Thứ tự middleware trong Program.cs bắt buộc là UseAuthentication() trước UseAuthorization().
Khóa bí mật JWT (JwtSettings:Secret) phải dài tối thiểu 32 ký tự.
👨‍💻 7. Tác giả
Họ tên sinh viên: Trần Tấn Đạt
Mã sinh viên: 2124110127
Lớp học phần: CCQ2411D
