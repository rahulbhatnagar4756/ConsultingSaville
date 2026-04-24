
CREATE   PROCEDURE [admin].[spGetTableNameIdByUUID]
    @TableTargetUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @TableNameId INT = NULL,
        @FoundInTable NVARCHAR(50) = NULL;

    ----------------------------------------------------------
    -- 1. Check KPI table
    ----------------------------------------------------------
    IF EXISTS (
        SELECT 1 
        FROM [Goals].[Goals].[KPI]
        WHERE UUID = @TableTargetUUID
          AND isDeleted = 0
    )
    BEGIN
        SELECT @TableNameId = Id
        FROM [Goals].[Goals].[TableName]
        WHERE Name = 'KPI';

        SET @FoundInTable = 'KPI';

        SELECT 
            @TableNameId AS TableNameId,
            @FoundInTable AS FoundInTable;
        RETURN;
    END

    ----------------------------------------------------------
    -- 2. Check KPA table
    ----------------------------------------------------------
    IF EXISTS (
        SELECT 1 
        FROM [Goals].[Goals].[KPA]
        WHERE UUID = @TableTargetUUID
          AND isDeleted = 0
    )
    BEGIN
        SELECT @TableNameId = Id
        FROM [Goals].[Goals].[TableName]
        WHERE Name = 'KPA';

        SET @FoundInTable = 'KPA';

        SELECT 
            @TableNameId AS TableNameId,
            @FoundInTable AS FoundInTable;
        RETURN;
    END

    ----------------------------------------------------------
    -- 3. Not Found
    ----------------------------------------------------------
    SELECT 
        NULL AS TableNameId,
        'NotFound' AS FoundInTable;
END