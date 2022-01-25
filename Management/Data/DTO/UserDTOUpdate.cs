using System.ComponentModel.DataAnnotations;

namespace Management.Data.DTO
{
    public class UserDTOUpdate
    {
        public int Id { get; set; }
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
