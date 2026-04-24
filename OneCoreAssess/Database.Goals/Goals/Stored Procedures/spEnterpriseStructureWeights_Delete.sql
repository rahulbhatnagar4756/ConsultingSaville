-- =============================================
-- Stored Procedure: Delete Enterprise Structure Weight
-- Description: Soft deletes an enterprise structure weight
-- =============================================
CREATE PROCEDURE [Goals].[spEnterpriseStructureWeights_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @Usersid INT
    , @SecurityRoleAccessidDelete INT = 8
    , @isHasAccess BIT = 0
    , @AuditLogsid BIGINT
    , @EnterpriseStructureWeightsid INT 

    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidDelete;

    IF @isHasAccess = 1
    BEGIN
        SELECT @CompanyId = [recordID] 
        FROM [Base].[dbo].[Companies] 
        WHERE [UUID] = @CompanyUUID;
    
        IF ISNULL(@CompanyId, 0) = 0
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
            RETURN 0;
        END 

        -- Verify the enterprise structure weight belongs to the company through EnterpriseStructureTypes
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

        UPDATE [Goals].[EnterpriseStructureWeights]
        SET [isDeleted] = 1
        WHERE [UUID] = @UUID;
    
        SELECT @EnterpriseStructureWeightsid = [Id]
        FROM [Goals].[EnterpriseStructureWeights]
        WHERE [UUID] = @UUID

        EXEC [audit].[spAuditLogs_Log_Delete_UUID] @CompanyUUID
                                                 , @UsersUUIDLoggedIn
                                                 , 'EnterpriseStructureWeights'
                                                 , @EnterpriseStructureWeightsid 
                                                 , @AuditLogsid OUTPUT

        SELECT @UUID [UUID]
        , 1 [isValid] 
        , 'Enterprise structure weight deleted successfully' [Message]
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END

