using Abp.Application.Services.Dto;
using Abp.Web.Models;
using Flurl.Http;
using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.MultiTenancy.Dto;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.MultiTenancy
{
    using System.Collections.Generic;

    public class ProxyTenantAppService : ProxyAppServiceBase, ITenantAppService
    {
        public async Task<PagedResultDto<TenantListDto>> GetTenants(GetTenantsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<TenantListDto>>(GetEndpoint(nameof(GetTenants)), input);
        }

        public async Task<List<AllTenantDto>> GetAllTenants()
        {
            try
            {
                using (IFlurlClient client = new FlurlClient(ApiUrlConfig.BaseUrl))
                {
                    client.WithHeader("Accept", new MediaTypeWithQualityHeaderValue("application/json"));
                    client.WithHeader("User-Agent", AbpApiClient.UserAgent);

                    var response = await client
                        .Request(GetEndpoint(nameof(GetAllTenants)))
                        .GetAsync()
                        .ReceiveJson<AjaxResponse<List<AllTenantDto>>>();

                    if (response.Success)
                    {
                        return await Task.FromResult(response.Result);
                    }

                    return await Task.FromResult(new List<AllTenantDto>());
                }
                //return await ApiClient.GetAnonymousAsync<List<AllTenantDto>>(GetEndpoint(nameof(GetAllTenants)));
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task CreateTenant(CreateTenantInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(CreateTenant)), input);
        }

        public async Task<TenantEditDto> GetTenantForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<TenantEditDto>(GetEndpoint(nameof(GetTenantForEdit)), input);
        }

        public async Task UpdateTenant(TenantEditDto input)
        {
            await ApiClient.PutAsync(GetEndpoint(nameof(UpdateTenant)), input);
        }

        public async Task DeleteTenant(EntityDto input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(DeleteTenant)), input);
        }

        public async Task<GetTenantFeaturesEditOutput> GetTenantFeaturesForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetTenantFeaturesEditOutput>(GetEndpoint(nameof(GetTenantFeaturesForEdit)), input);
        }

        public async Task UpdateTenantFeatures(UpdateTenantFeaturesInput input)
        {
            await ApiClient.PutAsync(GetEndpoint(nameof(UpdateTenantFeatures)), input);
        }

        public async Task ResetTenantSpecificFeatures(EntityDto input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(ResetTenantSpecificFeatures)), input);
        }

        public async Task UnlockTenantAdmin(EntityDto input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(UnlockTenantAdmin)), input);
        }
    }
}