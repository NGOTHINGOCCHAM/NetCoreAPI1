using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiThucHanhSo3.Models
{
    public class ImportReceipt
    {
        public int Id { get; set; }

        [Display(Name = "Ngày nhập")]
        public DateTime ImportDate { get; set; } = DateTime.Now;

        [StringLength(200)]
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nhà cung cấp")]
        [Display(Name = "Nhà cung cấp")]
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public ICollection<ImportDetail> ImportDetails { get; set; } = new List<ImportDetail>();

        [NotMapped]
        public decimal TotalAmount => ImportDetails.Sum(d => d.LineTotal);
    }
}
