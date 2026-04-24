using Library.Goals.Models;
using Library.Goals.Models.Pillars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Mappers;

public static class PillarsMapper
{
    public static PillarsSaveModel ToPillarsSaveModel(
        this PillarsModel model,
        BasicModel? basic)
    {
        return new PillarsSaveModel
        {
            CompanyUUID = basic?.CompanyUUID,
            UsersUUIDLoggedIn = basic?.UsersUUIDLoggedIn,
            UUID = model.UUID,
            Name = model.Name ?? string.Empty,
            Description = model.Description,
            Icons = model.Icons,
            IconColor = model.IconColor,
            OrderVal = model.OrderVal
        };
    }
}
