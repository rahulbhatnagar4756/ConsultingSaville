

CREATE PROCEDURE [base].[spEmployeeBusinessUnitTypesSave]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
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

    SELECT @Companyid = [Recordid] 
    FROM [dbo].[Companies] c 
    WHERE c.[UUID] = @CompanyUUID 

    IF ISNULL(@Companyid, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
        RETURN 0;
    END

    IF @UUID IS NULL
    BEGIN
        SELECT @UUID = but.[UUID]
        FROM [dbo].[EmployeeBusinessUnitTypes] but
        WHERE but.[Name] = @Name
            AND but.[Companyid] = @Companyid; 
    END
    --to the id for Icon
     
    SELECT TOP(1) @IconsId = i.[Id]
    FROM [admin].[Icons] i
    WHERE i.[Name] = @Icon
     

    IF @UUID IS NULL
    BEGIN
 
        INSERT INTO [dbo].[EmployeeBusinessUnitTypes] ( [Companyid]
                                                      , [Name]
                                                      , [IconsId]
                                                      , [IconColor])
         VALUES ( @Companyid
                , @Name
                , @IconsId
                , @IconColor)
                
        SET @EmployeeBusinessUnitTypesid = SCOPE_IDENTITY();

        SELECT @UUID = [UUID]
        FROM [dbo].[EmployeeBusinessUnitTypes] bu
        WHERE bu.[Id] = @EmployeeBusinessUnitTypesid

        SELECT @UUID [UUID], 1 [isValid], 'Successfully added Business Unit Type' [Message]
        RETURN 1;
    END
    ELSE
    BEGIN

        UPDATE [dbo].[EmployeeBusinessUnitTypes]
        SET [Name] = @Name
        , [IconsId] = @IconsId
        , [IconColor] = @IconColor
        , [isDeleted] = 0
        WHERE [UUID] = @UUID;

        SELECT @UUID [UUID], 1 [isValid], 'Business Unit Type successfully updated' [Message]
        RETURN 1;

    END




END
