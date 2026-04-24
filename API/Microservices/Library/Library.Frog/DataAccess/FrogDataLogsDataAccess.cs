using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.DataAccess;

internal static class FrogDataLogsDataAccess
{

    public static async Task<IEnumerable<Models.FrogDataLogsModel>?> GetReadinessToMoveToNextRole(ISqlDataAccess sql, Models.BaseModel access) =>
        await sql.LoadDataAsync<Models.FrogDataLogsModel, dynamic>("[Frog].[spFrogDataLogs_ReadinessToMoveToNextRole]", access);

}
