using Library.Goals.Models;
using Library.Goals.Models.EnterpriseStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Mappers.EnterpriseStructures
{
    public static class EnterpriseStructureWeightsMapper
    {
        public static EnterpriseStructureWeightsSaveModel EnterpriseStructureWeightsSaveModel(
            this EnterpriseStructureWeightsModel model,
            BasicModel? basic)
        {
            return new EnterpriseStructureWeightsSaveModel
            {
                CompanyUUID = basic?.CompanyUUID,
                UsersUUIDLoggedIn = basic?.UsersUUIDLoggedIn,
                UUID = model.UUID,
                EnterpriseStructureTypesUUID = model.EnterpriseStructureTypesUUID,
                EmployeeLevelsUUID = model.EmployeeLevelsUUID,
                Weight = model.Weight
            };
        }
    }
}
