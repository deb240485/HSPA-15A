using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    public class CityDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        [RegularExpression(".*[a-zA-Z]+.*",ErrorMessage ="Numeric values are not allowed")]
        public string? Name { get; set; }
        [Required]
        public string? Country { get; set; }
    }
}