// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IncidentsAppService.cs" company="IIASA">
//   EOS
// </copyright>
// <summary>
//   Defines the IncidentsAppService.cs type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
using NetTopologySuite.Geometries;
using Omu.ValueInjecter;

namespace IIASA.FloodCitiSense.Datatypes
{
    //[AbpAuthorize(AppPermissions.Pages_Incidents)]
    /// <summary>
    ///     Defines the <see cref="IncidentsAppService" />
    /// </summary>
    [AbpAuthorize]
    public class IncidentsAppService : FloodCitiSenseAppServiceBase, IIncidentsAppService
    {
        private readonly IRepository<FloodType> _floodTypeRepository;

        /// <summary>
        ///     Defines the _incidentRepository
        /// </summary>
        private readonly IRepository<Incident> _incidentRepository;

        /// <summary>
        ///     Defines the _locationRepository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        /// <summary>
        ///     Defines the _pictureRepository
        /// </summary>
        private readonly IRepository<Picture> _pictureRepository;

        private readonly IRepository<User, long> _userRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IncidentsAppService" /> class.
        /// </summary>
        /// <param name="incidentRepository">The incidentRepository<see cref="IRepository{Incident}" /></param>
        /// <param name="locationRepository">The locationRepository<see cref="IRepository{Location}" /></param>
        /// <param name="pictureRepository">The pictureRepository<see cref="IRepository{Picture}" /></param>
        /// <param name="floodTypeRepository"></param>
        /// <param name="userRepository"></param>
        public IncidentsAppService(IRepository<Incident> incidentRepository, IRepository<Location> locationRepository,
            IRepository<Picture> pictureRepository, IRepository<FloodType> floodTypeRepository,
            IRepository<User, long> userRepository)
        {
            _incidentRepository = incidentRepository;
            _locationRepository = locationRepository;
            _pictureRepository = pictureRepository;
            _floodTypeRepository = floodTypeRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        ///     The GetAll
        /// </summary>
        /// <param name="input">The input<see cref="GetAllIncidentsInput" /></param>
        [AbpAllowAnonymous]
        public async Task<PagedResultDto<GetAllIncidentsOutput>> GetAll(GetAllIncidentsInput input)
        {
            var query = from o in _incidentRepository.GetAll()
                    .Include(x => x.Locations)
                    .Include(x => x.FloodTypes)
                    .Include(x => x.Pictures)
                        join u in _userRepository.GetAll() on o.CreatorUserId equals u.Id
                        select new GetAllIncidentsOutput
                        {
                            Incident = ObjectMapper.Map<IncidentDto>(o),
                            Username = u.UserName
                        };

            var totalCount = await query.CountAsync();

            var incidents = await query
                .OrderBy(input.Sorting ?? "incident.id asc")
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<GetAllIncidentsOutput>(
                totalCount,
                incidents
            );
        }

        /// <summary>
        ///     The GetIncidentForEdit
        /// </summary>
        /// <param name="input">The input<see cref="EntityDto" /></param>
        /// <returns>The <see cref="Task{GetIncidentForEditOutput}" /></returns>
        [AbpAllowAnonymous]
        public async Task<GetIncidentForView> GetIncidentById(EntityDto input)
        {
            var incident = await _incidentRepository.Query(x => x.Where(y => y.Id == input.Id))
                .Include(x => x.FloodTypes)
                .Include(x => x.Locations)
                .Include(x => x.Pictures).FirstOrDefaultAsync();
            var output = ObjectMapper.Map<IncidentDto>(incident);

            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == incident.CreatorUserId);

            return new GetIncidentForView
            {
                Incident = output,
                Username = user?.UserName
            };
        }

        public async Task<PagedResultDto<GetAllIncidentsOutput>> GetIncidentByUserId(EntityDto input)
        {
            var query = from o in _incidentRepository.GetAll()
                    .Where(x => x.CreatorUserId == input.Id)
                    .Include(x => x.Locations)
                    .Include(x => x.FloodTypes)
                    .Include(x => x.Pictures)
                        select new GetAllIncidentsOutput
                        {
                            Incident = ObjectMapper.Map<IncidentDto>(o)
                        };

            var totalCount = await query.CountAsync();

            var incidents = await query.ToListAsync();

            return new PagedResultDto<GetAllIncidentsOutput>(
                totalCount,
                incidents
            );
        }

