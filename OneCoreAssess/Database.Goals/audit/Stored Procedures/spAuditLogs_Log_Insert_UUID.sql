

CREATE PROCEDURE [audit].[spAuditLogs_Log_Insert_UUID]
    @CompanyUUID NVARCHAR(200),
    @UsersUUID NVARCHAR(200),
    @AuditLogTableName NVARCHAR(255), 
    @Tableid NVARCHAR(255), 
    @AuditLogsid BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AuditLogActionsid INT = 1,
    @Companyid INT,
    @Usersid INT,
    @UserIPAddress NVARCHAR(45) = NULL
    
    SELECT @Usersid = [Id]
    FROM [base].[users]
    WHERE [UUID] = @UsersUUID 

    SELECT @Companyid = [Recordid]
    FROM [base].[Companies]
    WHERE [UUID] = @CompanyUUID 

    EXEC [audit].[spAuditLogs_Save] @Companyid,
                                    @Usersid,
                                    @AuditLogTableName, 
                                    @Tableid,
                                    @AuditLogActionsid,
                                    @UserIPAddress,
                                    @AuditLogsid OUTPUT
END
