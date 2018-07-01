using System.ComponentModel.DataAnnotations;

namespace SilentLux.Web.Models
{
    public class SignInModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
