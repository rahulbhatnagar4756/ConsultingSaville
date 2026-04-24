using Library.Base.BusinessLogic.Logs;
using Library.Base.DataAccess.Users;
using Library.Base.Models.Users;
using Library.Database.DAL;
using Microsoft.Extensions.Configuration;

namespace Library.Base.Services
{
    public interface IUserLoginService
    {
        Task<UserLoginResponseModel> LoginAsync(string? companyUUID, string username, string password);
        Task<UserLoginTokenModel> LoginReturnTokenAsync(string? companyUUID, string username, string password);
    }

    public class UserLoginService : IUserLoginService
    {
        private readonly ISqlDataAccess _sql;
        private readonly IConfiguration _config;

        public UserLoginService(ISqlDataAccess sql, IConfiguration config)
        {
            _sql = sql;
            _config = config;
        }

        /// <summary>
        /// Authenticates a user with the provided credentials
        /// </summary>
        /// <param name="companyUUID">Optional company UUID for specific company login</param>
        /// <param name="username">User's username</param>
        /// <param name="password">User's password</param>
        /// <returns>Login response with authentication result</returns>
        public async Task<UserLoginResponseModel> LoginAsync(string? companyUUID, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return new UserLoginResponseModel
                {
                    IsSuccessful = false,
                    Message = "Username and password are required"
                };
            }

            try
            {
                var result = await UserLoginDataAccess.LoginAsync(_sql, companyUUID, username, password);
                return result;
            }
            catch (Exception ex)
            {
                return new UserLoginResponseModel
                {
                    IsSuccessful = false,
                    Message = "An unexpected error occurred during authentication"
                };
            }
        }

        public async Task<UserLoginTokenModel> LoginReturnTokenAsync(string? companyUUID, string username, string password)
        {
            var result = await LoginAsync(companyUUID, username, password);

            UserLoginTokenModel tokenModel = new()
            {
                IsSuccessful = result.IsSuccessful,
                Message = result.Message,
                Token = string.Empty 
            };

            if (!result.IsSuccessful) return tokenModel;

            string CompanyUUID = result.Company.FirstOrDefault()?.CompanyUUID ?? string.Empty;
            List<string> Roles = new();
            
            result.Company.FirstOrDefault()?.Roles.ForEach(r => Roles.Add(r.Roles));

            LoggingProcessor loggingProcessor = new(_config);
            var token = await loggingProcessor.GenerateLoginToken(
                CompanyUUID,
                result.UsersUUID!,
                Roles);

            tokenModel.Token = token;

            return tokenModel;
        }

    }
}