using System.ComponentModel.DataAnnotations;

namespace TodoWebApp.Share
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
