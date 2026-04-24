 CREATE PROCEDURE [audit].[spAuditLogDetails_Save]
     @AuditLogsid BIGINT,
     @FieldName NVARCHAR(255),
     @OldValue NVARCHAR(MAX)
 AS
 BEGIN

    SET NOCOUNT ON;

    DECLARE @AuditLogFieldNamesid INT = NULL

    SELECT TOP(1) @AuditLogFieldNamesid = n.Id 
    FROM [audit].[AuditLogFieldNames] n
    WHERE n.[Name] = @FieldName
    ORDER BY [Id]

    IF ISNULL(@AuditLogFieldNamesid,0) = 0
    BEGIN

        INSERT INTO [audit].[AuditLogFieldNames] ([Name])
        VALUES( @FieldName );

        SET @AuditLogFieldNamesid = SCOPE_IDENTITY();
    END

    INSERT INTO [audit].[AuditLogDetails] ([AuditLogsid]
                                         , [AuditLogFieldNamesid]
                                         , [OldValue])
    VALUES ( @AuditLogsid
           , @AuditLogFieldNamesid
           , @OldValue)
 
END