using CityInfo.API.Entities;
using CityInfo.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    //DbContext ensures that the properties are not null when leaving the constructor
    public class CityInfoContext : DbContext
    {
        //register the entities
        // in caz ca apar erori lasasem  PointOfInterest in loc de City la primul DbSet
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointOfInterest { get; set; } = null!;

        // using the  DbContextOptions from  DbContext constructor
        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData
            (
            new City("New York City")
            {
                Id = 1,
                Description = "Best city"
            },
            new City("Rucar")
            {
                Id = 2,
                Description = "Village city"
            },
            new City("Brasov")
            {
                Id = 3,
                Description = "Mountain city"
            }
            );


            modelBuilder.Entity<PointOfInterest>().HasData
            (
            new PointOfInterest("Central Park")
            {
                Id = 1,
                CityId = 1,
                Description = "p1 description",
            }, new PointOfInterest("Monument")
            {
                Id = 2,
                CityId = 1,
                Description = "p2 description",
            }, new PointOfInterest("Brezaia")
            {
                Id = 3,
                CityId = 2,
                Description = "p3 description",
            }, new PointOfInterest("Profi")
            {
                Id = 4,
                CityId = 2,
                Description = "p4 description",
            },
            new PointOfInterest("Old center")
            {
                Id = 5,
                CityId = 3,
                Description = "p5 description",
            },
            new PointOfInterest("Times")
            {
                Id = 6,
                CityId = 3,
                Description = "p6 description",
            }
            );

            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //method available through the nuget package installed
        //    optionsBuilder.UseSqlite("connectionstring");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
