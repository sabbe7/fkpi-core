using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FKPI_API.Dtos;
using FKPI_API.Models;
using FKPI_API.Core;
using Microsoft.EntityFrameworkCore;
using SqlKata.Execution;

namespace FKPI_API.DAL
{
    public class KPIRepository : IKPIRepository
    {
        private readonly ApplicationDbContext context;
        private readonly QueryFactory db;
        private readonly QueryParser queryParser;

        public KPIRepository(ApplicationDbContext context, QueryFactory db, QueryParser queryParser)
        {
            this.context = context;
            this.db = db;
            this.queryParser = queryParser;
        }

        public async Task<IEnumerable<KPI>> GetAsync()
        {
            return await context.KPI
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<KPI> FindAsync(int kpiId)
        {
            return await context.KPI.FirstOrDefaultAsync(x => x.KPIId == kpiId);
        }

        public void AddKPI(KPI kpi)
        {
            context.KPI.Add(kpi);
        }

        public void DeleteKPI(KPI kpi)
        {
            context.KPI.Remove(kpi);
        }

        public void UpdateKPI(KPI kpi)
        {
            context.KPI.Update(kpi);
        }

        /// <summary>
        /// Get values of a KPI
        /// </summary>
        /// <param name="kpi">A KPI to evaluate</param>        
        /// <returns>Returns the list of KPI values per year</returns>
        public List<KPIValueDto> Evaluate(KPI kpi)
        {
            var retVal = new List<KPIValueDto>();

            var tokens = kpi.Formula.Split(';').ToList();

            var query = queryParser.Parse(tokens);

            retVal = db.FromQuery(query).Get<KPIValueDto>().ToList();

            return retVal;
        }
    }
}