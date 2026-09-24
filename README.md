🛒 MINISUPERMARKET SYSTEM
Hệ thống Quản lý Siêu thị Mini

Môn học: Lập trình Ứng dụng .NET Core (Mã môn: 229162) Buổi thực hành: Buổi 2 - Bảo mật Web API bằng JWT, phân quyền theo vai trò và màn hình Đăng nhập cho WinForms

</div>
📑 Mục lục
Giới thiệu
Tính năng
Kiến trúc hệ thống
Công nghệ sử dụng
Cấu trúc Solution
Tài khoản và phân quyền
Hướng dẫn cài đặt và chạy
Kiểm thử
Xử lý lỗi thường gặp
Hướng phát triển
Tác giả
📖 1. Giới thiệu

MiniSupermarket là hệ thống quản lý siêu thị mini theo mô hình Client - Server, được xây dựng dần qua từng buổi thực hành:

Buổi	Nội dung	Trạng thái
1	Web API quản lý danh mục (CRUD) + WinForms Client	✅ Hoàn thành
2	Bảo mật JWT, phân quyền Admin/Cashier, màn hình Đăng nhập	✅ Buổi hiện tại
3	Kết nối cơ sở dữ liệu SQL Server bằng Entity Framework Core	⏳ Dự kiến
✨ 2. Tính năng

Backend (Web API)

🔑 Đăng nhập và cấp phát JWT Token (hạn sử dụng 2 giờ)
🛡️ Bảo vệ toàn bộ API danh mục bằng [Authorize]
👮 Phân quyền theo vai trò (Admin, Cashier) bằng [Authorize(Roles = "...")]
📚 CRUD danh mục: xem, thêm, sửa, xóa
🧪 Swagger UI tích hợp nút Authorize để dán token và kiểm thử

Frontend (WinForms)

🔐 Màn hình Đăng nhập (ẩn mật khẩu, phím Enter để đăng nhập)
💾 Lưu phiên làm việc trong SessionManager (Token + Vai trò)
📋 Hiển thị danh mục trên DataGridView, hiển thị vai trò đang đăng nhập
➕ Thêm danh mục / 🗑️ Xóa danh mục / 🔄 Tải lại
⚠️ Thông báo thân thiện khi bị từ chối quyền (403) hoặc hết phiên (401)
🏗️ 3. Kiến trúc hệ thống

Dự án tách biệt hoàn toàn giữa Backend và Frontend:

MiniSupermarket.API (Backend): ASP.NET Core Web API xử lý nghiệp vụ, quản lý dữ liệu, xác thực người dùng và cấp phát JWT.
MiniSupermarket.WinForms (Frontend Client): Ứng dụng Windows Forms đóng vai trò máy trạm POS, dùng HttpClient gọi API và đính kèm token vào mọi request.
🔐 Luồng xác thực (Authentication Flow)
text
WinForms (FormLogin)                       Web API
       │                                      │
       │ 1. POST /api/auth/login              │
       │    {username, password}              │
       │ ───────────────────────────────────► │
       │                                      │ 2. Kiểm tra tài khoản, tạo JWT
       │ 3. { token, role }                   │
       │ ◄─────────────────────────────────── │
       │ 4. Lưu vào SessionManager            │
       │                                      │
       │ 5. GET/POST/DELETE /api/categories   │
       │    Header: Bearer {token}            │
       │ ───────────────────────────────────► │
       │                                      │ 6. Xác thực token + kiểm tra Role
       │ 7. 200 OK / 401 / 403                │
       │ ◄─────────────────────────────────── │
🧾 Cấu trúc JWT Token

Token gồm 3 phần ngăn cách bởi dấu chấm: Header.Payload.Signature

Phần	Nội dung
Header	Thuật toán ký: HS256
Payload	Claims: name (tên đăng nhập), role (vai trò), exp (thời điểm hết hạn)
Signature	Chữ ký tạo từ khóa bí mật JwtSettings:Secret, dùng để chống giả mạo

Bạn có thể dán token vào jwt.io để xem nội dung giải mã.

