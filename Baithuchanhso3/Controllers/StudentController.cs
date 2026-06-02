using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaiThucHanhSo3.Data;
using BaiThucHanhSo3.Models;
using BaiThucHanhSo3.Models.ViewModels;

namespace BaiThucHanhSo3.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentController(ApplicationDbContext context) => _context = context;

        // ── Trang chính (load 1 lần duy nhất) ──────────────────────────────
        public IActionResult Index()
        {
            ViewBag.Faculties = new SelectList(
                _context.Faculties.OrderBy(f => f.FacultyName).ToList(), "Id", "FacultyName");
            return View();
        }

        // ── AJAX: Lấy danh sách sinh viên ──────────────────────────────────
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _context.Students
                .Include(s => s.Faculty)
                .Select(s => new StudentFacultyViewModel
                {
                    Id          = s.Id,
                    StudentCode = s.StudentCode,
                    FullName    = s.FullName,
                    Age         = s.Age,
                    Email       = s.Email,
                    FacultyName = s.Faculty != null ? s.Faculty.FacultyName : "Chưa có khoa"
                })
                .OrderBy(s => s.StudentCode)
                .ToList();

            return Json(list);
        }

        // ── AJAX: Lấy 1 sinh viên theo id (dùng cho form Sửa) ──────────────
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var s = _context.Students.Find(id);
            if (s == null) return NotFound();
            return Json(s);
        }

        // ── AJAX: Thêm sinh viên ────────────────────────────────────────────
        [HttpPost]
        public IActionResult Create([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value!.Errors.Any())
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                    );
                return BadRequest(new { errors });
            }

            _context.Students.Add(student);
            _context.SaveChanges();
            return Ok(new { message = "Thêm sinh viên thành công!" });
        }

        // ── AJAX: Cập nhật sinh viên ────────────────────────────────────────
        [HttpPut]
        public IActionResult Edit(int id, [FromBody] Student student)
        {
            if (id != student.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value!.Errors.Any())
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                    );
                return BadRequest(new { errors });
            }

            var existing = _context.Students.Find(id);
            if (existing == null) return NotFound();

            existing.StudentCode = student.StudentCode;
            existing.FullName    = student.FullName;
            existing.Age         = student.Age;
            existing.Email       = student.Email;
            existing.FacultyId   = student.FacultyId;

            _context.SaveChanges();
            return Ok(new { message = "Cập nhật sinh viên thành công!" });
        }

        // ── AJAX: Xoá sinh viên ─────────────────────────────────────────────
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            _context.SaveChanges();
            return Ok(new { message = "Xóa sinh viên thành công!" });
        }
    }
}
