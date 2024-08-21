using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class PointOfInterestForCreationDto
    {
        [Required]
        [MaxLength(30)]
        public string Name {  get; set; }

        [MaxLength(120)]
        public string Description { get; set; }
    }
}
