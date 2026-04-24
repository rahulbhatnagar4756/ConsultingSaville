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

public interface ITemplatesService
{
    Task<List<ResultsModel>?> CloneLastContractPeriod(string templatesUUID, List<UUIDsModel>? KPA);
    Task<List<ResultsModel>?> Delete(BasicModel basic, string templatesUUID);
    Task<List<TemplatesModel>?> GetTemplates(BasicModel Basic);
    Task<List<ResultsModel>?> Save(TemplateContractModel templateContract);

    /// <summary>
    /// Retrieves all users from the service.
    /// </summary>
    /// <returns>
    /// A list of <see cref="UsersModel"/> objects, or null if no users are found 
    /// or the request cannot be completed.
    /// </returns>
    Task<List<UsersModel>?> GetAllUsers(UserTemplatesModel userTemplates);


    /// <summary>
    /// Saves or updates a user template.
    /// </summary>
    /// <param name="userTemplates">The user template data to save.</param>
    /// <returns>
    /// A list of <see cref="ResultsModel"/> objects indicating the outcome of the operation,
    /// or null if the request cannot be completed.
    /// </returns>
    Task<List<ResultsModel>?> UserTemplateSave(UserTemplatesModel userTemplates);

    /// <summary>
    /// Deletes a user template asynchronously.
    /// </summary>
    /// <param name="userTemplates">The user template to delete.</param>
    /// <returns>
    /// A list of <see cref="ResultsModel"/> representing the result of the deletion,
    /// or <c>null</c> if the deletion failed or no data was returned.
    /// </returns>
    Task<List<ResultsModel>?> UserTemplateDelete(UserTemplatesModel userTemplates);

    /// <summary>
    /// Retrieves a list of users linked to a specific user template asynchronously.
    /// </summary>
    /// <param name="userTemplates">The user template to check for linked users.</param>
    /// <returns>
    /// A list of <see cref="UsersModel"/> representing the users linked to the template,
    /// or <c>null</c> if no users are linked.
    /// </returns>
    Task<List<UsersModel>?> LinkedUserTemplate(UserTemplatesModel userTemplates);

}

public class TemplatesService : ITemplatesService
{
    private ISqlDataAccess _sql;

    public TemplatesService(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public async Task<List<TemplatesModel>?> GetTemplates(BasicModel Basic)
    {
        var Data = await TemplatesDataAccess.Information(_sql, Basic);
        if (Data == null) return null;
        return Data;
    }

    public async Task<List<ResultsModel>?> Save(TemplateContractModel templateContract)
    {
        var Data = await TemplatesDataAccess.Save(_sql, templateContract);
        if (Data == null) return null;
        return Data.ToList();
    }

    /// <summary>
    /// Retrieves all users from the database using <see cref="TemplatesDataAccess"/>.
    /// </summary>
    /// <returns>
    /// A list of <see cref="UsersModel"/> objects. Returns an empty list if no users are found.
    /// </returns>
    public async Task<List<UsersModel>?> GetAllUsers(UserTemplatesModel userTemplates)
    {
        var Data = await TemplatesDataAccess.GetAllUsers(_sql, userTemplates);
        if (Data == null) return null;
        return Data.ToList();
    }


    /// <summary>
    /// Saves or updates a user template using <see cref="TemplatesDataAccess"/>.
    /// </summary>
    /// <param name="userTemplates">The user template data to save.</param>
    /// <returns>
    /// A list of <see cref="ResultsModel"/> objects representing the outcome of the save operation, 
    /// or null if the operation returns no data.
    /// </returns>
    public async Task<List<ResultsModel>?> UserTemplateSave(UserTemplatesModel userTemplates)
    {
        var Data = await TemplatesDataAccess.UserTemplateSave(_sql, userTemplates);
        if (Data == null) return null;
        return Data.ToList();
    }

    /// <summary>
    /// Deletes a user template from the database asynchronously and returns the result as a list.
    /// </summary>
    /// <param name="userTemplates">The user template to delete.</param>
    /// <returns>
    /// A list of <see cref="ResultsModel"/> representing the deletion result, 
    /// or <c>null</c> if no data was returned.
    /// </returns>
    public async Task<List<ResultsModel>?> UserTemplateDelete(UserTemplatesModel userTemplates)
    {
        var Data = await TemplatesDataAccess.UserTemplateDelete(_sql, userTemplates);
        if (Data == null) return null;
        return Data.ToList();
    }

    public async Task<List<ResultsModel>?> Delete(BasicModel basic, string templatesUUID)
    {
       var Data = await TemplatesDataAccess.Delete(_sql, basic, templatesUUID);
         if (Data == null) return null;
         return Data.ToList();
    }

    public async Task<List<ResultsModel>?> CloneLastContractPeriod(string templatesUUID, List<UUIDsModel>? KPA)
    {
        if (KPA == null || KPA.Count == 0) return null;

        //throw an error for now
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves a list of users linked to a specific user template asynchronously.
    /// </summary>
    /// <param name="userTemplates">The user template to check for linked users.</param>
    /// <returns>
    /// A list of <see cref="UsersModel"/> representing the linked users,
    /// or <c>null</c> if no users are linked to the template.
    /// </returns>
    public async Task<List<UsersModel>?> LinkedUserTemplate(UserTemplatesModel userTemplates)
    {
        var Data = await TemplatesDataAccess.LinkedUserTemplate(_sql, userTemplates);
        if (Data == null) return null;
        return Data.ToList();
    }
}