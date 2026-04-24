using Library.Database.DAL;
using Library.Frog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.DataAccess
{
    internal static class FrogElementsDataAccess
    {

        public static async Task<IEnumerable<Models.FrogElementModel>?> GetElements(ISqlDataAccess sql, Models.FormSetsAccessModel access) =>
            await sql.LoadDataAsync<Models.FrogElementModel, Models.FormSetsAccessModel>("[Frog].[spFrogElementsid]", access);

        public static async Task<IEnumerable<ResultModel>> Save(ISqlDataAccess sql, FrogElementSaveModel elementResult) =>
            await sql.LoadDataAsync<Models.ResultModel, dynamic>("[Frog].[spFrogElements_Save]", elementResult);

        public static async Task<IEnumerable<ResultModel>> Save(ISqlDataAccess sql, FrogElementSaveLogModel elementResult) =>
            await sql.LoadDataAsync<Models.ResultModel, dynamic>("[Frog].[spFrogElements_Save_log]", elementResult);

    }
}
