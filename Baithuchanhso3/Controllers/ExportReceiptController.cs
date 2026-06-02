using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaiThucHanhSo3.Data;
using BaiThucHanhSo3.Models;

namespace BaiThucHanhSo3.Controllers
{
    public class ExportReceiptController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ExportReceiptController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var list = _context.ExportReceipts
                .Include(r => r.ExportDetails)
                .OrderByDescending(r => r.ExportDate)
                .ToList();
            return View(list);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ExportReceipt receipt)
        {
            if (!ModelState.IsValid) return View(receipt);
            _context.ExportReceipts.Add(receipt);
            _context.SaveChanges();
            TempData["Success"] = "Tạo phiếu xuất thành công! Hãy thêm thiết bị vào phiếu.";
            return RedirectToAction("Details", new { id = receipt.Id });
        }

        public IActionResult Delete(int id)
        {
            var receipt = _context.ExportReceipts.FirstOrDefault(r => r.Id == id);
            if (receipt == null) return RedirectToAction("NotFoundPage", "Home");
            return View(receipt);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var receipt = _context.ExportReceipts.Include(r => r.ExportDetails).FirstOrDefault(r => r.Id == id);
            if (receipt == null) return RedirectToAction("NotFoundPage", "Home");

            // Hoàn trả tồn kho
            foreach (var detail in receipt.ExportDetails)
            {
                var device = _context.Devices.Find(detail.DeviceId);
                if (device != null) device.StockQuantity += detail.Quantity;
            }
            _context.ExportDetails.RemoveRange(receipt.ExportDetails);
            _context.ExportReceipts.Remove(receipt);
            _context.SaveChanges();
            TempData["Success"] = "Xóa phiếu xuất thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var receipt = _context.ExportReceipts
                .Include(r => r.ExportDetails).ThenInclude(d => d.Device).ThenInclude(d => d!.DeviceType)
                .FirstOrDefault(r => r.Id == id);
            if (receipt == null) return RedirectToAction("NotFoundPage", "Home");

            // Chỉ hiển thị thiết bị còn tồn kho
            ViewBag.Devices = new SelectList(
                _context.Devices.Where(d => d.StockQuantity > 0).OrderBy(d => d.DeviceName).ToList(),
                "Id", "DeviceName");
            return View(receipt);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddDetail(int receiptId, int deviceId, int quantity, decimal exportPrice)
        {
            var device = _context.Devices.Find(deviceId);
            if (device == null) return RedirectToAction("Details", new { id = receiptId });

            if (device.StockQuantity < quantity)
            {
                TempData["Error"] = $"Tồn kho không đủ! Hiện còn {device.StockQuantity} thiết bị.";
                return RedirectToAction("Details", new { id = receiptId });
            }

            var existing = _context.ExportDetails
                .FirstOrDefault(d => d.ExportReceiptId == receiptId && d.DeviceId == deviceId);
            if (existing != null)
            {
                if (device.StockQuantity < existing.Quantity + quantity)
                {
                    TempData["Error"] = $"Tồn kho không đủ! Hiện còn {device.StockQuantity} thiết bị.";
                    return RedirectToAction("Details", new { id = receiptId });
                }
                device.StockQuantity -= quantity;
                existing.Quantity    += quantity;
                _context.ExportDetails.Update(existing);
            }
            else
            {
                device.StockQuantity -= quantity;
                _context.ExportDetails.Add(new ExportDetail
                {
                    ExportReceiptId = receiptId,
                    DeviceId        = deviceId,
                    Quantity        = quantity,
                    ExportPrice     = exportPrice
                });
            }
            _context.Devices.Update(device);
            _context.SaveChanges();
            TempData["Success"] = "Thêm thiết bị vào phiếu xuất thành công!";
            return RedirectToAction("Details", new { id = receiptId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult RemoveDetail(int detailId, int receiptId)
        {
            var detail = _context.ExportDetails.Find(detailId);
            if (detail != null)
            {
                var device = _context.Devices.Find(detail.DeviceId);
                if (device != null) device.StockQuantity += detail.Quantity;
                _context.ExportDetails.Remove(detail);
                _context.SaveChanges();
                TempData["Success"] = "Đã xoá thiết bị khỏi phiếu xuất!";
            }
            return RedirectToAction("Details", new { id = receiptId });
        }
    }
}
