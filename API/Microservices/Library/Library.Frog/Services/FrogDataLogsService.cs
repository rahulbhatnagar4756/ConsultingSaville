using Library.Database.DAL;
using Library.Frog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.Services
{
    public interface IFrogDataLogsService
    {
        Task<List<FrogDataLogsModel>?> GetReadinessToMoveToNextRole(BaseModel access);
    }

    public class FrogDataLogsService : IFrogDataLogsService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public FrogDataLogsService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<Library.Frog.Models.FrogDataLogsModel>?> GetReadinessToMoveToNextRole(Library.Frog.Models.BaseModel access) {
            var Data = await Library.Frog.DataAccess.FrogDataLogsDataAccess.GetReadinessToMoveToNextRole(_sqlDataAccess, access);
            return Data?.ToList() ?? null;
        }
    }
}
