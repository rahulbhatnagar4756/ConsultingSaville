
-- TemplateBusinessUnitDepartmentPositions - Save (Insert/Update)
CREATE   PROCEDURE [Goals].[spTemplateBusinessUnitDepartmentPositions_Save]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200) = NULL,
    @TemplatesUUID NVARCHAR(200),
    @EmployeeBusinessUnitsUUID NVARCHAR(200),
    @EmployeeDepartmentsUUID NVARCHAR(200) = NULL,
    @EmployeeJobsUUID NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @PositionId INT
    , @TemplateId INT
    , @BusinessUnitId INT
    , @DepartmentId INT = NULL
    , @JobId INT = NULL
    , @NewUUID NVARCHAR(200)
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidEdit INT = 7
    , @AuditLogsid BIGINT
    , @isUpdate BIT = 0

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
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidEdit;

    IF @isHasAccess = 1
    BEGIN
        -- Get Template ID
        SELECT @TemplateId = [Id]
        FROM [Goals].[Templates]
        WHERE [UUID] = @TemplatesUUID 
          AND [Companyid] = @CompanyId
          AND [isDeleted] = 0;

        IF @TemplateId IS NULL
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Valid template not found' [Message]
            RETURN 0;
        END

        -- Get Business Unit ID
        SELECT @BusinessUnitId = [Id]
        FROM [Base].[vwEmployeeBusinessUnits]
        WHERE [UUID] = @EmployeeBusinessUnitsUUID
          AND [Companyid] = @CompanyId
          AND [isDeleted] = 0;

        IF @BusinessUnitId IS NULL
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Valid business unit not found' [Message]
            RETURN 0;
        END

        -- Get Department ID (if provided)
        IF @EmployeeDepartmentsUUID IS NOT NULL AND @EmployeeDepartmentsUUID != ''
        BEGIN
            SELECT @DepartmentId = [Id]
            FROM [Base].[vwEmployeeDepartments]
            WHERE [UUID] = @EmployeeDepartmentsUUID
              AND [Companyid] = @CompanyId
              AND [isDeleted] = 0;

            IF @DepartmentId IS NULL
            BEGIN
                SELECT NULL [UUID], 0 [isValid], 'Valid department not found' [Message]
                RETURN 0;
            END
        END

        -- Get Job ID (if provided)
        IF @EmployeeJobsUUID IS NOT NULL AND @EmployeeJobsUUID != ''
        BEGIN
            SELECT @JobId = [Id]
            FROM [Base].[vwEmployeeJobs]
            WHERE [UUID] = @EmployeeJobsUUID
              AND [Companyid] = @CompanyId
              AND [isDeleted] = 0;

            IF @JobId IS NULL
            BEGIN
                SELECT NULL [UUID], 0 [isValid], 'Valid job not found' [Message]
                RETURN 0;
            END
        END

        -- Check for duplicate combination
        DECLARE @ExistingPositionId INT = NULL;
        SELECT @ExistingPositionId = [Id]
        FROM [Goals].[TemplateBusinessUnitDepartmentPositions]
        WHERE [Templatesid] = @TemplateId
          AND [EmployeeBusinessUnitsid] = @BusinessUnitId
          AND ISNULL([EmployeeDepartmentsid], 0) = ISNULL(@DepartmentId, 0)
          AND ISNULL([EmployeeJobsid], 0) = ISNULL(@JobId, 0)
          AND [isDeleted] = 0
          AND (@UUID IS NULL OR [UUID] != @UUID);

        IF @ExistingPositionId IS NOT NULL
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'This position combination already exists for this template' [Message]
            RETURN 0;
        END

        IF @UUID IS NULL OR @UUID = ''
        BEGIN
        
            -- Insert new record
            INSERT INTO [Goals].[TemplateBusinessUnitDepartmentPositions] 
            ([Templatesid], [EmployeeBusinessUnitsid], [EmployeeDepartmentsid], [EmployeeJobsid])
            VALUES (@TemplateId, @BusinessUnitId, @DepartmentId, @JobId);
        
            SET @PositionId = SCOPE_IDENTITY();

            -- Log creation
            EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, 
                                                  @UsersId, 
                                                  'TemplateBusinessUnitDepartmentPositions', 
                                                  @PositionId, 
                                                  @TemplatesUUID, 
                                                  @AuditLogsid OUTPUT
        
            SELECT [UUID]
            , 1 [isValid]
            , 'Template position created successfully' as Message
            FROM [Goals].[TemplateBusinessUnitDepartmentPositions]
            WHERE [Id] = @PositionId; 

        END
        ELSE
        BEGIN
            -- Get existing record for comparison
            DECLARE @OldBusinessUnitId INT = NULL
            , @OldDepartmentId INT = NULL
            , @OldJobId INT = NULL

            SELECT @PositionId = tbdp.[Id]
            , @OldBusinessUnitId = tbdp.[EmployeeBusinessUnitsid]
            , @OldDepartmentId = tbdp.[EmployeeDepartmentsid]
            , @OldJobId = tbdp.[EmployeeJobsid]
            , @isUpdate = CASE WHEN tbdp.[EmployeeBusinessUnitsid] != @BusinessUnitId OR
                                    ISNULL(tbdp.[EmployeeDepartmentsid], 0) != ISNULL(@DepartmentId, 0) OR
                                    ISNULL(tbdp.[EmployeeJobsid], 0) != ISNULL(@JobId, 0) THEN 1 ELSE 0 END
            FROM [Goals].[TemplateBusinessUnitDepartmentPositions] tbdp
            INNER JOIN [Goals].[vwTemplates] t ON t.[Id] = tbdp.[Templatesid]
            WHERE tbdp.[UUID] = @UUID 
              AND t.[Companyid] = @CompanyId
              AND tbdp.[isDeleted] = 0
              AND t.[isDeleted] = 0;

            IF @PositionId IS NULL
            BEGIN
                SELECT NULL [UUID], 0 [isValid], 'Template position not found or access denied' [Message]
                RETURN 0;
            END

            IF @isUpdate = 1
            BEGIN
                -- Update the record
                UPDATE [Goals].[TemplateBusinessUnitDepartmentPositions]
                SET [EmployeeBusinessUnitsid] = @BusinessUnitId,
                    [EmployeeDepartmentsid] = @DepartmentId,
                    [EmployeeJobsid] = @JobId
                WHERE [UUID] = @UUID 
                  AND [isDeleted] = 0;

                -- Log the changes
                EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                      @UsersId, 
                                                      'TemplateBusinessUnitDepartmentPositions', 
                                                      @PositionId, 
                                                      @TemplatesUUID, 
                                                      @AuditLogsid OUTPUT

                -- Log changes (store old IDs for reference)
                IF @OldBusinessUnitId != @BusinessUnitId
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'EmployeeBusinessUnitsid', @OldBusinessUnitId 

                IF ISNULL(@OldDepartmentId, 0) != ISNULL(@DepartmentId, 0)
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'EmployeeDepartmentsid', @OldDepartmentId

                IF ISNULL(@OldJobId, 0) != ISNULL(@JobId, 0)
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'EmployeeJobsid', @OldJobId
            END
        
            SELECT @UUID [UUID], 1 as [isValid], 'Template position updated successfully' [Message];
        END
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END