🛠️ 4. Công nghệ sử dụng
Lớp	Công nghệ
Ngôn ngữ	C# (.NET 8.0)
Backend	ASP.NET Core Web API, Controllers, In-Memory Data, LINQ
Bảo mật	Microsoft.AspNetCore.Authentication.JwtBearer, System.IdentityModel.Tokens.Jwt
Frontend	Windows Forms (.NET 8.0), System.Net.Http.Json
Kiểm thử	Swagger UI (Swashbuckle)
IDE	Visual Studio 2022
📂 5. Cấu trúc Solution
text
MiniSupermarket/
│
├── MiniSupermarket.sln
├── README.md
│
├── MiniSupermarket.API/                  # Web API (Backend)
│   ├── Controllers/
│   │   ├── AuthController.cs             # Đăng nhập, sinh JWT           🆕 Buổi 2
│   │   └── CategoriesController.cs       # CRUD + [Authorize]            ✏️ Cập nhật
│   ├── Properties/
│   │   └── launchSettings.json           # Cổng chạy: https://localhost:7123
│   ├── appsettings.json                  # Chứa JwtSettings:Secret
│   └── Program.cs                        # Cấu hình JWT, Swagger         ✏️ Cập nhật
│
└── MiniSupermarket.WinForms/             # Windows Forms (Frontend Client)
    ├── FormLogin.cs                      # Màn hình đăng nhập            🆕 Buổi 2
    ├── FormCategoryManagement.cs         # Quản lý danh mục + Token      ✏️ Cập nhật
    ├── SessionManager.cs                 # Lưu JwtToken, CurrentRole     🆕 Buổi 2
    ├── CategoryDto.cs                    # Lớp dữ liệu danh mục
    └── Program.cs                        # Chạy FormLogin đầu tiên       ✏️ Cập nhật
