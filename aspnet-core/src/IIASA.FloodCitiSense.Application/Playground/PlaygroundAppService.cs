using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using IIASA.FloodCitiSense.Playground.Dtos;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace IIASA.FloodCitiSense.Playground
{
	[AbpAuthorize(AppPermissions.Pages_CreativeEntiies)]
    public class PlaygroundAppService : FloodCitiSenseAppServiceBase, IPlaygroundAppService
    {
		 private readonly IRepository<CreativeEntiy> _creativeEntiyRepository;

		  public PlaygroundAppService(IRepository<CreativeEntiy> creativeEntiyRepository) 
		  {
			_creativeEntiyRepository = creativeEntiyRepository;
		  }

		 public async Task<PagedResultDto<CreativeEntiyDto>> GetAll(GetAllCreativeEntiiesInput input)
         {
            var query = _creativeEntiyRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                    e =>  e.name.Contains(input.Filter) || e.comment.Contains(input.Filter) 
                );

            var totalCount = await query.CountAsync();

            var creativeEntiies = await query
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<CreativeEntiyDto>(
                totalCount,
                ObjectMapper.Map<List<CreativeEntiyDto>>(creativeEntiies)
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_CreativeEntiies_Edit)]
		 public async Task<CreateOrEditCreativeEntiyDto> GetCreativeEntiyForEdit(EntityDto<int> input)
         {
            var creativeEntiy = await _creativeEntiyRepository.FirstOrDefaultAsync(input.Id);
            return ObjectMapper.Map<CreateOrEditCreativeEntiyDto>(creativeEntiy);
         }

		 public async Task CreateOrEdit(CreateOrEditCreativeEntiyDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_CreativeEntiies_Create)]
		 private async Task Create(CreateOrEditCreativeEntiyDto input)
         {
            var creativeEntiy = ObjectMapper.Map<CreativeEntiy>(input);

			if (AbpSession.TenantId != null)
            { 
            creativeEntiy.TenantId =  (int) AbpSession.TenantId;
            }

            await _creativeEntiyRepository.InsertAsync(creativeEntiy);
         }

		 [AbpAuthorize(AppPermissions.Pages_CreativeEntiies_Edit)]
		 private async Task Update(CreateOrEditCreativeEntiyDto input)
         {
            var creativeEntiy = await _creativeEntiyRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, creativeEntiy);
         }

		 [AbpAuthorize(AppPermissions.Pages_CreativeEntiies_Delete)]
         public async Task Delete(EntityDto<int> input)
         {
            await _creativeEntiyRepository.DeleteAsync(input.Id);
         }
    }
}