using Microsoft.AspNetCore.Mvc;
using BaiThucHanhSo3.Data;
using BaiThucHanhSo3.Models;

namespace BaiThucHanhSo3.Controllers
{
    public class DeviceTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DeviceTypeController(ApplicationDbContext context) => _context = context;

        public IActionResult Index(string? search)
        {
            ViewBag.Search = search;
            var query = _context.DeviceTypes.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                query = query.Where(t => t.TypeName.Contains(search));
            return View(query.OrderBy(t => t.TypeName).ToList());
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(DeviceType deviceType)
        {
            if (!ModelState.IsValid) return View(deviceType);
            _context.DeviceTypes.Add(deviceType);
            _context.SaveChanges();
            TempData["Success"] = "Thêm loại thiết bị thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var dt = _context.DeviceTypes.Find(id);
            if (dt == null) return RedirectToAction("NotFoundPage", "Home");
            return View(dt);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(DeviceType deviceType)
        {
            if (!ModelState.IsValid) return View(deviceType);
            _context.DeviceTypes.Update(deviceType);
            _context.SaveChanges();
            TempData["Success"] = "Cập nhật loại thiết bị thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var dt = _context.DeviceTypes.Find(id);
            if (dt == null) return RedirectToAction("NotFoundPage", "Home");
            return View(dt);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var dt = _context.DeviceTypes.Find(id);
            if (dt == null) return RedirectToAction("NotFoundPage", "Home");
            _context.DeviceTypes.Remove(dt);
            _context.SaveChanges();
            TempData["Success"] = "Xóa loại thiết bị thành công!";
            return RedirectToAction("Index");
        }
    }
}
