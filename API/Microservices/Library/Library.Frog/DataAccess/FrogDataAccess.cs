using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.DataAccess
{
    internal static class FrogDataAccess
    {
        internal static async Task<IEnumerable<Models.ResultModel>> ChangeStatus(ISqlDataAccess sql, Models.FrogChangeStatusModel elementResult) =>
            await sql.LoadDataAsync<Models.ResultModel, dynamic>("[dbo].[spFrog_Update_Status]", elementResult);
         
    }
}
