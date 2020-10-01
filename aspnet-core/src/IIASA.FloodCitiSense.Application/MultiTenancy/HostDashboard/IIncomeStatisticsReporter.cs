using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IIASA.FloodCitiSense.MultiTenancy.HostDashboard.Dto;

namespace IIASA.FloodCitiSense.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}