
-- Templates - Delete (Soft Delete)
CREATE   PROCEDURE [Goals].[spTemplates_Delete]
    @UUID NVARCHAR(200),
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @TemplateId INT
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidDelete INT = 8
    , @AuditLogsid BIGINT
    , @TemplateName NVARCHAR(500)
    , @TemplateCode NVARCHAR(500)

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
        -- Get template details for validation and auditing
        SELECT @TemplateId = [Id]
        , @TemplateName = [Name]
        , @TemplateCode = [Code]
        FROM [Goals].[Templates]
        WHERE [UUID] = @UUID 
          AND [Companyid] = @CompanyId
          AND [isDeleted] = 0;

        IF @TemplateId IS NULL
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Template not found or already deleted' [Message]
            RETURN 0;
        END

        -- Check if template is being used in contracts
        DECLARE @ContractCount INT = 0;
        SELECT @ContractCount = COUNT(*)
        FROM [Goals].[vwContracts_Templates] c 
        WHERE c.[TemplatesUUID] = @UUID
          AND c.[isDeleted] = 0;

        --IF @ContractCount > 0
        --BEGIN
        --    SELECT NULL [UUID], 0 [isValid], 'Cannot delete template as it is being used in active contracts' [Message]
        --    RETURN 0;
        --END

        DECLARE @TemplateBusinessUnitDepartmentPositionsUUID NVARCHAR(200);

        -- Check if template has associated business unit department positions
        DECLARE curPositions CURSOR FAST_FORWARD 
        FOR
        SELECT [UUID]
        FROM [Goals].[vwTemplateBusinessUnitDepartmentPositions] tbdp
        WHERE tbdp.[TemplatesUUID] = @UUID;

        -- Open cursor
        OPEN curPositions;

        -- Fetch first row
        FETCH NEXT FROM curPositions INTO @TemplateBusinessUnitDepartmentPositionsUUID;

        -- Loop through all rows
        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Soft delete associated positions
            EXEC [Goals].[spTemplateBusinessUnitDepartmentPositions_Delete] 
                 @CompanyUUID, 
                 @UsersUUIDLoggedIn, 
                 @TemplateBusinessUnitDepartmentPositionsUUID;

            -- Fetch next row
            FETCH NEXT FROM curPositions INTO @TemplateBusinessUnitDepartmentPositionsUUID;
        END

        -- Cleanup
        CLOSE curPositions;
        DEALLOCATE curPositions;
        
       

        -- Perform soft delete
        UPDATE [Goals].[Templates]
        SET [isDeleted] = 1
        WHERE [UUID] = @UUID 
          AND [Companyid] = @CompanyId
          AND [isDeleted] = 0;

        -- Log deletion
        EXEC [audit].[spAuditLogs_Log_Delete] @CompanyId, 
                                              @UsersId, 
                                              'Templates', 
                                              @TemplateId, 
                                              NULL, 
                                              @AuditLogsid OUTPUT


        SELECT @UUID [UUID], 1 [isValid], 'Template deleted successfully' [Message];
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END

