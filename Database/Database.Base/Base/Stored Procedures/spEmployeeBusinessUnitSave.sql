

CREATE PROCEDURE [base].[spEmployeeBusinessUnitSave]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @EmployeeBusinessUnitTypesUUID NVARCHAR(200) = null,
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
    , @EmployeeBusinessUnitTypesid INT = NULL
    , @EmployeeBusinessUnitsid INT = NULL

    SELECT @Companyid = [Recordid] 
    FROM [dbo].[Companies] c 
    WHERE c.[UUID] = @CompanyUUID 

    IF ISNULL(@Companyid, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
        RETURN 0;
    END

    
    SELECT @EmployeeBusinessUnitTypesid = but.[Id]
    FROM [dbo].[EmployeeBusinessUnitTypes] but
    WHERE but.[UUID] = @EmployeeBusinessUnitTypesUUID;

    --to the id for Icon
     
        SELECT TOP(1) @IconsId = i.[Id]
        FROM [admin].[Icons] i
        WHERE i.[Name] = @Icon
     

    IF @UUID IS NULL
    BEGIN
 
        INSERT INTO [dbo].[EmployeeBusinessUnits] ([Companyid]
               , [EmployeeBusinessUnitTypesid]
               , [Name]
               , [Description]
               , [IconId]
               , [IconColor]
                )
         VALUES
               ( @Companyid
               , @EmployeeBusinessUnitTypesid 
               , @Name
               , @Description
               , @IconsId
               , @IconColor
                )
                
        SET @EmployeeBusinessUnitsid = SCOPE_IDENTITY();

        SELECT @UUID = [UUID]
        FROM [dbo].[EmployeeBusinessUnits] bu
        WHERE bu.[Recordid] = @EmployeeBusinessUnitsid

        SELECT @UUID [UUID], 1 [isValid], 'Successfully added Business Unit' [Message]
        RETURN 1;
    END
    ELSE
    BEGIN

        UPDATE [dbo].[EmployeeBusinessUnits]
        SET [EmployeeBusinessUnitTypesid] = @EmployeeBusinessUnitTypesid
        , [Name] = @Name
        , [Description] = @Description
        , [IconId] = @IconsId
        , [IconColor] = @IconColor
        , [isDeleted] = 0
        WHERE [UUID] = @UUID;

        SELECT @UUID [UUID], 1 [isValid], 'Business Unit successfully updated' [Message]
        RETURN 1;

    END




END
