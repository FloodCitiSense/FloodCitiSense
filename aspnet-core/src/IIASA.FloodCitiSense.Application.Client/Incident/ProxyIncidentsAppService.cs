using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Datatypes;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using System;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Incident
{
    class ProxyIncidentsAppService : ProxyAppServiceBase, IIncidentsAppService
    {
        public async Task<PagedResultDto<GetAllIncidentsOutput>> GetAll(GetAllIncidentsInput input)
        {
            return await ApiClient.GetAnonymousAsync<PagedResultDto<GetAllIncidentsOutput>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<GetIncidentForView> GetIncidentById(EntityDto input)
        {
            return await ApiClient.GetAsync<GetIncidentForView>(GetEndpoint(nameof(GetIncidentById)), input);
        }

        public async Task<PagedResultDto<GetAllIncidentsOutput>> GetIncidentByUserId(EntityDto input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetAllIncidentsOutput>>(GetEndpoint(nameof(GetIncidentByUserId)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(Delete)), input);
        }

        public Task<int> DeleteTempData()
        {
            throw new NotImplementedException();
        }

        public async Task<OutputDto> Update(IncidentViewModel input)
        {
            return await ApiClient.PutAsync<OutputDto>(GetEndpoint(nameof(Update)), input);
        }

        public async Task<OutputDto> Create(IncidentViewModel input)
        {
            return await ApiClient.PostAsync<OutputDto>(GetEndpoint(nameof(Create)), input);
        }
    }
}
