
CREATE PROCEDURE [audit].[spAuditLogs_Log_Insert]
    @Companyid INT,
    @Usersid INT,
    @AuditLogTableName NVARCHAR(255), 
    @Tableid NVARCHAR(255), 
    @UserIPAddress NVARCHAR(45) = NULL,
    @AuditLogsid BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AuditLogActionsid INT = 1

    EXEC [audit].[spAuditLogs_Save] @Companyid,
                                   @Usersid,
                                   @AuditLogTableName, 
                                   @Tableid,
                                   @AuditLogActionsid,
                                   @UserIPAddress,
                                   @AuditLogsid OUTPUT
END
