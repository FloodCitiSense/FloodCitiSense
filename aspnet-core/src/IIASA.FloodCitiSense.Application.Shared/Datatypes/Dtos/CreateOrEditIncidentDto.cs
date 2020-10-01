
using Abp.Application.Services.Dto;
using System;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class CreateOrEditIncidentDto : AuditedEntityDto<int?>
    {

        public string FloodingType { get; set; }

        public string Comment { get; set; }

        public int SelectedLocationID { get; set; }

        public int GPSLocationID { get; set; }

        public int ReporterUserID { get; set; }

        public string PictureIDs { get; set; }

        public DateTime TimeCreated { get; set; }


    }
}