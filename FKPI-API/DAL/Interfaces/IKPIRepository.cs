using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FKPI_API.Models;

namespace FKPI_API.DAL
{
    public interface IKPIRepository
    {
        Task<IEnumerable<KPI>> GetAsync();
        Task<KPI> FindAsync(int kpiId);
        void AddKPI(KPI kpi);
        void UpdateKPI(KPI kpi);
        void DeleteKPI(KPI kpi);
        List<Dtos.KPIValueDto> Evaluate(KPI kpi);
    }
}