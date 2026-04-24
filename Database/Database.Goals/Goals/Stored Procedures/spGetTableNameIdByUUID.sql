USE [Goals]
GO
/****** Object:  StoredProcedure [admin].[spGetTableNameIdByUUID]    Script Date: 08/12/2025 11:37:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [admin].[spGetTableNameIdByUUID]
    @TableTargetUUID NVARCHAR(200),
    @IsAdminOrManager BIT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @TableNameId INT = NULL,
        @FoundInTable NVARCHAR(50) = NULL;

    -------------------------------------------------------------------
    -- CASE 1: If user is Admin OR Manager → search ONLY Users table
    -------------------------------------------------------------------
    IF (@IsAdminOrManager = 1)
BEGIN
    -- Return TableNameId of USERS directly, without checking Users table
    SELECT @TableNameId = Id
    FROM [Goals].[Goals].[TableName]
    WHERE Name = 'Users';

    SET @FoundInTable = 'Users';

    SELECT 
        @TableNameId AS TableNameId,
        @FoundInTable AS FoundInTable;

    RETURN;
END



    -------------------------------------------------------------------
    -- CASE 2: Normal User → FIRST check KPI → KPA → Users
    -------------------------------------------------------------------

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
