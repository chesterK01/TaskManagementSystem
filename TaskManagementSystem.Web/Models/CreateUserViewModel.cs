using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Tên đăng nhập")]
        [StringLength(50)]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "*")]
        [Display(Name = "Mật khẩu ban đầu")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        public string Password { get; set; } = null!;

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
    }
}
