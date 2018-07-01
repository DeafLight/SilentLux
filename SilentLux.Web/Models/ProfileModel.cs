using System.ComponentModel.DataAnnotations;

namespace SilentLux.Web.Models
{
    public class ProfileModel
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
