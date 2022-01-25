using System.ComponentModel.DataAnnotations;

namespace Management.Data.DTO
{
    public class GroupDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
