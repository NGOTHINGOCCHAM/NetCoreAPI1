namespace BaiThucHanhSo3.Models.ViewModels
{
    // ViewModel hiển thị thông tin sinh viên và khoa
    public class StudentFacultyViewModel
    {
        public int Id { get; set; }
        public string StudentCode { get; set; } = "";
        public string FullName { get; set; } = "";
        public int? Age { get; set; }
        public string? Email { get; set; }
        public string FacultyName { get; set; } = "Chưa có khoa";
    }
}
