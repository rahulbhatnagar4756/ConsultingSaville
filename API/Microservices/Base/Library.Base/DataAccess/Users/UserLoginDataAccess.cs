using Dapper;
using Library.Base.Models;
using Library.Base.Models.Users;
using Library.Database.DAL;
using System.Data;
using System.Text.Json;

namespace Library.Base.DataAccess.Users
{
    internal static class UserLoginDataAccess
    {
        /// <summary>
        /// Authenticates a user and returns login information including company access
        /// </summary>
        /// <param name="sql">SQL Data Access interface</param>
        /// <param name="companyUUID">Optional company UUID for specific company login</param>
        /// <param name="username">User's username</param>
        /// <param name="password">User's password (case-sensitive)</param>
        /// <returns>UserLoginResponseModel with authentication result</returns>
        public static async Task<UserLoginResponseModel> LoginAsync(ISqlDataAccess sql, string? companyUUID, string username, string password)
        {
            try
            {
                DynamicParameters parameters = new();
                parameters.Add("CompanyUUID", companyUUID, DbType.String, ParameterDirection.Input);
                parameters.Add("Username", username, DbType.String, ParameterDirection.Input);
                parameters.Add("Password", password, DbType.String, ParameterDirection.Input);
                parameters.Add("isSuccessful", null, DbType.Boolean, ParameterDirection.Output);
                parameters.Add("Message", null, DbType.String, ParameterDirection.Output, size: 500);

                // Execute stored procedure with output parameters
                var result = await sql.LoadDataJsonAsync<UserLoginResponseModel, DynamicParameters>("[secure].[spUsers_Login]", parameters);
                
                var ResultLogin = result.FirstOrDefault()??null;
                //var ResultLogin = await Library.Tools.Conversions.JsonConvert.DeserializeJsonAsync<UserLoginResponseModel>(ResultLoginJson);

                if (ResultLogin == null)
                {
                    return new UserLoginResponseModel
                    {
                        IsSuccessful = false,
                        Message = "Authentication failed"
                    };
                }

                ResultLogin.IsSuccessful = true;// result.OutputParams.Get<bool>("isSuccessful");
                ResultLogin.Message = "Successfully Logged in"; // result.OutputParams.Get<string>("Message") ?? "Login successful";

                return ResultLogin;
            }
            catch (Exception ex)
            {
                return new UserLoginResponseModel
                {
                    IsSuccessful = false,
                    Message = $"Authentication error: {ex.Message}"
                };
            }
        }
    }

 
}
