using IIASA.FloodCitiSense.Datatypes.Dtos;
using IIASA.FloodCitiSense.Mobile.Core.Core.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Report
{
    public interface IIncidentService
    {
        Task<bool> Update(LocalIncident report);
        Task<List<LocalIncident>> GetAllAsync();
        Task<List<LocalIncident>> GetAllAnonAsync();
        LocalIncident GetReportById(string id);
        LocalIncident GetReportByMobileId(string id);

        Task<OutputDto> Upload(LocalIncident report);

        Task<List<GeoCoordinate>> GetLocations();
        Task<bool> DeleteAsync(string reportId);
        Task<bool> Create(LocalIncident report, string eventName = "IncidentCreated");
    }
}