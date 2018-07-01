using System.ComponentModel.DataAnnotations;

namespace SilentLux.Web.Models
{
    public class SignUpModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string RepeatPassword { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
