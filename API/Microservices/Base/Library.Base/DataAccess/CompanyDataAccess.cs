using Library.Base.Models;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.DataAccess
{
    internal static class CompanyDataAccess
    {

        public static async Task<IEnumerable<CompanyBasicModel>> GetCompanyByUUID(ISqlDataAccess _sql, string CompanyUUID, string UsersUUIDLoggedIn) =>
            await _sql.LoadDataAsync<CompanyBasicModel, dynamic>("[base].[spCompanyByUUID]", new { CompanyUUID, UsersUUIDLoggedIn });



    }
}
