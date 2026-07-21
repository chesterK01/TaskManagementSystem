using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Không được vượt quá 50 ký tự")]
        public string UserName { get; set; } = null!;

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "*")]
        [MaxLength(255, ErrorMessage = "Không được vượt quá 255 ký tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
