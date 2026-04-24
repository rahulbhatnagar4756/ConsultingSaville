
CREATE   PROCEDURE [Goals].[spKPI_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @KPIUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @KPIId INT
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidDelete INT = 8 -- Assuming 8 is delete permission
    , @AuditLogsid BIGINT

    -- Get Company ID
    SELECT @CompanyId = [recordID] 
    FROM [Base].[dbo].[Companies] 
    WHERE [UUID] = @CompanyUUID;
    
    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
        RETURN 0;
    END

    -- Get User ID
    SELECT TOP(1) @UsersId = [Id]
    FROM [Base].[Users]
    WHERE [UUID] = @UsersUUIDLoggedIn;

    IF ISNULL(@UsersId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid user not supplied' [Message]
        RETURN 0;
    END

    -- Check if KPI exists and get its ID
    SELECT @KPIId = [Id]
    FROM [Goals].[KPI]
    WHERE [UUID] = @KPIUUID 
      AND [Companyid] = @CompanyId
      AND [isDeleted] = 0;

    IF @KPIId IS NULL
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'KPI not found or already deleted' [Message]
        RETURN 0;
    END

    -- Check Access Rights
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidDelete;

    IF @isHasAccess = 1
    BEGIN
        -- Check if KPI has dependencies (like tracking records, scores, etc.)
        DECLARE @HasDependencies INT = 0;
        
        -- Add dependency checks here as needed
        -- Example: Check if KPI has tracking records
        -- SELECT @HasDependencies = COUNT(*)
        -- FROM [Goals].[KPITracking]
        -- WHERE [KPIid] = @KPIId AND [isDeleted] = 0;

        IF @HasDependencies > 0
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Cannot delete KPI: It has associated records' [Message]
            RETURN 0;
        END

        -- Perform soft delete
        UPDATE [Goals].[KPI]
        SET [isDeleted] = 1
        WHERE [Id] = @KPIId;

        -- Log the deletion
        EXEC [audit].[spAuditLogs_Log_Delete] @CompanyId, 
                                              @UsersId, 
                                              'KPI', 
                                              @KPIId, 
                                              NULL, 
                                              @AuditLogsid OUTPUT

        SELECT @KPIUUID [UUID], 1 [isValid], 'KPI deleted successfully' [Message];
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END