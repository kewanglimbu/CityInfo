using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CityDataStore
    {
        public List<CityDto> Cities { get; set; }
        public static CityDataStore Current { get; } = new CityDataStore();

        public CityDataStore()
        {
            Cities = new List<CityDto>
            {
                new CityDto
                {
                    Id = 1,
                    Name = "Gothgaun",
                    Description= "It is small peaceful semi urban area located in the morang district.",
                    PointOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto
                        {
                            Id= 1,
                            Name="Purbanchal University",
                            Description = "The PU is situated here. "
                        },
                        new PointOfInterestDto
                        {
                            Id=2,
                            Name="Panchayat School",
                            Description="The oldest government school in the gothgaun."
                        }
                    },
                },
                new CityDto
                {
                    Id=2,
                    Name="Nakhipot",
                    Description="It is the beautiful place located in the satdobato, lalipur. ",
                    PointOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto
                        {
                            Id=3,
                            Name="Kirat Yakthung Chumlung",
                            Description="Kirat Yakthung Chumlung is a social organization of the Limbu indigenous ethnic group of Nepal."
                        },
                        new PointOfInterestDto
                        {
                            Id=4,
                            Name="Ullens School",
                            Description="Ullens School is a private school in Nepal that offers holistic education from Kindergarten to IBDP."
                        },
                        new PointOfInterestDto
                        {
                            Id=5,
                            Name="Bhatbhateni Store",
                            Description="It is the biggest goods store in the Satdobato."
                        }
                    }
                }
            };
        }

    }
}
