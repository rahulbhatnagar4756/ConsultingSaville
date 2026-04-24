
-- =============================================
-- Stored Procedure: Save Enterprise Structure Weight
-- Description: Creates or updates an enterprise structure weight
-- =============================================
CREATE PROCEDURE [Goals].[spEnterpriseStructureWeights_Save]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200) = NULL,
    @EnterpriseStructureTypesUUID NVARCHAR(200),
    @EmployeeLevelsUUID NVARCHAR(200),
    @Weight DECIMAL(18,4)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @EnterpriseStructureWeightId INT
    , @EnterpriseStructureTypesId INT
    , @EmployeeLevelsId INT
    , @NewUUID NVARCHAR(200)
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidEdit INT = 7
    , @AuditLogsid BIGINT
    , @isUpdate BIT = 0

    SELECT @CompanyId = [recordID] 
    FROM [Base].[dbo].[Companies] 
    WHERE [UUID] = @CompanyUUID;
    
    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
        RETURN 0;
    END

    SELECT TOP(1) @UsersId = [Id]
    FROM [Base].[Users]
    WHERE [UUID] = @UsersUUIDLoggedIn;

    IF ISNULL(@UsersId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid user not supplied' [Message]
        RETURN 0;
    END

    -- Get EnterpriseStructureType ID and verify company ownership
    SELECT @EnterpriseStructureTypesId = [Id]
    FROM [Goals].[EnterpriseStructureTypes]
    WHERE [UUID] = @EnterpriseStructureTypesUUID 
      AND [Companyid] = @CompanyId
      AND [isDeleted] = 0;

    IF ISNULL(@EnterpriseStructureTypesId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Invalid enterprise structure type or access denied' [Message]
        RETURN 0;
    END

    -- Get EmployeeLevel ID
    SELECT @EmployeeLevelsId = [Id]
    FROM [Base].[EmployeeLevels]
    WHERE [UUID] = @EmployeeLevelsUUID 
      AND [isDeleted] = 0;

    IF ISNULL(@EmployeeLevelsId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Invalid employee level' [Message]
        RETURN 0;
    END

    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidEdit;

    IF @isHasAccess = 1
    BEGIN
        IF @UUID IS NULL OR @UUID = ''
        BEGIN
            -- Check for duplicate combination
            IF EXISTS (
                SELECT 1 FROM [Goals].[EnterpriseStructureWeights] esw
                INNER JOIN [Goals].[EnterpriseStructureTypes] est ON est.Id = esw.EnterpriseStructureTypesid
                WHERE esw.EnterpriseStructureTypesid = @EnterpriseStructureTypesId 
                  AND esw.EmployeeLevelsid = @EmployeeLevelsId
                  AND esw.isDeleted = 0
                  AND est.Companyid = @CompanyId
            )
            BEGIN
                SELECT NULL [UUID], 0 [isValid], 'A weight already exists for this Enterprise Structure Type and Employee Level combination' [Message]
                RETURN 0;
            END

           
            INSERT INTO [Goals].[EnterpriseStructureWeights] ([EnterpriseStructureTypesid], [EmployeeLevelsid], [Weight], [isDeleted])
            VALUES (@EnterpriseStructureTypesId, @EmployeeLevelsId, @Weight, 0);
        
            SET @EnterpriseStructureWeightId = SCOPE_IDENTITY();

            -- Log creation
            EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, 
                                                  @UsersId, 
                                                  'EnterpriseStructureWeights', 
                                                  @EnterpriseStructureWeightId, 
                                                  NULL, 
                                                  @AuditLogsid OUTPUT
        
            SELECT [UUID]
            , 1 [isValid]
            , 'Enterprise structure weight created successfully' as Message
            FROM [Goals].[EnterpriseStructureWeights]
            WHERE [Id] = @EnterpriseStructureWeightId; 
        END
        ELSE
        BEGIN
            -- Capture current values for comparison
            DECLARE @OldEnterpriseStructureTypesId INT = NULL
            , @OldEmployeeLevelsId INT = NULL
            , @OldWeight DECIMAL(18,4) = NULL

            -- Verify the weight belongs to the company before updating
            IF NOT EXISTS (
                SELECT 1 
                FROM [Goals].[EnterpriseStructureWeights] esw
                INNER JOIN [Goals].[EnterpriseStructureTypes] est ON esw.[EnterpriseStructureTypesid] = est.[Id]
                WHERE esw.[UUID] = @UUID 
                  AND est.[Companyid] = @CompanyId
                  AND esw.[isDeleted] = 0
            )
            BEGIN
                SELECT NULL [UUID], 0 [isValid], 'Enterprise structure weight not found or access denied' [Message]
                RETURN 0;
            END

            -- Check for duplicate combination (excluding current record)
            IF EXISTS (
                SELECT 1 
                FROM [Goals].[EnterpriseStructureWeights] esw
                INNER JOIN [Goals].[EnterpriseStructureTypes] est ON est.Id = esw.EnterpriseStructureTypesid
                WHERE esw.EnterpriseStructureTypesid = @EnterpriseStructureTypesId 
                  AND esw.EmployeeLevelsid = @EmployeeLevelsId
                  AND esw.UUID != @UUID
                  AND esw.isDeleted = 0
                  AND est.Companyid = @CompanyId 
            )
            BEGIN
                SELECT NULL [UUID], 0 [isValid], 'A weight already exists for this Enterprise Structure Type and Employee Level combination' [Message]
                RETURN 0;
            END

            -- Get current values and check for changes
            SELECT @EnterpriseStructureWeightId = [Id]
            , @OldEnterpriseStructureTypesId = [EnterpriseStructureTypesid]
            , @OldEmployeeLevelsId = [EmployeeLevelsid]
            , @OldWeight = [Weight]
            , @isUpdate = CASE WHEN [EnterpriseStructureTypesid] != @EnterpriseStructureTypesId OR
                                    [EmployeeLevelsid] != @EmployeeLevelsId OR
                                    [Weight] != @Weight THEN 1 ELSE 0 END
            FROM [Goals].[EnterpriseStructureWeights]
            WHERE [UUID] = @UUID;

            IF @isUpdate = 1
            BEGIN
                -- Update existing record
                UPDATE [Goals].[EnterpriseStructureWeights]
                SET [EnterpriseStructureTypesid] = @EnterpriseStructureTypesId,
                    [EmployeeLevelsid] = @EmployeeLevelsId,
                    [Weight] = @Weight
                WHERE [UUID] = @UUID;

                -- Log the changes
                EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                      @UsersId, 
                                                      'EnterpriseStructureWeights', 
                                                      @EnterpriseStructureWeightId, 
                                                      NULL, 
                                                      @AuditLogsid OUTPUT

                -- Save only the values that have changed
                IF @OldEnterpriseStructureTypesId != @EnterpriseStructureTypesId
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'EnterpriseStructureTypesid', @OldEnterpriseStructureTypesId

                IF @OldEmployeeLevelsId != @EmployeeLevelsId
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'EmployeeLevelsid', @OldEmployeeLevelsId

                IF @OldWeight != @Weight
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Weight', @OldWeight
            END
        
            SELECT @UUID [UUID]
            , 1 as [isValid]
            , 'Enterprise structure weight updated successfully' [Message];
        END
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END

