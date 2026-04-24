using Library.Database.DAL;
using Library.Goals.DataAccess.Contracts;
using Library.Goals.Models;
using Library.Goals.Models.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services.Contracts;

public interface ITemplateBusinessUnitDepartmentPositionsService
{
    Task<List<TemplateBusinessUnitDepartmentPositionsModel>?> GetByTemplate(TemplateBusinessUnitDepartmentPositionsGetModel model);
    Task<List<ResultsModel>?> Save(TemplateBusinessUnitDepartmentPositionsSaveModel model);
    Task<List<ResultsModel>?> Delete(string uuid, string companyUUID, string usersUUIDLoggedIn);
}

public class TemplateBusinessUnitDepartmentPositionsService : ITemplateBusinessUnitDepartmentPositionsService
{
    private ISqlDataAccess _sql;

    public TemplateBusinessUnitDepartmentPositionsService(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public async Task<List<TemplateBusinessUnitDepartmentPositionsModel>?> GetByTemplate(TemplateBusinessUnitDepartmentPositionsGetModel model)
    {
        var Data = await TemplateBusinessUnitDepartmentPositionsDataAccess.GetByTemplate(_sql, model);
        if (Data == null) return null;
        return Data;
    }

    public async Task<List<ResultsModel>?> Save(TemplateBusinessUnitDepartmentPositionsSaveModel model)
    {
        var Data = await TemplateBusinessUnitDepartmentPositionsDataAccess.Save(_sql, model);
        if (Data == null) return null;
        return Data.ToList();
    }

    public async Task<List<ResultsModel>?> Delete(string uuid, string companyUUID, string usersUUIDLoggedIn)
    {
        var deleteModel = new
        {
            UUID = uuid,
            CompanyUUID = companyUUID,
            UsersUUIDLoggedIn = usersUUIDLoggedIn
        };

        var Data = await TemplateBusinessUnitDepartmentPositionsDataAccess.Delete(_sql, deleteModel);
        if (Data == null) return null;
        return Data.ToList();
    }
}