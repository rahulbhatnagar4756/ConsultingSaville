using Library.Base.DataAccess.Users;
using Library.Base.Models;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.Services
{
    public interface IUserRolesService
    {
        Task<List<RoleBasicModel>?> GetRolesByUsersUUID(string companyUUID, string usersUUIDLoggedin, string usersUUID);
    }

    public class UserRolesService : IUserRolesService
    {
        private readonly ISqlDataAccess _sql;

        public UserRolesService(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public async Task<List<RoleBasicModel>?> GetRolesByUsersUUID(string companyUUID, string usersUUIDLoggedin, string usersUUID)
        {
            var data = await UserRolesDataAccess.GetUserRolesByUsersUUID(_sql, companyUUID, usersUUIDLoggedin, usersUUID);
            if (data == null || data.Count() == 0) return null;
            return data.ToList();
        }

    }
}
