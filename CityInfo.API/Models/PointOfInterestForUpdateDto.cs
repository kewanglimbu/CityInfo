using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class PointOfInterestForUpdateDto
    {
        [Required(ErrorMessage ="Please provide a name value")]
        [MaxLength(30)]
        public string Name {  get; set; }

        [MaxLength(120)]
        public string? Description { get; set; }
    }
}
