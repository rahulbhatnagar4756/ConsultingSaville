using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess
{
    internal class ContractsDataAccess
    {
        public static async Task<IEnumerable<ResultsModel>?> CloneContract(ISqlDataAccess sql, string CompanyUUID, string UsersUUIDLoggedIn, ContractsCloneModel contractsClone)
        {
            //convert contractsClone to json
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(contractsClone);
            var data = await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spContracts_Clone_Json]", new { CompanyUUID, UsersUUIDLoggedIn, json });
            return data;
        }



    }
}
