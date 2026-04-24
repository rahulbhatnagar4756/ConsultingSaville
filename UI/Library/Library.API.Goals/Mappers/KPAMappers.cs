using Library.API.Goals.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Mappers
{
    public static class KPAMappers
    { 
        public static ContractKPASaveModel ToContractKPASaveModel(this ContractKPAModel contractKPA)
        {
            return new ContractKPASaveModel()
            {
                UUID = contractKPA.UUID,
                CompanyUUID = string.Empty,
                UsersUUIDLoggedIn = string.Empty,
                UsersUUID = null,
                ContractPillarsUUID = contractKPA.ContractPillarsUUID ?? string.Empty,
                StatusUUID = contractKPA.StatusUUID ?? string.Empty,
                RatingPeriodsUUID = contractKPA.RatingPeriodsUUID,
                Name = contractKPA.Name ?? string.Empty,
                Description = contractKPA.Description,
                Weights = contractKPA.Weight ?? 100.0m,
                isActive = contractKPA.isActive

            };
        }
    }
}
