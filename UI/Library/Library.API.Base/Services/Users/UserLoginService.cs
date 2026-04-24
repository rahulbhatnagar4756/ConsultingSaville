using Library.API.Base.Models.Users;
using Library.API.Service;
using Microsoft.Extensions.Configuration;

namespace Library.API.Base.Services.Users
{
    public interface IUserLoginService
    {
        Task<UserLoginTokenModel> LoginReturnTokenAsync(string? companyUUID, string username, string password);
    }

    public class UserLoginService : IUserLoginService
    {
        private readonly IConfiguration _config;
        private readonly IAPIConnectService _apiConnect;
        private readonly string _urlBase;
        private readonly string _endPointLogin = "Users/Login/LoginToken";

        public UserLoginService(IConfiguration config, IAPIConnectService apiConnect)
        {
            _config = config;
            _apiConnect = apiConnect;
            _urlBase = _config.GetSection("API:Base:URL").Value ?? "";
        }

        public async Task<UserLoginTokenModel> LoginReturnTokenAsync(string? companyUUID, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return new UserLoginTokenModel
                {
                    IsSuccessful = false,
                    Message = "Username and password are required"
                };
            }

            try
            {
                var loginRequest = new UserLoginRequestModel
                {
                    Username = username,
                    Password = password,
                    CompanyUUID = companyUUID
                };

                var response = await _apiConnect.PostAsync<UserLoginTokenModel, UserLoginRequestModel>(
                    $"{_urlBase}{_endPointLogin}", loginRequest);

                return response ?? new UserLoginTokenModel
                {
                    IsSuccessful = false,
                    Message = "Failed to connect to login service"
                };
            }
            catch (Exception ex)
            {
                return new UserLoginTokenModel
                {
                    IsSuccessful = false,
                    Message = "An unexpected error occurred during login"
                };
            }
        }
    }
}