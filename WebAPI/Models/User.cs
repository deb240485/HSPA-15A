using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class User : BaseEntity
    {
        [Required]
        public string? userName { get; set; }
        [Required]
        public byte[]? password { get; set; }

        public byte[]? passwordKey { get; set; }
    }
}