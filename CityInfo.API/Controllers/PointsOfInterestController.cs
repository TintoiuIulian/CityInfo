using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Converters;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CityInfo.API.Controllers
{
    [Route("api/v{version:apiVersion}/cities/{cityId}/pointsofinterest")]
    //[Authorize(Policy = "MustBeFromRucar")]
    [ApiVersion("2.0")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        // se injecteaza in constructor dependinta
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointsOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            //var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

            //if (!await _cityInfoRepository.CityNameMatchesCityId(cityName, cityId))
            //{
            //    return Forbid();
            //}

            if (!await _cityInfoRepository.CityExistAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accesing the points of interest");
                return NotFound();
            }
            var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointsOfInterestDto>>(pointsOfInterestForCity));

        }


        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<IEnumerable<PointsOfInterestDto>>> GetPointOfInterest(int cityId, int pointofinterestid)
        {
            if (!await _cityInfoRepository.CityExistAsync(cityId))
            {

                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointofinterestid);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointsOfInterestDto>(pointOfInterest));
        }

        [HttpPost]
        public async Task<ActionResult<PointsOfInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {

            if (!await _cityInfoRepository.CityExistAsync(cityId))
            {
                return NotFound();
            }

            //it doesn t map the pointOfInterest 
            //primary key id is auto generated
            //map PointOfInterestForCreationDto to PointOfInterest entity
            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();

            //PointOfInterest entity mapping back to a dto
            var createdPointOfInterestToReturn = _mapper.Map<Models.PointsOfInterestDto>(finalPointOfInterest);


            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, pointOfInterestId = createdPointOfInterestToReturn.Id }, createdPointOfInterestToReturn);
        }

        [HttpPut("{pointOfInterestId}")]


        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
            {
                return NotFound();

            }
            //map  PointOfInterestForUpdateDto to PointOfInterest entity
            //(source,destination)
            //automapper overrides the values in destination object with those from the source object
            _mapper.Map(pointOfInterest, pointOfInterestEntity);

            //persisting the changes to db
            await _cityInfoRepository.SaveChangesAsync();


            return NoContent();
        }


       

        //finding the entity first and then mapping to the PointOfInterestForUpdateDto(because the patch doc works with it) before applying the patch document
        //async calls in the db
        [HttpPatch("{pointOfInterestId}")]

        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
            {
                return NotFound();

            }

            //pointOfInterestEntity to PointOfInterestForUpdateDto(am creat si maparea in profile)
            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);


            //eroare aparuta in postman datorita ModelState ul
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);

            }

            //pointOfInterestToPatch(adica DTO ul) to pointOfInterestEntity
            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();


            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]

        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
            {
                return NotFound();

            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            _mailService.Send("Point of interest deleted.", $"Point of interest {pointOfInterestEntity} with id {pointOfInterestEntity.Id}");

            return NotFound();
        }


    }
}
