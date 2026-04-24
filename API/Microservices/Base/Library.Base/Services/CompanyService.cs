using Library.Base.DataAccess;
using Library.Base.Models;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Services
{
    public interface ICompanyService
    {
        Task<CompanyBasicModel> GetCompanyByUUID(string companyUUID, string usersUUIDLoggedin);
    }

    public class CompanyService : ICompanyService
    {
        private readonly ISqlDataAccess _sql;

        public CompanyService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public async Task<CompanyBasicModel> GetCompanyByUUID(string companyUUID, string usersUUIDLoggedin)
        {
            var data = await CompanyDataAccess.GetCompanyByUUID(_sql, companyUUID, usersUUIDLoggedin);
            if (data == null) return null;
            return data.FirstOrDefault();
        }

    }
}
