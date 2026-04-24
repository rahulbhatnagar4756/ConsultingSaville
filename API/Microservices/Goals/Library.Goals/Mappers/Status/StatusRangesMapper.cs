using Library.Goals.Models;
using Library.Goals.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Mappers.Status
{
    public static class StatusRangesMapper
    {
        public static StatusRangesSaveModel StatusRangesSaveModel(
            this StatusRangesModel model,
            BasicModel? basic,
            string statusUUID)
        {
            return new StatusRangesSaveModel
            {
                CompanyUUID = basic?.CompanyUUID,
                UsersUUIDLoggedIn = basic?.UsersUUIDLoggedIn,
                UUID = model.UUID,
                StatusUUID = statusUUID,
                Name = model.Name ?? string.Empty,
                Icons = model.Icons,
                Color = model.Color,
                RangeStart = model.RangeStart,
                isNegative = model.isNegative
            };
        }
    }
}
