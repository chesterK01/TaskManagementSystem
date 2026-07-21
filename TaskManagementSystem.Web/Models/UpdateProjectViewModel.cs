using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models
{
    public class UpdateProjectViewModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Tên dự án")]
        [StringLength(200)]
        public string ProjectName { get; set; } = null!;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        public DateOnly? StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        [DataType(DataType.Date)]
        public DateOnly? EndDate { get; set; }

        [Display(Name = "Trạng thái")]
        public byte Status { get; set; }
    }
}
