using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(120)]
        public string? Description { get; set; }
        public ICollection<PointOfInterest> PointOfInterests { get; set; } = new List<PointOfInterest>();

        public City(string name)
        {
            Name= name;
        }
    }
}
