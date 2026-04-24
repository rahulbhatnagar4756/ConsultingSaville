using Library.API.Goals.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Mappers
{
    public static class KPIMapper
    {

        public static async Task<ContractKPISaveModel> ToContractKPISaveModel(this ContractKPIModel contractKPI)
        {
            return new ContractKPISaveModel()
            {
                UUID = contractKPI.UUID,
                KPAUUID = contractKPI.KPAUUID ?? string.Empty,
                UsersUUID = contractKPI.UsersUUID ?? string.Empty,
                Name = contractKPI.Name ?? string.Empty,
                Description = contractKPI.Description,
                StatusUUID = contractKPI.StatusUUID,
                ToleranceSetsUUID = contractKPI.ToleranceSetsUUID,
                DateStart = contractKPI.DateStart,
                DateEnd = contractKPI.DateEnd,
                Target = contractKPI.Target ?? 100m,
                Weights = contractKPI.Weight ?? 100m,
                CompanyUUID = contractKPI.CompanyUUID ?? string.Empty,
                isScoreProcessing = contractKPI.isScoreProcessing 
            };
        }

    }
}
