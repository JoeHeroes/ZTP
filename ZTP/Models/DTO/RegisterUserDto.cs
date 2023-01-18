using System.ComponentModel.DataAnnotations;

namespace ZTP.Models.DTO
{
    public class RegisterUserDto
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string ConfirmPassword { get; set; } = null!;
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public int RoleId { get; set; } = 1;
        //1 User 
        //2 Admin
    }
}
