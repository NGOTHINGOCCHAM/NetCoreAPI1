using System.ComponentModel.DataAnnotations;

namespace BaiThucHanhSo3.Models
{
    public class DeviceType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên loại thiết bị không được để trống")]
        [StringLength(100)]
        [Display(Name = "Tên loại thiết bị")]
        public string TypeName { get; set; } = "";

        [StringLength(300)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}
