using System.ComponentModel.DataAnnotations;

namespace BaiThucHanhSo3.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã sinh viên không được để trống")]
        [StringLength(10, ErrorMessage = "Tối đa 10 ký tự")]
        [Display(Name = "Mã sinh viên")]
        public string StudentCode { get; set; } = "";

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; } = "";

        [Range(18, 60, ErrorMessage = "Tuổi phải từ 18 đến 60")]
        [Display(Name = "Tuổi")]
        public int? Age { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        // Khoá ngoại: Một sinh viên chỉ thuộc một khoa
        [Display(Name = "Khoa")]
        public int? FacultyId { get; set; }
        public Faculty? Faculty { get; set; }
    }
}
