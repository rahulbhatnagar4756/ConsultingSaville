
-- TemplateBusinessUnitDepartmentPositions - Delete (Soft Delete)
CREATE   PROCEDURE [Goals].[spTemplateBusinessUnitDepartmentPositions_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @PositionId INT
    , @TemplateId INT
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidDelete INT = 8
    , @AuditLogsid BIGINT
    , @TemplatesUUID NVARCHAR(200)

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

    -- Check Access Rights
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidDelete;

    IF @isHasAccess = 1
    BEGIN
        -- Get position details and verify it exists
        SELECT @PositionId = tbdp.[Id]
        , @TemplateId = tbdp.[Templatesid]
        , @TemplatesUUID = t.[UUID]
        FROM [Goals].[TemplateBusinessUnitDepartmentPositions] tbdp
        INNER JOIN [Goals].[Templates] t ON t.[Id] = tbdp.[Templatesid]
        WHERE tbdp.[UUID] = @UUID 
          AND t.[Companyid] = @CompanyId
          AND tbdp.[isDeleted] = 0
          AND t.[isDeleted] = 0;

        IF @PositionId IS NULL
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Template position not found or already deleted' [Message]
            RETURN 0;
        END

        

        -- Perform soft delete
        UPDATE [Goals].[TemplateBusinessUnitDepartmentPositions]
        SET [isDeleted] = 1
        WHERE [UUID] = @UUID 
          AND [isDeleted] = 0;

        -- Log deletion
        EXEC [audit].[spAuditLogs_Log_Delete] @CompanyId, 
                                              @UsersId, 
                                              'TemplateBusinessUnitDepartmentPositions', 
                                              @PositionId, 
                                              @TemplatesUUID, 
                                              @AuditLogsid OUTPUT

        SELECT @UUID [UUID], 1 [isValid], 'Template position deleted successfully' [Message];
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END

