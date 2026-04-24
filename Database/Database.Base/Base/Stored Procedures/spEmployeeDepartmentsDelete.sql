

CREATE PROCEDURE [base].[spEmployeeDepartmentsDelete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200) = null 
AS
BEGIN
    -- Insert or update logic here
    SET NOCOUNT ON;

    DECLARE @Companyid INT = 0
    , @EmployeeDepartmentssid INT = NULL
    , @SecurityRoleAccessidDelete INT = 6
    , @isHasAccess BIT = 0
    , @AuditLogsid BIGINT

    EXEC @isHasAccess = [secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidDelete;

    IF @isHasAccess = 1
    BEGIN
        SELECT @Companyid = [Recordid] 
        FROM [dbo].[Companies] c 
        WHERE c.[UUID] = @CompanyUUID 

        IF ISNULL(@Companyid, 0) = 0
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
            RETURN 0;
        END

        UPDATE [dbo].[EmployeeDepartments]
        SET [isDeleted] = 1
        WHERE [UUID] = @UUID;

        SELECT @EmployeeDepartmentssid = [Recordid]
        FROM [dbo].[EmployeeDepartments]
        WHERE [UUID] = @UUID;

        EXEC [audit].[spAuditLogs_Log_Delete_UUID] @CompanyUUID
                                                 , @UsersUUIDLoggedIn
                                                 , 'EmployeeDepartments'
                                                 , @EmployeeDepartmentssid 
                                                 , @AuditLogsid OUTPUT


        SELECT @UUID [UUID], 1 [isValid], 'Department successfully deleted' [Message]

        RETURN 1;
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'You do not have access for this function' [Message]
        RETURN 0;
    END

END
