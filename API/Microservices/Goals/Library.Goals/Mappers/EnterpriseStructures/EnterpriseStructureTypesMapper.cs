using Library.Goals.Models;
using Library.Goals.Models.EnterpriseStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Mappers.EnterpriseStructures
{
    public static class EnterpriseStructureTypesMapper
    {
        public static EnterpriseStructureTypesSaveModel EnterpriseStructureTypesSaveModel(
            this EnterpriseStructureTypesModel model,
            BasicModel? basic)
        {
            return new EnterpriseStructureTypesSaveModel
            {
                CompanyUUID = basic?.CompanyUUID,
                UsersUUIDLoggedIn = basic?.UsersUUIDLoggedIn,
                UUID = model.UUID,
                Name = model.Name,
                Description = model.Description,
                Icons = model.Icons,
                Color = model.Color
            };
        }
    }
}
