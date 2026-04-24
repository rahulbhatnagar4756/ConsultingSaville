using Library.Base.Models;
using Library.Base.Models.Employees;
using Library.Base.Models.Employees.BusinessStructure;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.DataAccess.Admin
{
    internal static class IconsDataAccess
    {
        //[Admin].[spIcons]
        public static async Task<IEnumerable<NameOnlyModel>> GetIcons(ISqlDataAccess sql, CompanyUsersLoggedInModel companyUsers) =>
          await sql.LoadDataAsync<NameOnlyModel, dynamic>("[Admin].[spIcons]", companyUsers);

    }
}
