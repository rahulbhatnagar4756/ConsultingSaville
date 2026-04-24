using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.KPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess
{
    internal static class KPADataAccess
    {
        public static async Task<IEnumerable<ResultsModel>?> Save(ISqlDataAccess sql, KPASaveModel saveModel)
        {
            var data = await sql.LoadDataAsync<ResultsModel, KPASaveModel>("[Goals].[spKPA_Save]", saveModel);
            return data;
        }
    }
}
