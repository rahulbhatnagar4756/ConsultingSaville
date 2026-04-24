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

namespace Library.Goals.Services
{
    public interface IEnterpriseStructureWeightsService
    {
        Task<List<EnterpriseStructureWeightsModel>?> GetEnterpriseStructureWeights(BasicModel basic);
        Task<ResultsModel?> SaveEnterpriseStructureWeight(BasicModel basic, EnterpriseStructureWeightsModel enterpriseStructureWeight);
        Task<ResultsModel?> DeleteEnterpriseStructureWeight(BasicModel basic, string uuid);
    }

    public class EnterpriseStructureWeightsService : IEnterpriseStructureWeightsService
    {
        private readonly ISqlDataAccess _sql;

        public EnterpriseStructureWeightsService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        /// <summary>
        /// Get all enterprise structure weights for a company
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <returns>List of enterprise structure weights</returns>
        public async Task<List<EnterpriseStructureWeightsModel>?> GetEnterpriseStructureWeights(BasicModel basic)
        {
            var data = await EnterpriseStructureWeightsDataAccess.GetAll(_sql, basic);
            var weights = data?.ToList();
            
            if (weights != null)
            {
                // Calculate normalized weights for display
                weights = await CalculateNormalizedWeights(basic, weights) ?? weights;
            }
            
            return weights;
        }

        

        /// <summary>
        /// Save enterprise structure weight (create or update)
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="enterpriseStructureWeight">Enterprise structure weight model</param>
        /// <returns>Result of the save operation</returns>
        public async Task<ResultsModel?> SaveEnterpriseStructureWeight(BasicModel basic, EnterpriseStructureWeightsModel enterpriseStructureWeight)
        {
            if (enterpriseStructureWeight == null || 
                string.IsNullOrWhiteSpace(enterpriseStructureWeight.EnterpriseStructureTypesUUID) ||
                string.IsNullOrWhiteSpace(enterpriseStructureWeight.EmployeeLevelsUUID))
                return new ResultsModel { UUID = null, isValid = false, Message = "Invalid enterprise structure weight data." };

            if (enterpriseStructureWeight.Weight < 0)
                return new ResultsModel { UUID = null, isValid = false, Message = "Weight must be greater than or equal to 0." };

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

        /// <summary>
        /// Calculate normalized weights using the formula: N1/(N1+N2+N3...)
        /// </summary>
        /// <param name="basic">Basic model containing company and user information</param>
        /// <param name="weights">List of weights to normalize</param>
        /// <returns>List of weights with normalized values</returns>
        public async Task<List<EnterpriseStructureWeightsModel>?> CalculateNormalizedWeights(BasicModel basic, List<EnterpriseStructureWeightsModel> weights)
        {
            if (weights == null || !weights.Any()) return weights;

            // Group by employee level to calculate normalized weights per level
            var groupedByLevel = weights.GroupBy(w => w.EmployeeLevelsUUID);

            foreach (var levelGroup in groupedByLevel)
            {
                var levelWeights = levelGroup.ToList();
                var totalWeight = levelWeights.Sum(w => w.Weight);

                if (totalWeight > 0)
                {
                    foreach (var weight in levelWeights)
                    {
                        // Apply the formula: N1/(N1+N2+N3...)
                        weight.Weight = weight.Weight / totalWeight;
                    }
                }
            }

            return weights;
        }
 
    }
}