        /// <summary>
        ///     The Update
        /// </summary>
        /// <param name="input">The input<see cref="IncidentEditDto" /></param>
        /// <returns>The <see cref="Task{OutputDto}" /></returns>
        public async Task<OutputDto> Update(IncidentViewModel input)
        {
            try
            {
                if (input == null)
                    return await Task.FromResult(new OutputDto
                    {
                        Message = "InvalidInput",
                        Success = false
                    });
                var mobileDataId = new Guid(input.MobileDataId);
                var currentIncident = await _incidentRepository.Query(x => x.Where(y => y.Id == input.Id))
                                                                .Include(x => x.FloodTypes)
                                                                .Include(x => x.Locations)
                                                               .Include(x => x.Pictures).FirstOrDefaultAsync();
                if(currentIncident == null)
                {
                    currentIncident = await _incidentRepository.Query(x => x.Where(y => y.MobileDataId == mobileDataId))
                                                                .Include(x => x.FloodTypes)
                                                                .Include(x => x.Locations)
                                                               .Include(x => x.Pictures).FirstOrDefaultAsync();
                }

                if (currentIncident != null)
                {
                    var tenantId = AbpSession.TenantId;

                    var userId = currentIncident.CreatorUserId;

                    currentIncident.TenantId = tenantId;

                    currentIncident.CreatorUserId = userId;

                    if (currentIncident.Pictures != null) foreach (var currentIncidentPicture in currentIncident?.Pictures) _pictureRepository.Delete(currentIncidentPicture.Id);
                    if (currentIncident.Locations != null) foreach (var location in currentIncident?.Locations) _locationRepository.Delete(location.Id);
                    if (currentIncident.FloodTypes != null) foreach (var floodType in currentIncident?.FloodTypes) _floodTypeRepository.Delete(floodType.Id);


                    var updated = ObjectMapper.Map(input, currentIncident);

                    foreach (var location in currentIncident.Locations)
                    {
                        location.TenantId = tenantId;
                        location.CreatorUserId = userId;
                    }

                    foreach (var picture in currentIncident.Pictures)
                    {
                        picture.TenantId = tenantId;
                        picture.CreatorUserId = userId;
                    }

                    foreach (var floodType in currentIncident.FloodTypes)
                    {
                        floodType.TenantId = tenantId;
                        floodType.CreatorUserId = userId;
                    }

                    var afterUpdated = await _incidentRepository.UpdateAsync(updated);

                    return await Task.FromResult(new OutputDto
                    {
                        Message = "UpdateSuccess",
                        Success = true,
                        Id = afterUpdated.Id
                    });
                }

                return await Task.FromResult(new OutputDto
                {
                    Message = "Failed",
                    Success = false
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.FromResult(new OutputDto
                {
                    Message = e.Message,
                    Success = false
                });
            }
        }



        /// <summary>
        ///     The Delete
        /// </summary>
        /// <param name="input">The input<see cref="EntityDto" /></param>
        /// <returns>The <see cref="Task" /></returns>
        public async Task Delete(EntityDto input)
        {
            var incident = _incidentRepository.GetAll()
                .Include(x => x.Locations)
                .Include(x => x.FloodTypes)
                .Include(x => x.Pictures)
                .FirstOrDefault(x => x.CreatorUserId == AbpSession.UserId && x.Id == input.Id);
            if(incident != null)
            {
                foreach (var incidentLocation in incident.Locations)
                    await _locationRepository.DeleteAsync(incidentLocation.Id);

                foreach (var incidentPicture in incident.Pictures)
                    await _pictureRepository.DeleteAsync(incidentPicture.Id);

                foreach (var incidentFloodType in incident.FloodTypes)
                    await _floodTypeRepository.DeleteAsync(incidentFloodType.Id);
                await _incidentRepository.DeleteAsync(incident.Id);
            }
            else
            {
                throw new Exception("Incident not found");
            }
        }

        [AbpAllowAnonymous]
        public async Task<int> DeleteTempData()
        {
            var userIds = new List<long>
            {
                19, 34, 55, 7, 48, 8, 33, 17, 18, 20, 3,
            };
            var count = 0;
            var incidents = _incidentRepository.GetAll().Where(x => userIds.Contains(x.CreatorUserId.Value))
                .Include(x => x.Locations)
                .Include(x => x.FloodTypes)
                .Include(x => x.Pictures);

            foreach (var incident in incidents)
            {
                foreach (var incidentLocation in incident.Locations)
                    await _locationRepository.DeleteAsync(incidentLocation.Id);

                foreach (var incidentPicture in incident.Pictures)
                    await _pictureRepository.DeleteAsync(incidentPicture.Id);

                foreach (var incidentFloodType in incident.FloodTypes)
                    await _floodTypeRepository.DeleteAsync(incidentFloodType.Id);
                await _incidentRepository.DeleteAsync(incident.Id);
                count++;
            }

            return count;
        }

        /// <summary>
        ///     The Create
        /// </summary>
        /// <param name="input">The input<see cref="IncidentViewModel" /></param>
        /// <returns>The <see cref="Task{OutputDto}" /></returns>
        public async Task<OutputDto> Create(IncidentViewModel input)
        {
            if (input == null)
                return await Task.FromResult(new OutputDto
                {
                    Message = "InvalidInput",
                    Success = false
                });
            try
            {
                var mobileDataId = new Guid(input.MobileDataId);

                var isCreated = await _incidentRepository.FirstOrDefaultAsync(x => x.MobileDataId == mobileDataId);
                var tenantId = AbpSession.TenantId;
                var userId = AbpSession.UserId;

                if (isCreated == null)
                {
                    var incident = ObjectMapper.Map<Incident>(input);
                    incident.TimeCreated = input.Date.UtcDateTime;
                    incident.CreatorUserId = userId;
                    incident.TenantId = tenantId;

                    foreach (var location in incident.Locations)
                    {
                        location.TenantId = tenantId;
                        location.CreatorUserId = userId;
                    }

                    foreach (var picture in incident.Pictures)
                    {
                        picture.TenantId = tenantId;
                        picture.CreatorUserId = userId;
                    }

                    foreach (var floodType in incident.FloodTypes)
                    {
                        floodType.TenantId = tenantId;
                        floodType.CreatorUserId = userId;
                    }

                    var newIncident = await _incidentRepository.InsertAsync(incident);

                    UnitOfWorkManager.Current.SaveChanges();

                    return await Task.FromResult(new OutputDto
                    {
                        Message = "CreateSuccess",
                        Success = true,
                        Id = newIncident.Id
                    });
                }

                return await Task.FromResult(new OutputDto
                {
                    Message = "AlreadyCreated",
                    Success = false,
                    Id = isCreated.Id
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.FromResult(new OutputDto
                {
                    Message = e.Message,
                    Success = false
                });
            }
        }

        private async Task UpdateLocationAndImage(IncidentViewModel input, int? tenantId, long? userId, Incident incident, Incident newIncident, bool isUpdate = false)
        {
            if (isUpdate)
            {
                if (newIncident.FloodTypes != null && newIncident.FloodTypes.Any())
                {
                    foreach (var floodType in newIncident.FloodTypes)
                    {
                        floodType.TenantId = tenantId;
                        floodType.CreatorUserId = userId;
                    }
                }
            }
            else
            {
                if (newIncident.FloodTypes != null && newIncident.FloodTypes.Any())
                {
                    foreach (var floodType in newIncident.FloodTypes)
                    {
                        floodType.TenantId = tenantId;
                        floodType.CreatorUserId = userId;
                    }
                }
            }



            if (input.Images != null && input.Images.Any())
                foreach (var inputImage in input.Images)
                {
                    var img = new Picture
                    {
                        Url = inputImage,
                        TenantId = tenantId,
                        MobileDataId = incident.MobileDataId,
                        Incident = newIncident,
                        CreatorUserId = userId
                    };

                    if (isUpdate)
                    {
                        await _pictureRepository.InsertAsync(img);
                    }
                    else
                    {
                        await _pictureRepository.UpdateAsync(img);
                    }
                }

            if (input.LocationDtos != null && input.LocationDtos.Any())
                foreach (var inputLocationDto in input.LocationDtos)
                {
                    var location = ObjectMapper.Map<Location>(inputLocationDto);
                    location.Point = new Point(location.Latitude, location.Longitude);
                    location.Incident = newIncident;
                    location.TenantId = tenantId;
                    location.LocationType = inputLocationDto.LocationType;
                    location.CreatorUserId = userId;
                    if (isUpdate)
                    {
                        await _locationRepository.InsertAsync(location);
                    }
                    else
                    {
                        await _locationRepository.UpdateAsync(location);
                    }
                }
        }


        /// <summary>
        ///     The Create
        /// </summary>
        /// <param name="input">The input<see cref="CreateOrEditIncidentDto" /></param>
        /// <returns>The <see cref="Task" /></returns>
        private async Task Create(CreateOrEditIncidentDto input)
        {
            var incident = ObjectMapper.Map<Incident>(input);

            if (AbpSession.TenantId != null) incident.TenantId = AbpSession.TenantId;

            await _incidentRepository.InsertAsync(incident);
        }
    }
}