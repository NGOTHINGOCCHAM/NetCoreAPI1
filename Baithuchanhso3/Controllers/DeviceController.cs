using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaiThucHanhSo3.Data;
using BaiThucHanhSo3.Models;

namespace BaiThucHanhSo3.Controllers
{
    public class DeviceController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DeviceController(ApplicationDbContext context) => _context = context;

        public IActionResult Index(string? search, int? deviceTypeId)
        {
            ViewBag.Search       = search;
            ViewBag.DeviceTypeId = deviceTypeId;
            ViewBag.DeviceTypes  = new SelectList(_context.DeviceTypes.OrderBy(t => t.TypeName).ToList(), "Id", "TypeName");

            var query = _context.Devices.Include(d => d.DeviceType).AsQueryable();
            if (!string.IsNullOrEmpty(search))
                query = query.Where(d => d.DeviceName.Contains(search) || d.DeviceCode.Contains(search));
            if (deviceTypeId.HasValue)
                query = query.Where(d => d.DeviceTypeId == deviceTypeId);

            return View(query.OrderBy(d => d.DeviceName).ToList());
        }

        public IActionResult Create()
        {
            ViewBag.DeviceTypeId = new SelectList(_context.DeviceTypes.OrderBy(t => t.TypeName).ToList(), "Id", "TypeName");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Device device)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DeviceTypeId = new SelectList(_context.DeviceTypes.OrderBy(t => t.TypeName).ToList(), "Id", "TypeName");
                return View(device);
            }
            _context.Devices.Add(device);
            _context.SaveChanges();
            TempData["Success"] = "Thêm thiết bị thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var device = _context.Devices.Find(id);
            if (device == null) return RedirectToAction("NotFoundPage", "Home");
            ViewBag.DeviceTypeId = new SelectList(_context.DeviceTypes.OrderBy(t => t.TypeName).ToList(), "Id", "TypeName", device.DeviceTypeId);
            return View(device);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Device device)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DeviceTypeId = new SelectList(_context.DeviceTypes.OrderBy(t => t.TypeName).ToList(), "Id", "TypeName", device.DeviceTypeId);
                return View(device);
            }
            _context.Devices.Update(device);
            _context.SaveChanges();
            TempData["Success"] = "Cập nhật thiết bị thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var device = _context.Devices.Include(d => d.DeviceType).FirstOrDefault(d => d.Id == id);
            if (device == null) return RedirectToAction("NotFoundPage", "Home");
            return View(device);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var device = _context.Devices.Find(id);
            if (device == null) return RedirectToAction("NotFoundPage", "Home");
            _context.Devices.Remove(device);
            _context.SaveChanges();
            TempData["Success"] = "Xóa thiết bị thành công!";
            return RedirectToAction("Index");
        }
    }
}
