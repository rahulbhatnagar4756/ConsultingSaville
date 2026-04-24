using Library.Goals.Models;
using Library.Goals.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Mappers.Status
{
    public static class StatusMapper
    {
        public static StatusSaveModel StatusSaveModel(
            this StatusModel model,
            BasicModel? basic)
        {
            return new StatusSaveModel
            {
                CompanyUUID = basic?.CompanyUUID,
                UsersUUIDLoggedIn = basic?.UsersUUIDLoggedIn,
                UUID = model.UUID,
                Name = model.Name,
                Description = model.Description,
                OrderVal = model.OrderVal
            };
        }
    }
}
