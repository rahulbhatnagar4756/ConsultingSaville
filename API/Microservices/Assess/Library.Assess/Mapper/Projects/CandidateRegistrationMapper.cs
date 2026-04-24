using Dapper;
using Library.Assess.Models.Projects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Mapper.Projects;

internal static  class CandidateRegistrationMapper
{
    public static DynamicParameters ToDynamicParameters(this CandidateRegistrationModel model)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@CompanyUUID", model.CompanyUUID);
        parameters.Add("@ProjectsUUID", model.ProjectsUUID);
        parameters.Add("@UsersUUIDCandidate", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        parameters.Add("@IDNumber", model.IDNumber);
        parameters.Add("@FirstName", model.FirstName);
        parameters.Add("@LastName", model.LastName);
        parameters.Add("@Email", model.Email);
        parameters.Add("@Mobile", model.Mobile);
        parameters.Add("@Gender", model.Gender);
        parameters.Add("@Ethnicity", model.Ethnicity);
        parameters.Add("@DateOfBirth", model.DateOfBirth);
        parameters.Add("@isSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

        return parameters;
    }
}