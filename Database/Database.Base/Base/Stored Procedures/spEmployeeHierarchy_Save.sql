CREATE PROCEDURE [Base].[spEmployeeHierarchy_Save]
    @EmployeeHierarchyUUID NVARCHAR(200) = NULL,
    @UsersUUID NVARCHAR(200),
    @UsersUUIDManager NVARCHAR(200),
    @Position NVARCHAR(255),
    @DateStartedAtPosition DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Usersid INT
    , @UsersidManager INT
    , @EmployeeJobsid INT
    , @EmployeeHierarchyid INT = 0
    , @isValid bit = 0;

    IF @DateStartedAtPosition IS NULL
        SET @DateStartedAtPosition = GETDATE();

    -- Check if UsersUUID and UsersUUIDManager exist in the users table
    SELECT @Usersid = [Recordid] 
    FROM [dbo].[users] 
    WHERE UUID = @UsersUUID;

    IF @Usersid IS NULL
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'User  does not exist' [Message]
        RETURN;
    END

    SELECT @UsersidManager = [Recordid] 
    FROM [dbo].[users] 
    WHERE UUID = @UsersUUIDManager;

    IF @UsersidManager IS NULL
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Manager does not exist' [Message]
        RETURN;
    END

    -- Get EmployeeJobsid (Assuming Position is stored in EmployeeJobs table)
    SELECT @EmployeeJobsid = [Recordid]
    FROM [dbo].[EmployeeJobs] 
    WHERE [UUID] = @Position;

    IF @EmployeeJobsid IS NULL
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Position does not exist' [Message]
        RETURN;
    END

    --make sure that epmployee manger and position does not already exit and is live

    BEGIN TRY

    SELECT @isValid = CASE WHEN COUNT(*) = 0 THEN 1 ELSE 0 END
    , @EmployeeHierarchyid = MAX([Recordid])
    , @EmployeeHierarchyUUID = MAX([UUID])
    FROM [EmployeeHierarchy] h
    WHERE (h.[Usersid] = @Usersid
    AND h.[UsersidManager] = @UsersidManager 
    AND h.[EmployeeJobsid] = @EmployeeJobsid 
    AND h.[isDeleted] = 0) OR h.[UUID] = ISNULL(@EmployeeHierarchyUUID,'0000-0000')

    IF @isValid = 1
    BEGIN
        -- If UUID is provided, update existing record
        IF @EmployeeHierarchyUUID IS NOT NULL
        BEGIN

            UPDATE [dbo].[EmployeeHierarchy]
            SET CreateDate = @DateStartedAtPosition
            WHERE UUID = @EmployeeHierarchyUUID;
        
            SELECT @EmployeeHierarchyUUID [UUID], 1 [isValid], 'Successfully updated' [Message]

            RETURN 1;
        END
        ELSE
        BEGIN

            -- Insert new record
            INSERT INTO [dbo].[EmployeeHierarchy] (EmployeeJobsid, Usersid, UsersidManager, CreateDate)
            VALUES (@EmployeeJobsid, @Usersid, @UsersidManager, @DateStartedAtPosition);
            SELECT @EmployeeHierarchyid = SCOPE_IDENTITY();
            
            SELECT [UUID], 1 [isValid], 'Successfully saved' [Message]
            FROM [dbo].[EmployeeHierarchy] 
            WHERE [Recordid] = @EmployeeHierarchyid;

            RETURN 1;
        END
    END

    END TRY
    BEGIN CATCH
        
        SELECT NULL [UUID], 0 [isValid], 'Failed to save position' [Message]
        RETURN 0;

    END CATCH

END;