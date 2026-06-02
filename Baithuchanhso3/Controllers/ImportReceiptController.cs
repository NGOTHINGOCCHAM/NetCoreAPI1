using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaiThucHanhSo3.Data;
using BaiThucHanhSo3.Models;

namespace BaiThucHanhSo3.Controllers
{
    public class ImportReceiptController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ImportReceiptController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var list = _context.ImportReceipts
                .Include(r => r.Supplier)
                .Include(r => r.ImportDetails)
                .OrderByDescending(r => r.ImportDate)
                .ToList();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.SupplierId = new SelectList(_context.Suppliers.OrderBy(s => s.SupplierName).ToList(), "Id", "SupplierName");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ImportReceipt receipt)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SupplierId = new SelectList(_context.Suppliers.OrderBy(s => s.SupplierName).ToList(), "Id", "SupplierName");
                return View(receipt);
            }
            _context.ImportReceipts.Add(receipt);
            _context.SaveChanges();
            TempData["Success"] = "Tạo phiếu nhập thành công! Hãy thêm thiết bị vào phiếu.";
            return RedirectToAction("Details", new { id = receipt.Id });
        }

        public IActionResult Delete(int id)
        {
            var receipt = _context.ImportReceipts.Include(r => r.Supplier).FirstOrDefault(r => r.Id == id);
            if (receipt == null) return RedirectToAction("NotFoundPage", "Home");
            return View(receipt);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var receipt = _context.ImportReceipts.Include(r => r.ImportDetails).FirstOrDefault(r => r.Id == id);
            if (receipt == null) return RedirectToAction("NotFoundPage", "Home");

            // Hoàn trả tồn kho
            foreach (var detail in receipt.ImportDetails)
            {
                var device = _context.Devices.Find(detail.DeviceId);
                if (device != null) device.StockQuantity -= detail.Quantity;
            }
            _context.ImportDetails.RemoveRange(receipt.ImportDetails);
            _context.ImportReceipts.Remove(receipt);
            _context.SaveChanges();
            TempData["Success"] = "Xóa phiếu nhập thành công!";
            return RedirectToAction("Index");
        }

        // Chi tiết phiếu nhập + thêm thiết bị
        public IActionResult Details(int id)
        {
            var receipt = _context.ImportReceipts
                .Include(r => r.Supplier)
                .Include(r => r.ImportDetails).ThenInclude(d => d.Device).ThenInclude(d => d!.DeviceType)
                .FirstOrDefault(r => r.Id == id);
            if (receipt == null) return RedirectToAction("NotFoundPage", "Home");

            ViewBag.Devices = new SelectList(_context.Devices.OrderBy(d => d.DeviceName).ToList(), "Id", "DeviceName");
            return View(receipt);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddDetail(int receiptId, int deviceId, int quantity, decimal importPrice)
        {
            var device = _context.Devices.Find(deviceId);
            if (device == null) return RedirectToAction("Details", new { id = receiptId });

            var existing = _context.ImportDetails
                .FirstOrDefault(d => d.ImportReceiptId == receiptId && d.DeviceId == deviceId);
            if (existing != null)
            {
                device.StockQuantity += quantity;
                existing.Quantity    += quantity;
                _context.ImportDetails.Update(existing);
            }
            else
            {
                device.StockQuantity += quantity;
                _context.ImportDetails.Add(new ImportDetail
                {
                    ImportReceiptId = receiptId,
                    DeviceId        = deviceId,
                    Quantity        = quantity,
                    ImportPrice     = importPrice
                });
            }
            _context.Devices.Update(device);
            _context.SaveChanges();
            TempData["Success"] = "Thêm thiết bị vào phiếu nhập thành công!";
            return RedirectToAction("Details", new { id = receiptId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult RemoveDetail(int detailId, int receiptId)
        {
            var detail = _context.ImportDetails.Find(detailId);
            if (detail != null)
            {
                var device = _context.Devices.Find(detail.DeviceId);
                if (device != null) device.StockQuantity -= detail.Quantity;
                _context.ImportDetails.Remove(detail);
                _context.SaveChanges();
                TempData["Success"] = "Đã xoá thiết bị khỏi phiếu nhập!";
            }
            return RedirectToAction("Details", new { id = receiptId });
        }
    }
}
