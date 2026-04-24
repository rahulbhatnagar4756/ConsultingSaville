using Library.Goals.Models;
using Library.Goals.Models.Tolerances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Mappers.Tolerances
{
    public static class ToleranceSetsMapper
    {
        public static ToleranceSetsSaveModel ToleranceSetsSaveModel(
            this ToleranceSetsModel model,
            BasicModel? basic)
        {

            return new ToleranceSetsSaveModel
            {
                CompanyUUID = basic?.CompanyUUID,
                UsersUUIDLoggedIn = basic?.UsersUUIDLoggedIn,
                UUID = model.UUID,
                Name = model.Name,
                Description = model.Description,
                MaxTotal = model.MaxTotal,
                OrderVal = model.OrderVal
            };
        }


    }
}
