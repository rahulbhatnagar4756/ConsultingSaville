using Library.Security.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library.Security.Services
{
    public interface ISecurityService
    {
        Task<LoggedInUserInformation?> GetUserInformation();
    }

    public class SecurityService : ISecurityService
    {
        private readonly AuthenticationStateProvider? _authenticationStateProvider;

        public SecurityService(AuthenticationStateProvider? authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<Models.LoggedInUserInformation?> GetUserInformation()
        {
            if (_authenticationStateProvider == null) return null;
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (state == null || state.User == null || state.User.Claims == null || state.User.Claims.Count() == 0) return null;

            Models.LoggedInUserInformation UserInformation = new();

            GetDefaultUserInformation(state.User, UserInformation);
            GetDefaultUserInformationRoles(state.User, UserInformation);

            return UserInformation;
        }


        private void GetDefaultUserInformation(ClaimsPrincipal state, Models.LoggedInUserInformation? userInformation) {
            if(userInformation == null && state == null) return;
            userInformation.UsersUUID = state.Claims.FirstOrDefault(c => c.Type == "Usersid").Value;
            userInformation.CompanyUUID = state.Claims.FirstOrDefault(c => c.Type == "Companyid").Value;
        }

        private void GetDefaultUserInformationRoles(ClaimsPrincipal state, Models.LoggedInUserInformation? userInformation) {
            if (userInformation == null && state == null) return;
            List<Models.LoggedInUserInformationRoles> roles = new();

            if (state.Identity == null || !state.Identity.IsAuthenticated) return;
            foreach (string role in state.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList())
                roles.Add(new() { Name = role });

            if(roles.Count == 0) return;
            userInformation.Roles = roles;
        }

    }
}
