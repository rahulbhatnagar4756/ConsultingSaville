using Library.Base.Models;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Services.Admin
{
    public interface IIconsService
    {
        Task<List<NameOnlyModel>?> GetIcons(CompanyUsersLoggedInModel companyUsers);
    }

    public class IconsService : IIconsService
    {
        private readonly ISqlDataAccess _sql;

        public IconsService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public async Task<List<NameOnlyModel>?> GetIcons(CompanyUsersLoggedInModel companyUsers)
        {
            var data = await DataAccess.Admin.IconsDataAccess.GetIcons(_sql, companyUsers);
            if (data == null || !data.Any()) return default;
            return data.ToList();
        }
    }
}
