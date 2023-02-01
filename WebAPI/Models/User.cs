using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? userName { get; set; }
        [Required]
        public string? password { get; set; }
    }
}