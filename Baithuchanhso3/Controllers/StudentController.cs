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

        // INDEX: dùng LinQ + ViewModel để hiển thị Mã SV, Họ tên, Khoa
        public IActionResult Index()
        {
            var list = _context.Students
                .Include(s => s.Faculty)
                .Select(s => new StudentFacultyViewModel
                {
                    Id        = s.Id,
                    StudentCode = s.StudentCode,
                    FullName  = s.FullName,
                    Age       = s.Age,
                    Email     = s.Email,
                    FacultyName = s.Faculty != null ? s.Faculty.FacultyName : "Chưa có khoa"
                })
                .ToList();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.FacultyId = new SelectList(_context.Faculties.ToList(), "Id", "FacultyName");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FacultyId = new SelectList(_context.Faculties.ToList(), "Id", "FacultyName");
                return View(student);
            }
            _context.Students.Add(student);
            _context.SaveChanges();
            TempData["Success"] = "Thêm sinh viên thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null) return RedirectToAction("NotFoundPage", "Home");
            ViewBag.FacultyId = new SelectList(_context.Faculties.ToList(), "Id", "FacultyName", student.FacultyId);
            return View(student);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FacultyId = new SelectList(_context.Faculties.ToList(), "Id", "FacultyName", student.FacultyId);
                return View(student);
            }
            _context.Students.Update(student);
            _context.SaveChanges();
            TempData["Success"] = "Cập nhật sinh viên thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var student = _context.Students.Include(s => s.Faculty).FirstOrDefault(s => s.Id == id);
            if (student == null) return RedirectToAction("NotFoundPage", "Home");
            return View(student);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null) return RedirectToAction("NotFoundPage", "Home");
            _context.Students.Remove(student);
            _context.SaveChanges();
            TempData["Success"] = "Xóa sinh viên thành công!";
            return RedirectToAction("Index");
        }
    }
}
