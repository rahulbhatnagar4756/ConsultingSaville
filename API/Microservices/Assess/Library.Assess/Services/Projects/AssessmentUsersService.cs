using Library.Assess.DataAccess.Projects;
using Library.Assess.Models;
using Library.Assess.Models.Projects;
using Library.Database.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Assess.Services.Projects
{
    public interface IAssessmentUsersService
    {
        Task<List<CandidateTestModel>?> GetCandidateTestsAsync(BasicGetWithProjectModel model);
    }

    public class AssessmentUsersService : IAssessmentUsersService
    {
        private readonly ISqlDataAccess _sql;

        public AssessmentUsersService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public async Task<List<CandidateTestModel>?> GetCandidateTestsAsync(
            BasicGetWithProjectModel model)
        {
            if (string.IsNullOrEmpty(model.CompanyUUID) || string.IsNullOrEmpty(model.UsersUUID))
                return null;
            try
            {
                return await AssessmentUsersDataAccess.GetCandidateTests(_sql, model);
            }
            catch
            {
                return null;
            }
        }
    }
}
