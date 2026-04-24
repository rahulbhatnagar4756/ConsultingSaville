using Library.Database.DAL;
using Library.Frog.DTO;
using Library.Frog.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Library.Frog.Helpers
{
    internal interface IFrogSQLStoredProceduresBuilder
    {
        Task<List<FrogLookupModel>?> GenerateData(FrogSQLStoredProceduresParameterTypesDTO parameters);
    }

    internal class FrogSQLStoredProceduresBuilder : IFrogSQLStoredProceduresBuilder
    {
        private ISqlDataAccess _sql;

        public FrogSQLStoredProceduresBuilder(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public async Task<List<FrogLookupModel>?> GenerateData(FrogSQLStoredProceduresParameterTypesDTO parameters)
        {
            var StoredProceduresAndParameters = await DataAccess.FrogSQLStoredProceduresDataAccess.GetSQLStoredProceduresAndParameters(_sql, parameters);
            if(StoredProceduresAndParameters == null) return null;
            var DefaultParameters = ProceduresToDynamic(StoredProceduresAndParameters.ToList());
            var Procedure = StoredProceduresAndParameters.FirstOrDefault()?.StoredProcedure?? null;
            if(Procedure == null) return null;
            return await _sql.LoadDataAsync<FrogLookupModel, dynamic>(Procedure, DefaultParameters);
        }


        /// <summary>
        /// Convert the list of stored procedures to a dynamic object
        /// </summary>
        /// <param name="reportGenerators"></param>
        /// <returns></returns>
        private dynamic ProceduresToDynamic(List<DTO.FrogSQLStoredProceduresDTO> reportGenerators)
        {
            dynamic obj = new ExpandoObject();
            var objDictionary = (IDictionary<string, object>)obj;

            // Populate the ExpandoObject with the properties from the list
            foreach (var item in reportGenerators.OrderBy(x => x.OrderVal))
            {
                objDictionary[item.Parameters] = item.DefaultValue;
            }

            return obj;
        }

    }
}
