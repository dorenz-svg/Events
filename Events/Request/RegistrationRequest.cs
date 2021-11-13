using System.ComponentModel.DataAnnotations;

namespace Events.Models.Request
{
    public class RegistrationRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
