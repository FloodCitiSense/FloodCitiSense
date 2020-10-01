using IIASA.FloodCitiSense.Authorization.Users;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using IIASA.FloodCitiSense.DataTypes.Exporting;
using IIASA.FloodCitiSense.DataTypes.Dtos;
using IIASA.FloodCitiSense.Dto;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace IIASA.FloodCitiSense.DataTypes
{
	[AbpAuthorize(AppPermissions.Pages_Cities)]
    public class CitiesAppService : FloodCitiSenseAppServiceBase, ICitiesAppService
    {
		 private readonly IRepository<City> _cityRepository;
		 private readonly ICitiesExcelExporter _citiesExcelExporter;
		 private readonly IRepository<User,long> _userRepository;
		 

		  public CitiesAppService(IRepository<City> cityRepository, ICitiesExcelExporter citiesExcelExporter , IRepository<User, long> userRepository) 
		  {
			_cityRepository = cityRepository;
			_citiesExcelExporter = citiesExcelExporter;
			_userRepository = userRepository;
		
		  }

		 public async Task<PagedResultDto<GetCityForView>> GetAll(GetAllCitiesInput input)
         {
			
			var filteredCities = _cityRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name.ToLower() == input.NameFilter.ToLower().Trim());


			var query = (from o in filteredCities
                         select new GetCityForView() {
							City = ObjectMapper.Map<CityDto>(o),
						})
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim());

            var totalCount = await query.CountAsync();

            var cities = await query
                .OrderBy(input.Sorting ?? "city.id asc")
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<GetCityForView>(
                totalCount,
                cities
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Cities_Edit)]
		 public async Task<GetCityForEditOutput> GetCityForEdit(EntityDto input)
         {
            var city = await _cityRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCityForEditOutput {City = ObjectMapper.Map<CreateOrEditCityDto>(city)};

		    if (output.City.UserId != null)
            {
                var user = await _userRepository.FirstOrDefaultAsync((long)output.City.UserId);
                output.UserName = user.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCityDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Cities_Create)]
		 private async Task Create(CreateOrEditCityDto input)
         {
            var city = ObjectMapper.Map<City>(input);
            await _cityRepository.InsertAsync(city);
         }

		 [AbpAuthorize(AppPermissions.Pages_Cities_Edit)]
		 private async Task Update(CreateOrEditCityDto input)
         {
            var city = await _cityRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, city);
         }

		 [AbpAuthorize(AppPermissions.Pages_Cities_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _cityRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCitiesToExcel(GetAllCitiesForExcelInput input)
         {
			
			var filteredCities = _cityRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name.ToLower() == input.NameFilter.ToLower().Trim());


			var query = (from o in filteredCities
                         select new GetCityForView() { 
							City = ObjectMapper.Map<CityDto>(o),
						 })
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim());


            var cityListDtos = await query.ToListAsync();

            return _citiesExcelExporter.ExportToFile(cityListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Cities)]
         public async Task<PagedResultDto<UserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new UserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name.ToString()
				});
			}

            return new PagedResultDto<UserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}