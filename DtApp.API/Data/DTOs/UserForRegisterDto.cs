using System.ComponentModel.DataAnnotations;

namespace DtApp.API.Data.DTOs
{
    public class UserForRegisterDto
    {
        [Required]
        public string username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Your password should be between 4 and 8 characters")]
        public string password { get; set; }
    }
}