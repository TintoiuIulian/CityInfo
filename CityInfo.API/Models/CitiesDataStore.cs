using System.Xml.Linq;

namespace CityInfo.API.Models
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {

            Cities = new List<CityDto>()
            {
            new CityDto()
            {
                Id = 1,
                Name = "New York",
                Description = "Best city",
                PointsOfInterest = new List<PointsOfInterestDto>()
                {
                    new PointsOfInterestDto()
                    {
                            Id = 1,
                            Name = "p1",
                            Description = "p1 description",
                    },
                     new PointsOfInterestDto()
                    {
                            Id = 2,
                            Name = "p2",
                            Description = "p2 description",
                    }

                }
            },
            new CityDto()
            {
                Id = 2,
                Name = "Rucar",
                Description = "Village",
                PointsOfInterest = new List<PointsOfInterestDto>()
                {
                    new PointsOfInterestDto()
                    {
                            Id = 3,
                            Name = "p3",
                            Description = "p3 description",
                    },
                     new PointsOfInterestDto()
                    {
                            Id = 4,
                            Name = "p4",
                            Description = "p4 description",
                    }

                }
            },
            new CityDto()
            {
                Id = 3,
                Name = "Brasov",
                Description = "Mountain city",
                PointsOfInterest = new List<PointsOfInterestDto>()
                {
                    new PointsOfInterestDto()
                    {
                            Id = 5,
                            Name = "p5",
                            Description = "p5 description",
                    },
                     new PointsOfInterestDto()
                    {
                            Id = 6,
                            Name = "p6",
                            Description = "p6 description",
                    }

                }
            }
            };

        }


    }
}
