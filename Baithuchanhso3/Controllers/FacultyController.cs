using Microsoft.AspNetCore.Mvc;
using BaiThucHanhSo3.Data;
using BaiThucHanhSo3.Models;

namespace BaiThucHanhSo3.Controllers
{
    public class FacultyController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FacultyController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var list = _context.Faculties.OrderBy(f => f.FacultyName).ToList();
            return View(list);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Faculty faculty)
        {
            if (!ModelState.IsValid) return View(faculty);
            _context.Faculties.Add(faculty);
            _context.SaveChanges();
            TempData["Success"] = "Thêm khoa thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var faculty = _context.Faculties.Find(id);
            if (faculty == null) return RedirectToAction("NotFoundPage", "Home");
            return View(faculty);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Faculty faculty)
        {
            if (!ModelState.IsValid) return View(faculty);
            _context.Faculties.Update(faculty);
            _context.SaveChanges();
            TempData["Success"] = "Cập nhật khoa thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var faculty = _context.Faculties.Find(id);
            if (faculty == null) return RedirectToAction("NotFoundPage", "Home");
            return View(faculty);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var faculty = _context.Faculties.Find(id);
            if (faculty == null) return RedirectToAction("NotFoundPage", "Home");
            _context.Faculties.Remove(faculty);
            _context.SaveChanges();
            TempData["Success"] = "Xóa khoa thành công!";
            return RedirectToAction("Index");
        }
    }
}
