using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models
{
    public class UpdateUserViewModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;

        [Display(Name = "Số điện thoại")]
        public string? Phone { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn vai trò")]
        [Display(Name = "Vai trò")]
        public int RoleId { get; set; }

        [Display(Name = "Đang hoạt động")]
        public bool IsActive { get; set; }
    }
}
