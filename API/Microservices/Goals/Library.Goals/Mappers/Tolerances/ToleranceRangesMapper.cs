using Library.Goals.Models;
using Library.Goals.Models.Tolerances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Mappers.Tolerances;

public static class ToleranceRangesMapper
{
    public static ToleranceRangesSaveModel ToleranceRangesSaveModel(
        this ToleranceRangesModel model,
        BasicModel? basic)
    {
        return new ToleranceRangesSaveModel
        {
            CompanyUUID = basic?.CompanyUUID,
            UsersUUIDLoggedIn = basic?.UsersUUIDLoggedIn,
            UUID = model.UUID,
            ToleranceSetsUUID = model.ToleranceSetsUUID,
            RangeStart = model.RangeStart,
            Score = model.Score
        };
    }
}
