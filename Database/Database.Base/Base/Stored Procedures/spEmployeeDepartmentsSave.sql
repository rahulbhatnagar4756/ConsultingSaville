

CREATE PROCEDURE [base].[spEmployeeDepartmentsSave]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @EmployeeBusinessUnitsUUID NVARCHAR(200) = null,
    @UUID NVARCHAR(200) = null,
    @Name NVARCHAR(255),
    @Description NVARCHAR(500) = null,
    @IconsId INT = null,
    @IconColor NVARCHAR(30) = null,
    @Icon NVARCHAR(200) = null 
AS
BEGIN
    -- Insert or update logic here
    SET NOCOUNT ON;

    DECLARE @Companyid INT = 0
    , @EmployeeBusinessUnitsid INT = NULL

    SELECT @Companyid = [Recordid] 
    FROM [dbo].[Companies] c 
    WHERE c.[UUID] = @CompanyUUID 

    IF ISNULL(@Companyid, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
        RETURN 0;
    END

    
    SELECT @EmployeeBusinessUnitsid = but.[Recordid]
    FROM [dbo].[EmployeeBusinessUnits] but
    WHERE but.[UUID] = @EmployeeBusinessUnitsUUID;

    --to the id for Icon
     
        SELECT TOP(1) @IconsId = i.[Id]
        FROM [admin].[Icons] i
        WHERE i.[Name] = @Icon
     

    IF @UUID IS NULL
    BEGIN
 
        INSERT INTO [dbo].[EmployeeDepartments] ([Companyid]
               , [EmployeeBusinessUnitsid]
               , [Name]
               , [Description]
               , [IconId]
               , [IconColor]
                )
         VALUES
               ( @Companyid
               , @EmployeeBusinessUnitsid 
               , @Name
               , @Description
               , @IconsId
               , @IconColor
                )
                
        SET @EmployeeBusinessUnitsid = SCOPE_IDENTITY();

        SELECT @UUID = [UUID]
        FROM [dbo].[EmployeeBusinessUnits] bu
        WHERE bu.[Recordid] = @EmployeeBusinessUnitsid

        SELECT @UUID [UUID], 1 [isValid], 'Successfully added Department' [Message]
        RETURN 1;
    END
    ELSE
    BEGIN

        UPDATE [dbo].[EmployeeDepartments]
        SET [EmployeeBusinessUnitsid] = @EmployeeBusinessUnitsid
        , [Name] = @Name
        , [Description] = @Description
        , [IconId] = @IconsId
        , [IconColor] = @IconColor
        , [isDeleted] = 0
        WHERE [UUID] = @UUID;

        SELECT @UUID [UUID], 1 [isValid], 'Department successfully updated' [Message]
        RETURN 1;

    END




END
