using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.EnterpriseStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.EnterpriseStructures;

internal static class EnterpriseStructureWeightsDataAccess
{
    /// <summary>
    /// Get all enterprise structure weights for a company
    /// </summary>
    /// <param name="sql">SQL data access interface</param>
    /// <param name="basic">Basic model with company and user information</param>
    /// <returns>Collection of enterprise structure weights</returns>
    public static async Task<IEnumerable<EnterpriseStructureWeightsModel>?> GetAll(ISqlDataAccess sql, BasicModel basic)
    {
        var result = await sql.LoadDataAsync<EnterpriseStructureWeightsModel, dynamic>("[Goals].[spEnterpriseStructureWeights]", basic);
        return result;
    }

    /// <summary>
    /// Save enterprise structure weight (create or update)
    /// </summary>
    /// <param name="sql">SQL data access interface</param>
    /// <param name="saveModel">Enterprise structure weight save model</param>
    /// <returns>Result of the save operation</returns>
    public static async Task<IEnumerable<ResultsModel>?> Save(ISqlDataAccess sql, EnterpriseStructureWeightsSaveModel saveModel)
    {
        var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spEnterpriseStructureWeights_Save]", saveModel);
        return result;
    }

    /// <summary>
    /// Delete enterprise structure weight (soft delete)
    /// </summary>
    /// <param name="sql">SQL data access interface</param>
    /// <param name="basic">Basic model with company and user information</param>
    /// <param name="uuid">UUID of the enterprise structure weight to delete</param>
    /// <returns>Result of the delete operation</returns>
    public static async Task<IEnumerable<ResultsModel>?> Delete(ISqlDataAccess sql, BasicModel basic, string uuid)
    {
        var parameters = new
        {
            basic.CompanyUUID,
            basic.UsersUUIDLoggedIn,
            UUID = uuid
        };

        var result = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spEnterpriseStructureWeights_Delete]", parameters);
        return result;
    }
     
}
