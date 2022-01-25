using System.ComponentModel.DataAnnotations;

namespace Management.Data.DTO
{
    public class UserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        public int GroupId { get; set; }        
    }
}
