using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BaiThucHanhSo3.Data;
using BaiThucHanhSo3.Models;

namespace BaiThucHanhSo3.Controllers
{
    // Buổi 10: Đọc dữ liệu từ file Excel và lưu vào bảng Student
    public class ImportController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ImportController(ApplicationDbContext context) => _context = context;

        // Trang upload + hướng dẫn cột Excel
        public IActionResult Index() => View();

        // Tải file Excel mẫu về
        public IActionResult DownloadTemplate()
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Students");

            // Tiêu đề cột
            ws.Cell(1, 1).Value = "StudentCode";
            ws.Cell(1, 2).Value = "FullName";
            ws.Cell(1, 3).Value = "Age";
            ws.Cell(1, 4).Value = "Email";

            // In đậm tiêu đề
            var headerRow = ws.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.LightBlue;

            // Dữ liệu mẫu
            ws.Cell(2, 1).Value = "SV001";
            ws.Cell(2, 2).Value = "Nguyễn Văn A";
            ws.Cell(2, 3).Value = 20;
            ws.Cell(2, 4).Value = "sva@email.com";

            ws.Cell(3, 1).Value = "SV002";
            ws.Cell(3, 2).Value = "Trần Thị B";
            ws.Cell(3, 3).Value = 21;
            ws.Cell(3, 4).Value = "svb@email.com";

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            stream.Position = 0;
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "MauImportStudent.xlsx");
        }

        // Xử lý upload và đọc Excel
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Upload(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                TempData["Error"] = "Vui lòng chọn file Excel!";
                return RedirectToAction("Index");
            }

            var extension = Path.GetExtension(excelFile.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls")
            {
                TempData["Error"] = "Chỉ chấp nhận file Excel (.xlsx, .xls)!";
                return RedirectToAction("Index");
            }

            int successCount = 0;
            int skipCount    = 0;
            var errors       = new List<string>();

            using var stream = excelFile.OpenReadStream();
            using var wb     = new XLWorkbook(stream);
            var ws = wb.Worksheets.First();

            // Đọc từ dòng 2 (dòng 1 là tiêu đề)
            var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;

            for (int row = 2; row <= lastRow; row++)
            {
                var studentCode = ws.Cell(row, 1).GetString().Trim();
                var fullName    = ws.Cell(row, 2).GetString().Trim();
                var ageStr      = ws.Cell(row, 3).GetString().Trim();
                var email       = ws.Cell(row, 4).GetString().Trim();

                // Bỏ qua dòng trống
                if (string.IsNullOrEmpty(studentCode) && string.IsNullOrEmpty(fullName))
                {
                    skipCount++;
                    continue;
                }

                // Validate bắt buộc
                if (string.IsNullOrEmpty(studentCode))
                {
                    errors.Add($"Dòng {row}: Mã sinh viên không được để trống.");
                    continue;
                }
                if (string.IsNullOrEmpty(fullName))
                {
                    errors.Add($"Dòng {row}: Họ tên không được để trống.");
                    continue;
                }

                // Kiểm tra trùng mã sinh viên
                if (_context.Students.Any(s => s.StudentCode == studentCode))
                {
                    errors.Add($"Dòng {row}: Mã sinh viên '{studentCode}' đã tồn tại, bỏ qua.");
                    skipCount++;
                    continue;
                }

                int? age = null;
                if (!string.IsNullOrEmpty(ageStr) && int.TryParse(ageStr, out int parsedAge))
                {
                    if (parsedAge >= 18 && parsedAge <= 60)
                        age = parsedAge;
                    else
                        errors.Add($"Dòng {row}: Tuổi '{ageStr}' không hợp lệ (18-60), trường này sẽ bị bỏ qua.");
                }

                var student = new Student
                {
                    StudentCode = studentCode[..Math.Min(studentCode.Length, 10)],
                    FullName    = fullName[..Math.Min(fullName.Length, 50)],
                    Age         = age,
                    Email       = string.IsNullOrEmpty(email) ? null : email
                };

                _context.Students.Add(student);
                successCount++;
            }

            _context.SaveChanges();

            TempData["ImportSuccess"] = $"Import thành công {successCount} sinh viên.";
            if (skipCount > 0)
                TempData["ImportSkip"] = $"Bỏ qua {skipCount} dòng (trùng hoặc rỗng).";
            if (errors.Any())
                TempData["ImportErrors"] = string.Join("|", errors);

            return RedirectToAction("Index");
        }
    }
}
