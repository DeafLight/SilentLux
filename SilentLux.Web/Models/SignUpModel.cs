using System.ComponentModel.DataAnnotations;

namespace SilentLux.Web.Models
{
    public class SignUpModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string RepeatPassword { get; set; }
    }
}
