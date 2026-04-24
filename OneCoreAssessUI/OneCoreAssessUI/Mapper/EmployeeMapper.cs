namespace OneCoreAssessUI.Mapper
{
    public static class EmployeeMapper
    {

        public static Library.API.Goals.Models.BASE.EmployeeBasicModel MapToEmployeeBasicModelFromGoals(this Library.API.Base.Models.Employees.EmployeesModel employee)
        {
            return new Library.API.Goals.Models.BASE.EmployeeBasicModel()
            {
                UUID = employee.UsersUUID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                IDNumber = employee.IDNumber,
                EmployeeNumber = employee.EmployeeNumber,
                Email = employee.Email,
                Mobile = employee.Mobile,
                Gender = employee.Gender,
                Ethnicity = employee.Race,
                DateOfBirth = employee.DateOfBirth,
                UsersImageURL = employee.Image
            };
        }


    }
}
