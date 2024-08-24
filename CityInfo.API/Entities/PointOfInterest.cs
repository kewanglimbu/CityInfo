using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(120)]
        public string? Description { get; set; }

        [ForeignKey("CityId")]
        public int CityId { get; set; }
        public City? City { get; set; }

        public PointOfInterest(string name)
        {
            Name = name;
        }
    }
}
