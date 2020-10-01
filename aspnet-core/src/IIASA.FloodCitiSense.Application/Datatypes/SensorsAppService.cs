
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using IIASA.FloodCitiSense.Datatypes.Exporting;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using IIASA.FloodCitiSense.Dto;
using Abp.Application.Services.Dto;
using IIASA.FloodCitiSense.Authorization;
using Abp.Authorization;
using Abp.UI;
using IIASA.FloodCitiSense.Authorization.Users;
using Microsoft.EntityFrameworkCore;

namespace IIASA.FloodCitiSense.Datatypes
{
    [AbpAllowAnonymous]
    public class SensorsAppService : FloodCitiSenseAppServiceBase, ISensorsAppService
    {
        private readonly IRepository<Sensor> _sensorRepository;
        private readonly UserManager _userManager;
        private readonly ISensorsExcelExporter _sensorsExcelExporter;


        public SensorsAppService(IRepository<Sensor> sensorRepository, UserManager userManager, ISensorsExcelExporter sensorsExcelExporter)
        {
            _sensorRepository = sensorRepository;
            _userManager = userManager;
            _sensorsExcelExporter = sensorsExcelExporter;

        }

        public async Task<PagedResultDto<GetSensorForView>> GetAll(GetAllSensorsInput input)
        {

            var query = (from o in _sensorRepository.GetAll()
                                    .Include(x => x.User)
                                    .Include(x => x.Locations)
                                    .Include(x => x.Pictures)

                         select new GetSensorForView()
                         {
                             Sensor = ObjectMapper.Map<SensorDto>(o)
                         });

            var totalCount = await query.CountAsync();

            var sensors = await query
                .OrderBy(input.Sorting ?? "sensor.id asc")
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<GetSensorForView>(
                totalCount,
                sensors
            );
        }

        public async Task<PagedResultDto<GetSensorForView>> GetSensorByUserId(EntityDto<long> input)
        {


            var query = (from o in _sensorRepository.GetAll()
                    .Include(x => x.User)
                    .Include(x => x.Locations)
                    .Include(x => x.Pictures)
                    .Where(x => x.User.Id == input.Id)
                         select new GetSensorForView()
                         {
                             Sensor = ObjectMapper.Map<SensorDto>(o)
                         });

            var totalCount = await query.CountAsync();

            var sensors = await query
                .ToListAsync();

            return new PagedResultDto<GetSensorForView>(
                totalCount,
                sensors
            );
        }

        public async Task<GetSensorForEditOutput> GetSensorForEdit(EntityDto input)
        {
            var sensor = await _sensorRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSensorForEditOutput { Sensor = ObjectMapper.Map<CreateOrEditSensorDto>(sensor) };

            return output;
        }

        public async Task<SensorDto> GetSensorById(EntityDto input)
        {
            var sensor = await _sensorRepository.GetAll()
                                                .Include(x => x.User)
                                                .Include(x => x.Locations)
                                                .Include(x => x.Pictures)
                                                .FirstOrDefaultAsync(x => x.Id == input.Id);

            var output = ObjectMapper.Map<SensorDto>(sensor);

            return output;
        }

        public async Task<SensorDto> CreateOrEdit(CreateOrEditSensorDto input)
        {
            return input.Id == null ? await Create(input) : await Update(input);
        }

        
        public async Task<SensorDto> Create(CreateOrEditSensorDto input)
        {
            var sensor = ObjectMapper.Map<Sensor>(input);
            PrepareSensor(input, sensor);
            var createdSensor = await _sensorRepository.InsertAsync(sensor);
            UnitOfWorkManager.Current.SaveChanges();
            return ObjectMapper.Map<SensorDto>(createdSensor);
        }

        private void PrepareSensor(CreateOrEditSensorDto input, Sensor sensor)
        {
            if (input.UserId.HasValue && AbpSession.TenantId.HasValue)
            {
                var user = _userManager.GetUser(new UserIdentifier(AbpSession.TenantId, input.UserId.Value));

                if (user != null)
                {
                    sensor.User = user;
                }
            }

            if (AbpSession.TenantId.HasValue)
            {
                sensor.TenantId = (int?) AbpSession.TenantId;

                foreach (var sensorLocation in sensor.Locations)
                {
                    sensorLocation.TenantId = sensor.TenantId;
                }

                foreach (var sensorPicture in sensor.Pictures)
                {
                    sensorPicture.TenantId = sensor.TenantId;
                }
            }
        }

        public async Task<SensorDto> Update(CreateOrEditSensorDto input)
        {
            if (input.Id != null)
            {
                var sensor = await _sensorRepository.FirstOrDefaultAsync((int)input.Id);

                if (sensor != null)
                {
                    var updated = ObjectMapper.Map(input, sensor);
                    PrepareSensor(input, updated);
                    var output = await _sensorRepository.UpdateAsync(updated);
                    return ObjectMapper.Map<SensorDto>(output); ;
                }
            }

            throw new UserFriendlyException("User update failed");
        }

        public async Task Delete(EntityDto input)
        {
            await _sensorRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSensorsToExcel(GetAllSensorsForExcelInput input)
        {

            var filteredSensors = _sensorRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.ToLower() == input.NameFilter.ToLower().Trim());


            var query = (from o in filteredSensors

                         select new GetSensorForView()
                         {
                             Sensor = ObjectMapper.Map<SensorDto>(o)

                         })
                         ;


            var sensorListDtos = await query.ToListAsync();

            return _sensorsExcelExporter.ExportToFile(sensorListDtos);
        }


    }
}