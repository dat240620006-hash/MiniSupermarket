using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiniSupermarket.API.Controllers
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bắt buộc có Token hợp lệ mới gọi được
    public class CategoriesController : ControllerBase
    {
        // Dữ liệu in-memory (giả lập Buổi 1). Buổi 3 sẽ thay bằng SQL Server.
        private static readonly List<CategoryDto> _categories = new()
        {
            new CategoryDto { Id = 1, Name = "Đồ uống",     Description = "Nước ngọt, nước suối, trà" },
            new CategoryDto { Id = 2, Name = "Bánh kẹo",    Description = "Bánh snack, kẹo, socola" },
            new CategoryDto { Id = 3, Name = "Gia vị",      Description = "Muối, đường, nước mắm" }
        };
        private static int _nextId = 4;

        // GET /api/categories  (Admin + Cashier)
        [HttpGet]
        public IActionResult GetAll() => Ok(_categories);

        // GET /api/categories/1
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _categories.FirstOrDefault(c => c.Id == id);
            return item == null ? NotFound() : Ok(item);
        }

        // POST /api/categories
        [HttpPost]
        public IActionResult Create([FromBody] CategoryDto dto)
        {
            dto.Id = _nextId++;
            _categories.Add(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // PUT /api/categories/1
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] CategoryDto dto)
        {
            var item = _categories.FirstOrDefault(c => c.Id == id);
            if (item == null) return NotFound();
            item.Name = dto.Name;
            item.Description = dto.Description;
            return NoContent();
        }

        // DELETE /api/categories/1  -> CHỈ ADMIN
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _categories.FirstOrDefault(c => c.Id == id);
            if (item == null) return NotFound();
            _categories.Remove(item);
            return NoContent();
        }

        // ===== Endpoint kiểm tra phân quyền (Phần 3 của bài) =====

        // Chỉ Admin
        [HttpGet("admin-dashboard")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminDashboard()
        {
            return Ok(new { message = "Chào mừng Admin! Bạn có toàn quyền quản trị hệ thống siêu thị mini." });
        }

        // Admin + Cashier
        [HttpGet("staff-pos")]
        [Authorize(Roles = "Admin,Cashier")]
        public IActionResult GetStaffPos()
        {
            return Ok(new { message = "Màn hình POS Thu ngân sẵn sàng phục vụ bán hàng." });
        }
    }
}
