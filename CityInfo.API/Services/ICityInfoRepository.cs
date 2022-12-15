using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    // interface = contract
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();

        //tuple
        Task<(IEnumerable<City>, PaginationMetaData)> GetCitiesAsync(string? name, string? searchQuery,int pageNumber, int pageSize);

        // ? - for nullable
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);

        Task<bool> CityExistAsync(int cityId);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);

        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);

        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

        // in memory operation not I/O operation so async isnt necessary here
        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        Task<bool> CityNameMatchesCityId(string? cityName,int cityId);

        Task<bool> SaveChangesAsync();


    }
}
