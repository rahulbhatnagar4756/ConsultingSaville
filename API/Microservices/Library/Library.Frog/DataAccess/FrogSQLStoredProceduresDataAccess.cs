using Library.Database.DAL;
using Library.Frog.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.DataAccess
{
    internal static class FrogSQLStoredProceduresDataAccess
    {

        public static async Task<IEnumerable<FrogSQLStoredProceduresDTO>> GetSQLStoredProceduresAndParameters(ISqlDataAccess sql, FrogSQLStoredProceduresParameterTypesDTO Parameters) =>
            await sql.LoadDataAsync<FrogSQLStoredProceduresDTO, dynamic>("[Frog].[spFrogSQLStoredProcedures]", Parameters);

    }
}
