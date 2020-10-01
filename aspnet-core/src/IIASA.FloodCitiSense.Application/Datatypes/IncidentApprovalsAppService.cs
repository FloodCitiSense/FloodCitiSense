
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace IIASA.FloodCitiSense.Datatypes
{
	[AbpAuthorize(AppPermissions.Pages_IncidentApprovals)]
    public class IncidentApprovalsAppService : FloodCitiSenseAppServiceBase, IIncidentApprovalsAppService
    {
		 private readonly IRepository<IncidentApproval> _incidentApprovalRepository;
		 

		  public IncidentApprovalsAppService(IRepository<IncidentApproval> incidentApprovalRepository ) 
		  {
			_incidentApprovalRepository = incidentApprovalRepository;
			
		  }

		 public async Task<PagedResultDto<GetAllIncidentApprovalsOutput>> GetAll(GetAllIncidentApprovalsInput input)
         {
			var query = (from o in _incidentApprovalRepository.GetAll()
                         
                         select new GetAllIncidentApprovalsOutput() { IncidentApproval = ObjectMapper.Map<IncidentApprovalDto>(o)
						 
						 })
						 .WhereIf(
							!string.IsNullOrWhiteSpace(input.Filter),
						    e => false  || e.IncidentApproval.Comment.Contains(input.Filter)
							);

            var totalCount = await query.CountAsync();

            var incidentApprovals = await query
                .OrderBy(input.Sorting ?? "incidentApproval.id asc")
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<GetAllIncidentApprovalsOutput>(
                totalCount,
                incidentApprovals
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_IncidentApprovals_Edit)]
		 public async Task<GetIncidentApprovalForEditOutput> GetIncidentApprovalForEdit(EntityDto input)
         {
            var incidentApproval = await _incidentApprovalRepository.FirstOrDefaultAsync(input.Id);
            var output = new GetIncidentApprovalForEditOutput {IncidentApproval = ObjectMapper.Map<CreateOrEditIncidentApprovalDto>(incidentApproval)};

			
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditIncidentApprovalDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_IncidentApprovals_Create)]
		 private async Task Create(CreateOrEditIncidentApprovalDto input)
         {
            var incidentApproval = ObjectMapper.Map<IncidentApproval>(input);

			
			if (AbpSession.TenantId != null)
			{
				incidentApproval.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _incidentApprovalRepository.InsertAsync(incidentApproval);
         }

		 [AbpAuthorize(AppPermissions.Pages_IncidentApprovals_Edit)]
		 private async Task Update(CreateOrEditIncidentApprovalDto input)
         {
            var incidentApproval = await _incidentApprovalRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, incidentApproval);
         }

		 [AbpAuthorize(AppPermissions.Pages_IncidentApprovals_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _incidentApprovalRepository.DeleteAsync(input.Id);
         }


    }
}