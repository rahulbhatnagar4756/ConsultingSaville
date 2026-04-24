using Library.Base.Models;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.DataAccess.Users
{
    internal static class UserRolesDataAccess
    {

        public static async Task<IEnumerable<RoleBasicModel>> GetUserRolesByUsersUUID(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedin, string usersUUID) =>
           await sql.LoadDataAsync<RoleBasicModel, dynamic>("[base].[spUserRolesByUUID]", new { CompanyUUID = companyUUID, UsersUUIDLoggedin = usersUUIDLoggedin, UsersUUID = usersUUID });
    }
}
