using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models
{
    public class UpdateProjectTaskViewModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Tiêu đề")]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Độ ưu tiên")]
        public byte Priority { get; set; }

        [Display(Name = "Hạn hoàn thành")]
        [DataType(DataType.Date)]
        public DateOnly? DueDate { get; set; }
    }
}
