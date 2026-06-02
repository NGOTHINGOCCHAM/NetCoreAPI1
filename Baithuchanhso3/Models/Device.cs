using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiThucHanhSo3.Models
{
    public class Device
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã thiết bị không được để trống")]
        [StringLength(20)]
        [Display(Name = "Mã thiết bị")]
        public string DeviceCode { get; set; } = "";

        [Required(ErrorMessage = "Tên thiết bị không được để trống")]
        [StringLength(150)]
        [Display(Name = "Tên thiết bị")]
        public string DeviceName { get; set; } = "";

        [StringLength(300)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Số lượng tồn kho")]
        public int StockQuantity { get; set; } = 0;

        // Khoá ngoại: thuộc loại thiết bị nào
        [Required(ErrorMessage = "Vui lòng chọn loại thiết bị")]
        [Display(Name = "Loại thiết bị")]
        public int DeviceTypeId { get; set; }
        public DeviceType? DeviceType { get; set; }

        public ICollection<ImportDetail> ImportDetails { get; set; } = new List<ImportDetail>();
        public ICollection<ExportDetail> ExportDetails { get; set; } = new List<ExportDetail>();
    }
}
