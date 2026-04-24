using Library.Base.Models.Users;
using Library.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base.DataAccess.Users
{
    internal static class UsersDataDataAccess
    {

        /// <summary>
        /// Get the Extra Employee information by UUID
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="companyUUID"></param>
        /// <param name="usersUUIDLoggedin"></param>
        /// <param name="usersUUID"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<UserDataBasicModel>?> GetUserEmployeeInformationByUsersUUID(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedin, string usersUUID) =>
            await sql.LoadDataAsync<UserDataBasicModel, dynamic>("[base].[spUsers_Employee_ByUUID]", new { CompanyUUID = companyUUID, UsersUUIDLoggedin = usersUUIDLoggedin, UsersUUID = usersUUID });
    }
}
