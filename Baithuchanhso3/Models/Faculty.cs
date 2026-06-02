using System.ComponentModel.DataAnnotations;

namespace BaiThucHanhSo3.Models
{
    public class Faculty
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên khoa không được để trống")]
        [StringLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        [Display(Name = "Tên khoa")]
        public string FacultyName { get; set; } = "";

        // Navigation property: 1 khoa có nhiều sinh viên
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
