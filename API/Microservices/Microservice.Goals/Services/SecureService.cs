using System.Security.Claims;

namespace Microservice.Base.Services
{
    public interface ISecureService
    {
        (string CompanyUUID, string UsersUUIDLoggedIn) GetUserClaims(ClaimsPrincipal user);
    }

    public class SecureService : ISecureService
    {
        public (string CompanyUUID, string UsersUUIDLoggedIn) GetUserClaims(ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User claims cannot be null.");

            string companyUUID = user.FindFirst("Companyid")?.Value ?? string.Empty;
            string usersUUIDLoggedIn = user.FindFirst("Usersid")?.Value ?? string.Empty;

            return (companyUUID, usersUUIDLoggedIn);
        }
    }
}
