using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.Contracts
{
    public static class TemplateBusinessUnitDepartmentPositionsDataAccess
    {
        public static async Task<IEnumerable<ResultsModel>?> Save(ISqlDataAccess sql, TemplateBusinessUnitDepartmentPositionsSaveModel model) =>
            await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spTemplateBusinessUnitDepartmentPositions_Save]", model);

        public static async Task<IEnumerable<ResultsModel>?> Delete(ISqlDataAccess sql, dynamic deleteModel) =>
            await sql.LoadDataAsync<ResultsModel, dynamic>("[Goals].[spTemplateBusinessUnitDepartmentPositions_Delete]", deleteModel);

        public static async Task<List<TemplateBusinessUnitDepartmentPositionsModel>?> GetByTemplate(ISqlDataAccess sql, TemplateBusinessUnitDepartmentPositionsGetModel model) =>
            await sql.LoadDataJsonAsync<TemplateBusinessUnitDepartmentPositionsModel, dynamic>("[Goals].[spTemplateBusinessUnitDepartmentPositions_GetByTemplate]", model);
    }
}