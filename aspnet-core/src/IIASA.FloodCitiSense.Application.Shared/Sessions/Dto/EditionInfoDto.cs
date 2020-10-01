using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.Sessions.Dto
{
    public class EditionInfoDto : EntityDto
    {
        public string DisplayName { get; set; }

        public int? TrialDayCount { get; set; }

        public decimal? MonthlyPrice { get; set; }

        public decimal? AnnualPrice { get; set; }

        public bool IsHighestEdition { get; set; }

        public bool IsFree { get; set; }
    }
}