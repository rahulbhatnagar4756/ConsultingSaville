using Library.Database.DAL;
using Library.TalentManagement.Models.Box9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TalentManagement.DataAccess.Box9;

internal static class Box9DataAccess
{
    public static async Task<IEnumerable<TalentManagementStatisticModel>?> Statistics(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedIn, string Json) =>
        await sql.LoadDataAsync<TalentManagementStatisticModel, dynamic>("[TM].[spBox9_Statistics_Filter_Json]", new { CompanyUUID = companyUUID, UsersUUIDLoggedIn = usersUUIDLoggedIn, Json });

    public static async Task<TalentManagementUsersStatisticModel?> StatisticsUser(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedIn, string usersUUID) =>
        await sql.LoadFirstDataAsync<TalentManagementUsersStatisticModel, dynamic>("[TM].[spBox9_Statistics_User]", new { CompanyUUID = companyUUID, UsersUUIDLoggedIn = usersUUIDLoggedIn, UsersUUID = usersUUID });


    public static async Task<Box9ResultsModel?> Box9ResultStatisticsUser(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedIn, string usersUUID) =>
        await sql.LoadFirstDataAsync<Box9ResultsModel, dynamic>("[Assess].[spBox9Results]", new { CompanyUUID = companyUUID, UsersUUIDLoggedIn = usersUUIDLoggedIn, UsersUUID = usersUUID });

    /// <summary>
    /// gets the employees information from the IDP and Succession Planning AND user companies tables 
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="companyUUID"></param>
    /// <param name="usersUUIDLoggedIn"></param>
    /// <param name="usersUUID"></param>
    /// <returns></returns>
    public static async Task<TalentManagementUserInformationModel?> TalentManagementUserInformation(ISqlDataAccess sql, string companyUUID, string usersUUIDLoggedIn, string usersUUID) =>
       await sql.LoadFirstDataAsync<TalentManagementUserInformationModel, dynamic>("[TM].[spTalentManagementUserInformation]", new { CompanyUUID = companyUUID, UsersUUIDLoggedIn = usersUUIDLoggedIn, UsersUUID = usersUUID });

}
