//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ProxyLocationsAppService.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   ProxyLocationsAppService.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Location
{
    public class ProxyLocationsAppService : ProxyAppServiceBase, ILocationsAppService
    {
        public async Task<PagedResultDto<GetAllLocationsOutput>> GetAll(GetAllLocationsInput input)
        {
            return await ApiClient.GetAnonymousAsync<PagedResultDto<GetAllLocationsOutput>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<PagedResultDto<GetAllLocationsOutput>> GetAllIncident(GetAllLocationsInput input)
        {
            return await ApiClient.GetAnonymousAsync<PagedResultDto<GetAllLocationsOutput>>(GetEndpoint(nameof(GetAllIncident)), input);
        }

        public async Task<PagedResultDto<GetFilterLocationForView>> GetFilteredIncident(FilterInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetFilterLocationForView>>(GetEndpoint(nameof(GetFilteredIncident)), input);
        }

        public async Task<PagedResultDto<GetAllLocationsOutput>> GetAllOtherUserIncident(GetAllLocationsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetAllLocationsOutput>>(GetEndpoint(nameof(GetAllOtherUserIncident)), input);
        }

        public async Task<PagedResultDto<GetAllLocationsOutput>> GetAllCurrentUserIncident(GetAllLocationsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetAllLocationsOutput>>(GetEndpoint(nameof(GetAllCurrentUserIncident)), input);
        }

        public async Task<PagedResultDto<GetAllLocationsOutput>> GetAllUser(GetAllLocationsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetAllLocationsOutput>>(GetEndpoint(nameof(GetAllUser)), input);
        }

        public Task<LocationDto> GetLocationById(EntityDto input)
        {
            throw new System.NotImplementedException();
        }

        public Task<ListResultDto<LocationDto>> GetLocationByIncidentId(EntityDto input)
        {
            throw new System.NotImplementedException();
        }

        public Task<GetLocationForEditOutput> GetLocationForEdit(EntityDto input)
        {
            throw new System.NotImplementedException();
        }

        public Task CreateOrEdit(CreateOrEditLocationDto input)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(EntityDto input)
        {
            throw new System.NotImplementedException();
        }
    }
}