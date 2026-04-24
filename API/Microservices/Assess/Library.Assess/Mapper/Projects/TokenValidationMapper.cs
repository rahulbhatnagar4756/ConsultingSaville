using Dapper;
using DocumentFormat.OpenXml.EMMA;
using Library.Assess.Models.Projects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Mapper.Projects
{
    internal static class TokenValidationMapper
    {
        public static DynamicParameters ToDynamicParameters(this TokenValidationModel model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyUUID", model.CompanyUUID);
            parameters.Add("@ProjectsUUID", model.ProjectsUUID);
            parameters.Add("@UsersUUID", model.UsersUUID); 
            parameters.Add("@isSuccessful", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Message", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

            return parameters;
        }
    }
}
