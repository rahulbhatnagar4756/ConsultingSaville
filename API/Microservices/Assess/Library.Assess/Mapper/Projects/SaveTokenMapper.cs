using Dapper;
using Library.Assess.Models.Projects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Mapper.Projects
{
    internal static class SaveTokenMapper
    {
        public static DynamicParameters ToDynamicParameters(this SaveTokenModel model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyUUID", model.CompanyUUID);
            parameters.Add("@UsersUUIDLoggedIn", model.UsersUUIDLoggedIn);
            parameters.Add("@ProjectsUUID", model.ProjectsUUID);
            parameters.Add("@UsersUUID", model.UsersUUID);
            parameters.Add("@Token", model.Token);
            parameters.Add("@isSuccessful", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            return parameters;
        }
    }
}
