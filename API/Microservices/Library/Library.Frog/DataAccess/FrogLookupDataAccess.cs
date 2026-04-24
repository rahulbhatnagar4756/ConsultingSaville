using Library.Database.DAL;
using Library.Frog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.DataAccess
{
    internal static class FrogLookupDataAccess
    {
        internal static async Task<IEnumerable<FrogLookupModel>> GetLookups(ISqlDataAccess sql, int FrogLookupTypesid) =>
            await sql.LoadDataAsync<FrogLookupModel, dynamic>("[Frog].[spFrogLookups]", new { FrogLookupTypesid });
    }
}
