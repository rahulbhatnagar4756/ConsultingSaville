using Library.Database.DAL;
using Library.Goals.DataAccess.EnterpriseStructures;
using Library.Goals.Mappers.EnterpriseStructures;
using Library.Goals.Models;
using Library.Goals.Models.EnterpriseStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services;

public interface IEnterpriseStructuresService
{
    Task<List<EnterpriseStructureTypesModel>?> GetEnterpriseStructureTypes(BasicModel basic);
    Task<ResultsModel?> SaveEnterpriseStructureType(BasicModel basic, EnterpriseStructureTypesModel enterpriseStructureType);
    Task<ResultsModel?> DeleteEnterpriseStructureType(BasicModel basic, string uuid);
    Task<List<EnterpriseStructureWeightsModel>?> GetEnterpriseStructureWeights(BasicModel basic);
    Task<ResultsModel?> SaveEnterpriseStructureWeight(BasicModel basic, EnterpriseStructureWeightsModel enterpriseStructureWeight);
    Task<ResultsModel?> DeleteEnterpriseStructureWeight(BasicModel basic, string uuid);
}

public class EnterpriseStructuresService : IEnterpriseStructuresService
{
    private readonly ISqlDataAccess _sql;

    public EnterpriseStructuresService(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    /// <summary>
    /// Get all enterprise structure types for a company
    /// </summary>
    /// <param name="basic">Basic model containing company and user information</param>
    /// <returns>List of enterprise structure types</returns>
    public async Task<List<EnterpriseStructureTypesModel>?> GetEnterpriseStructureTypes(BasicModel basic)
    {
        var data = await EnterpriseStructureTypesDataAccess.GetAll(_sql, basic);
        return data?.ToList();
    }

    /// <summary>
    /// Save enterprise structure type (create or update)
    /// </summary>
    /// <param name="basic">Basic model containing company and user information</param>
    /// <param name="enterpriseStructureType">Enterprise structure type model</param>
    /// <returns>Result of the save operation</returns>
    public async Task<ResultsModel?> SaveEnterpriseStructureType(BasicModel basic, EnterpriseStructureTypesModel enterpriseStructureType)
    {
        if (enterpriseStructureType == null || string.IsNullOrWhiteSpace(enterpriseStructureType.Name))
            return new ResultsModel { UUID = null, isValid = false, Message = "Invalid enterprise structure type data." };

        var saveModel = enterpriseStructureType.EnterpriseStructureTypesSaveModel(basic);
        var result = await EnterpriseStructureTypesDataAccess.Save(_sql, saveModel);
        return result?.FirstOrDefault();
    }

    /// <summary>
    /// Delete enterprise structure type (soft delete)
    /// </summary>
    /// <param name="basic">Basic model containing company and user information</param>
    /// <param name="uuid">UUID of the enterprise structure type to delete</param>
    /// <returns>Result of the delete operation</returns>
    public async Task<ResultsModel?> DeleteEnterpriseStructureType(BasicModel basic, string uuid)
    {
        if (string.IsNullOrEmpty(uuid))
            return new ResultsModel { UUID = null, isValid = false, Message = "Invalid enterprise structure type selected." };

        var result = await EnterpriseStructureTypesDataAccess.Delete(_sql, basic, uuid);
        return result?.FirstOrDefault();
    }

    public async Task<List<EnterpriseStructureWeightsModel>?> GetEnterpriseStructureWeights(BasicModel basic)
    {
        var data = await EnterpriseStructureWeightsDataAccess.GetAll(_sql, basic);
        return data?.ToList();
    }



    /// <summary>
    /// Save enterprise structure weight (create or update)
    /// </summary>
    /// <param name="basic">Basic model containing company and user information</param>
    /// <param name="enterpriseStructureWeight">Enterprise structure weight model</param>
    /// <returns>Result of the save operation</returns>
    public async Task<ResultsModel?> SaveEnterpriseStructureWeight(BasicModel basic, EnterpriseStructureWeightsModel enterpriseStructureWeight)
    {
        var saveModel = enterpriseStructureWeight.EnterpriseStructureWeightsSaveModel(basic);
        var result = await EnterpriseStructureWeightsDataAccess.Save(_sql, saveModel);
        return result?.FirstOrDefault();
    }

    /// <summary>
    /// Delete enterprise structure weight (soft delete)
    /// </summary>
    /// <param name="basic">Basic model containing company and user information</param>
    /// <param name="uuid">UUID of the enterprise structure weight to delete</param>
    /// <returns>Result of the delete operation</returns>
    public async Task<ResultsModel?> DeleteEnterpriseStructureWeight(BasicModel basic, string uuid)
    {
        if (string.IsNullOrEmpty(uuid))
            return new ResultsModel { UUID = null, isValid = false, Message = "Invalid enterprise structure weight selected." };

        var result = await EnterpriseStructureWeightsDataAccess.Delete(_sql, basic, uuid);
        return result?.FirstOrDefault();
    }
}
