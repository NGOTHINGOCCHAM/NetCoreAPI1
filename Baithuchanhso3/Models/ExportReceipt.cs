using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiThucHanhSo3.Models
{
    public class ExportReceipt
    {
        public int Id { get; set; }

        [Display(Name = "Ngày xuất")]
        public DateTime ExportDate { get; set; } = DateTime.Now;

        [StringLength(150)]
        [Display(Name = "Người nhận")]
        public string? RecipientName { get; set; }

        [StringLength(200)]
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        public ICollection<ExportDetail> ExportDetails { get; set; } = new List<ExportDetail>();

        [NotMapped]
        public decimal TotalAmount => ExportDetails.Sum(d => d.LineTotal);
    }
}
