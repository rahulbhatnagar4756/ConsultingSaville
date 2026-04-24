using Dapper;
using Library.Assess.Models;
using Library.Assess.Models.Projects;
using Library.Database.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Assess.DataAccess.Projects
{
    internal class AssessmentUsersDataAccess
    {
        /// <summary>
        /// Get tests assigned to a candidate
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="companyUUID">Company UUID</param>
        /// <param name="usersUUID">User UUID</param>
        /// <param name="languageUUID">Language UUID (optional)</param>
        /// <returns>List of candidate test assignments</returns>
        public static async Task<List<CandidateTestModel>?> GetCandidateTests(
            ISqlDataAccess sql, 
            BasicGetWithProjectModel model)
        {

            var result = await sql.LoadDataAsync<CandidateTestModel, dynamic>(
                "[Assess].[spAssessmentUsers_Tests]", 
                model);

            return result?.ToList();
        }
    }
}