👥 6. Tài khoản và phân quyền
Tài khoản mẫu
Tài khoản	Mật khẩu	Vai trò	Quyền hạn
admin	123456	Admin	Toàn quyền: xem, thêm, sửa, xóa, truy cập trang quản trị
cashier	123456	Cashier	Xem, thêm, sửa, dùng màn hình POS. Không được xóa
Bảng endpoint
Method	Endpoint	Quyền yêu cầu	Mô tả
POST	/api/auth/login	Công khai	Đăng nhập, trả về token và role
GET	/api/categories	Đã đăng nhập	Lấy danh sách danh mục
GET	/api/categories/{id}	Đã đăng nhập	Lấy một danh mục
POST	/api/categories	Đã đăng nhập	Thêm danh mục
PUT	/api/categories/{id}	Đã đăng nhập	Cập nhật danh mục
DELETE	/api/categories/{id}	🔒 Admin	Xóa danh mục
GET	/api/categories/admin-dashboard	🔒 Admin	Test phân quyền Admin
GET	/api/categories/staff-pos	🔒 Admin, Cashier	Test phân quyền nhân viên
Ý nghĩa mã trạng thái HTTP
Mã	Ý nghĩa	Khi nào xảy ra
200 OK	Thành công	Có token hợp lệ và đủ quyền
401 Unauthorized	Chưa xác thực	Không gửi token, token sai hoặc hết hạn
403 Forbidden	Không đủ quyền	Đã đăng nhập nhưng vai trò không được phép (ví dụ Cashier xóa danh mục)
🚀 7. Hướng dẫn cài đặt và chạy
Yêu cầu
Visual Studio 2022 với 2 workload: ASP.NET and web development và .NET desktop development
.NET 8 SDK
Kết nối Internet lần đầu để tải gói NuGet
Bước 1: Chạy Backend (Web API)
Mở MiniSupermarket.sln bằng Visual Studio 2022 và đợi NuGet khôi phục gói.
Chuột phải project MiniSupermarket.API → Set as Startup Project.
Nhấn F5. Trình duyệt tự mở Swagger UI tại https://localhost:7123/swagger.
Bước 2: Chạy Frontend (WinForms)
Kiểm tra SessionManager.ApiBaseUrl khớp với cổng API (mặc định https://localhost:7123/api/).
Chuột phải project MiniSupermarket.WinForms → Debug → Start New Instance.
Đăng nhập bằng admin / 123456 hoặc cashier / 123456.

💡 Mẹo: Muốn chạy cả hai cùng lúc, chuột phải Solution → Configure Startup Projects → Multiple startup projects, đặt cả hai là Start (API để trên WinForms).

🧪 8. Kiểm thử
8.1. Kiểm thử trên Swagger
#	Thao tác	Kết quả mong đợi
1	GET /api/categories khi chưa đăng nhập	❌ 401 Unauthorized
2	POST /api/auth/login với cashier / 123456, sao chép token	✅ 200 OK kèm token
3	Bấm Authorize, nhập Bearer <token>	Swagger gắn token cho các request sau
4	GET /api/categories/staff-pos	✅ 200 OK
5	GET /api/categories/admin-dashboard	❌ 403 Forbidden
6	DELETE /api/categories/1 bằng cashier	❌ 403 Forbidden
7	Đăng nhập lại bằng admin, lặp lại bước 5 và 6	✅ Thành công

Body đăng nhập mẫu:

json
{ "username": "cashier", "password": "123456" }
8.2. Kiểm thử trên WinForms
#	Thao tác	Kết quả mong đợi
1	Đăng nhập sai mật khẩu	Thông báo "Sai tài khoản hoặc mật khẩu"
2	Đăng nhập cashier / 123456	Mở form danh mục, hiện "Vai trò hiện tại: Cashier"
3	Bấm Thêm với tên hợp lệ	Danh mục mới xuất hiện trên bảng
4	Chọn một dòng, bấm Xóa	Thông báo 403: chỉ Admin được xóa
5	Đăng nhập admin / 123456, xóa một dòng	Xóa thành công
🛠️ 9. Xử lý lỗi thường gặp
Hiện tượng	Nguyên nhân	Cách xử lý
WinForms báo "Lỗi kết nối đến Server"	API chưa chạy hoặc sai cổng	Chạy API trước; kiểm tra ApiBaseUrl
Lỗi chứng chỉ SSL	Chưa tin cậy chứng chỉ dev	Chạy dotnet dev-certs https --trust
Luôn bị 401 dù đã đăng nhập	Thiếu chữ Bearer  hoặc sai thứ tự middleware	Nhập Bearer <token>; đảm bảo UseAuthentication() đứng trước UseAuthorization()
Swagger không có nút Authorize	Chưa cấu hình AddSecurityDefinition	Dùng Program.cs của dự án này
Lỗi cài NuGet	Sai phiên bản hoặc cache lỗi	Chọn gói bản 8.x cho .NET 8; chạy dotnet nuget locals all --clear
Lỗi khóa JWT quá ngắn	Secret dưới 32 ký tự	Đặt khóa dài từ 32 ký tự trở lên trong appsettings.json
🔭 10. Hướng phát triển
 Lưu người dùng và mật khẩu vào cơ sở dữ liệu, băm mật khẩu (BCrypt) thay vì so sánh trực tiếp
 Chuyển dữ liệu từ In-Memory sang SQL Server + Entity Framework Core (Buổi 3)
 Thêm chức năng Sửa và Tìm kiếm danh mục trên giao diện WinForms
 Ẩn/hiện nút theo vai trò (ví dụ ẩn nút Xóa với Cashier)
 Refresh Token và đăng xuất
 Mở rộng module Sản phẩm, Hóa đơn, Khách hàng

⚠️ Lưu ý bảo mật: Tài khoản cứng và khóa JWT trong appsettings.json chỉ phục vụ học tập. Khi triển khai thực tế phải dùng cơ sở dữ liệu, biến môi trường hoặc Secret Manager.

👨‍💻 11. Tác giả
	
Họ tên sinh viên	Trần Tấn Đạt
Mã sinh viên	2124110127
Lớp học phần	CCQ2411D
