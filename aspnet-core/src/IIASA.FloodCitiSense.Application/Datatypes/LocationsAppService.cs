using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using IIASA.FloodCitiSense.Authorization.Users;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using Microsoft.EntityFrameworkCore;

namespace IIASA.FloodCitiSense.Datatypes
{
    [AbpAuthorize]
    public class LocationsAppService : FloodCitiSenseAppServiceBase, ILocationsAppService
    {
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<User, long> _userRepository;

        public LocationsAppService(IRepository<Location> locationRepository, IRepository<User, long> userRepository)
        {
            _locationRepository = locationRepository;
            _userRepository = userRepository;
        }

        public async Task<PagedResultDto<GetAllLocationsOutput>> GetAll(GetAllLocationsInput input)
        {
            var query = from o in _locationRepository.GetAll().Include(x => x.Incident)
                select new GetAllLocationsOutput
                {
                    Location = ObjectMapper.Map<LocationDto>(o)
                };
            var totalCount = await query.CountAsync();

            var locations = await query
                .OrderBy(input.Sorting ?? "location.id asc")
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<GetAllLocationsOutput>(
                totalCount,
                locations
            );
        }

        public async Task<PagedResultDto<GetAllLocationsOutput>> GetAllIncident(GetAllLocationsInput input)
        {
            var query = from o in _locationRepository.GetAll().Include(x => x.Incident)
                    .Where(x => x.LocationType == LocationType.Incident)
                select new GetAllLocationsOutput
                {
                    Location = ObjectMapper.Map<LocationDto>(o)
                };

            var totalCount = await query.CountAsync();

            var locations = await query
                .OrderBy(input.Sorting ?? "location.id asc")
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<GetAllLocationsOutput>(
                totalCount,
                locations
            );
        }

        public async Task<PagedResultDto<GetFilterLocationForView>> GetFilteredIncident(FilterInput input)
        {
            var query = from o in _locationRepository.GetAll().Include(x => x.Incident)
                    .Where(x => x.LocationType == LocationType.Incident)
                    .Include(x => x.Incident)
                    .WhereIf(input.From != DateTime.MinValue && input.To != DateTime.MinValue,
                        x => x.Incident.TimeCreated != null && input.From.ToUniversalTime() <= x.Incident.TimeCreated &&
                             input.To.ToUniversalTime() >= x.Incident.TimeCreated)
                    .WhereIf(input.LastTime != DateTime.MinValue,
                        x => x.Incident.TimeCreated != null &&
                             input.LastTime.ToUniversalTime().CompareTo(x.Incident.TimeCreated) < 0)
                select new GetFilterLocationForView
                {
                    Location = ObjectMapper.Map<LocationDto>(o),
                    CreatedUserUserId = o.CreatorUserId,
                    LoggedInUserId = AbpSession.UserId
                };

            var totalCount = await query.CountAsync();

            var locations = await query.ToListAsync();

            return new PagedResultDto<GetFilterLocationForView>(
                totalCount,
                locations
            );
        }

        public async Task<PagedResultDto<GetAllLocationsOutput>> GetAllOtherUserIncident(GetAllLocationsInput input)
        {
            var query = from o in _locationRepository.GetAll().Include(x => x.Incident).Where(x =>
                    x.LocationType == LocationType.Incident && x.CreatorUserId != AbpSession.UserId)
                select new GetAllLocationsOutput
                {
                    Location = ObjectMapper.Map<LocationDto>(o)
                };

            var totalCount = await query.CountAsync();

            var locations = await query
                .OrderBy(input.Sorting ?? "location.id asc")
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<GetAllLocationsOutput>(
                totalCount,
                locations
            );
        }

        public async Task<PagedResultDto<GetAllLocationsOutput>> GetAllCurrentUserIncident(GetAllLocationsInput input)
        {
            var query = from o in _locationRepository.GetAll()
                    .Include(x => x.Incident)
                    .Where(x => x.LocationType == LocationType.Incident && x.CreatorUserId == AbpSession.UserId)
                select new GetAllLocationsOutput
                {
                    Location = ObjectMapper.Map<LocationDto>(o)
                };

            var totalCount = await query.CountAsync();

            var locations = await query
                .OrderBy(input.Sorting ?? "location.id asc")
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<GetAllLocationsOutput>(
                totalCount,
                locations
            );
        }

        public async Task<PagedResultDto<GetAllLocationsOutput>> GetAllUser(GetAllLocationsInput input)
        {
            var query = from o in _locationRepository.GetAll().Include(x => x.Incident)
                    .Where(x => x.LocationType == LocationType.User)
                select new GetAllLocationsOutput
                {
                    Location = ObjectMapper.Map<LocationDto>(o)
                };

            var totalCount = await query.CountAsync();

            var locations = await query
                .OrderBy(input.Sorting ?? "location.id asc")
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<GetAllLocationsOutput>(
                totalCount,
                locations
            );
        }

        public async Task<LocationDto> GetLocationById(EntityDto input)
        {
            var location = await _locationRepository.Query(x => x.Where(y => y.Id == input.Id))
                .FirstOrDefaultAsync();
            var output = ObjectMapper.Map<LocationDto>(location);
            return output;
        }

        public async Task<ListResultDto<LocationDto>> GetLocationByIncidentId(EntityDto input)
        {
            var locationDtos = await _locationRepository.Query(x => x.Where(y => y.Incident.Id == input.Id))
                .Select(x => ObjectMapper.Map<LocationDto>(x)).ToListAsync();

            return new ListResultDto<LocationDto>(locationDtos);
        }

        public async Task<GetLocationForEditOutput> GetLocationForEdit(EntityDto input)
        {
            var location = await _locationRepository.FirstOrDefaultAsync(input.Id);
            var output = new GetLocationForEditOutput {Location = ObjectMapper.Map<CreateOrEditLocationDto>(location)};

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLocationDto input)
        {
            if (input.Id == null)
                await Create(input);
            else
                await Update(input);
        }

        public async Task Delete(EntityDto input)
        {
            await _locationRepository.DeleteAsync(input.Id);
        }

        private async Task Create(CreateOrEditLocationDto input)
        {
            var location = ObjectMapper.Map<Location>(input);

            if (AbpSession.TenantId != null) location.TenantId = AbpSession.TenantId.GetValueOrDefault(-1);

            await _locationRepository.InsertAsync(location);
        }

        private async Task Update(CreateOrEditLocationDto input)
        {
            var location = await _locationRepository.FirstOrDefaultAsync((int) input.Id);
            ObjectMapper.Map(input, location);
        }
    }
}