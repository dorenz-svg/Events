using System.ComponentModel.DataAnnotations;

namespace Events.Models.Request
{
    public class RegistrationRequest:LoginRequest
    {
        [Required]
        public string UserName { get; set; }
    }
}
