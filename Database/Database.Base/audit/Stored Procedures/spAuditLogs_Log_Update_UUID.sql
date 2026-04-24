

CREATE PROCEDURE [audit].[spAuditLogs_Log_Update_UUID]
    @CompanyUUID NVARCHAR(200),
    @UsersUUID NVARCHAR(200),
    @AuditLogTableName NVARCHAR(255), 
    @Tableid NVARCHAR(255), 
    @AuditLogsid BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AuditLogActionsid INT = 2
    , @UserIPAddress NVARCHAR(45) = NULL
    , @Companyid INT
    , @Usersid INT
    
    SELECT @Usersid = [Recordid]
    FROM [dbo].[users]
    WHERE [UUID] = @UsersUUID 

    SELECT @Companyid = [Recordid]
    FROM [dbo].[Companies]
    WHERE [UUID] = @CompanyUUID 

    EXEC [audit].[spAuditLogs_Save] @Companyid,
                                   @Usersid,
                                   @AuditLogTableName, 
                                   @Tableid,
                                   @AuditLogActionsid,
                                   @UserIPAddress,
                                   @AuditLogsid OUTPUT
END
