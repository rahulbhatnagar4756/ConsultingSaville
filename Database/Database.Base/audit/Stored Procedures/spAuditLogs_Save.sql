


 


CREATE PROCEDURE [audit].[spAuditLogs_Save]
    @Companyid INT,
    @Usersid INT,
    @AuditLogTableName NVARCHAR(255), 
    @Recordid INT,
    @AuditLogActionsid INT,
    @UserIPAddress NVARCHAR(45) = NULL,
    @AuditLogsid BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

     
    DECLARE @AuditLogTableNamesid INT;

    BEGIN TRY
        -- Check if the table name exists in [base].[AuditLogTableNames]
        SELECT TOP(1) @AuditLogTableNamesid = Id
        FROM [audit].[AuditLogTableNames]
        WHERE [Name] = @AuditLogTableName;

        -- If the table name does not exist, insert it and get the new ID
        IF @AuditLogTableNamesid IS NULL
        BEGIN
            INSERT INTO [audit].[AuditLogTableNames] ([Name])
            VALUES (@AuditLogTableName);

            SET @AuditLogTableNamesid = SCOPE_IDENTITY();  
        END;

        -- Now, insert the audit log entry using the retrieved or newly created @TableId
        INSERT INTO [audit].[AuditLogs] (
            [CompanyId],
            [UsersID],
            [AuditLogsTableNamesid], -- This column in [iag].[base].[AuditLogs] now holds the ID from [base].[AuditLogTableNames]
            [RecordID],
            [AuditLogActionsid],
            [UserIPAddress]
        )
        VALUES (
            @CompanyId,
            @UsersID,
            @AuditLogTableNamesid, -- Use the retrieved/generated TableId here
            @Recordid,
            @AuditLogActionsid,
            @UserIPAddress
        );

        -- Get the ID of the newly inserted AuditLog row
        SET @AuditLogsid = SCOPE_IDENTITY();

        -- Return the new ID
        RETURN 1;

    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(MAX) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH

    RETURN 0;
END;
