using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    // interface = contract
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();

        // ? - for nullable
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);

        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);

        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);

    }
}
