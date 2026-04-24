using Library.Base.DataAccess.Users;
using Library.Base.Models.Users;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Services
{
    public interface IUserDataService
    {
        Task<UserDataBasicModel?> GetUserEmployeeInformationByUsersUUID(string companyUUID, string usersUUIDLoggedin, string usersUUID);
    }

    public class UserDataService : IUserDataService
    {
        private readonly ISqlDataAccess _sql;

        public UserDataService(ISqlDataAccess sql)
        {
            _sql = sql;
        }


        public async Task<UserDataBasicModel?> GetUserEmployeeInformationByUsersUUID(string companyUUID, string usersUUIDLoggedin, string usersUUID)
        {
            var datas = await UsersDataDataAccess.GetUserEmployeeInformationByUsersUUID(_sql, companyUUID, usersUUIDLoggedin, usersUUID);
            if (datas == null || datas.Count() == 0) return null;
            return datas?.FirstOrDefault() ?? null;
        }
    }
}
