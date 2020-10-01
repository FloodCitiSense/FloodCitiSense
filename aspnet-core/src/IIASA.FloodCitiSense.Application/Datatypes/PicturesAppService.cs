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
using Abp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace IIASA.FloodCitiSense.Datatypes
{
    [AbpAuthorize]
    public class PicturesAppService : FloodCitiSenseAppServiceBase, IPicturesAppService
    {
        private readonly IRepository<Picture> _pictureRepository;

        public PicturesAppService(IRepository<Picture> pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }

        public async Task<PagedResultDto<GetAllPicturesOutput>> GetAll(GetAllPicturesInput input)
        {
            var query = (from o in _pictureRepository.GetAll()

                         select new GetAllPicturesOutput()
                         {
                             Picture = ObjectMapper.Map<PictureDto>(o)
                         })
                         .WhereIf(
                            !string.IsNullOrWhiteSpace(input.Filter),
                            e => false || e.Picture.PhysicalPath.Contains(input.Filter) || e.Picture.URL.Contains(input.Filter) || e.Picture.Base64.Contains(input.Filter)
                            );

            var totalCount = await query.CountAsync();

            var pictures = await query
                .OrderBy(input.Sorting ?? "picture.id asc")
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<GetAllPicturesOutput>(
                totalCount,
                pictures
            );
        }

        public async Task<GetPictureForEditOutput> GetPictureForEdit(EntityDto<int> input)
        {
            var picture = await _pictureRepository.FirstOrDefaultAsync(input.Id);
            var output = new GetPictureForEditOutput { Picture = ObjectMapper.Map<CreateOrEditPictureDto>(picture) };

            return output;
        }

        public async Task<PictureDto> GetPictureById(EntityDto<int> input)
        {
            var picture = await _pictureRepository.Query(x => x.Where(y => y.Id == input.Id))
                            .FirstOrDefaultAsync();
            var output = ObjectMapper.Map<PictureDto>(picture);
            return output;
        }

        public async Task<ListResultDto<PictureDto>> GetPictureByIncidentId(EntityDto<int> input)
        {
            var pictureDtos = await _pictureRepository.Query(x => x.Where(y => y.Incident.Id == input.Id))
                .Select(x => ObjectMapper.Map<PictureDto>(x)).ToListAsync();

            return new ListResultDto<PictureDto>(pictureDtos);
        }

        public async Task CreateOrEdit(CreateOrEditPictureDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        private async Task Create(CreateOrEditPictureDto input)
        {
            var picture = ObjectMapper.Map<Picture>(input);

            if (AbpSession.TenantId != null)
            {
                picture.TenantId = AbpSession.TenantId.GetValueOrDefault(-1);
            }

            await _pictureRepository.InsertAsync(picture);
        }

        private async Task Update(CreateOrEditPictureDto input)
        {
            var picture = await _pictureRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, picture);
        }

        public async Task Delete(EntityDto<int> input)
        {
            await _pictureRepository.DeleteAsync(input.Id);
        }

        public Task<CreatePictureDto> CreatePicture()
        {
            //var files = HttpContext.
            return null;
        }
    }
}