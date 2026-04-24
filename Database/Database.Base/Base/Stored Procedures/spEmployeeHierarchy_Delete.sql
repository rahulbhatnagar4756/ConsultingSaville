CREATE PROCEDURE [base].[spEmployeeHierarchy_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @EmployeeHierarchyUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UsersidLoggedIn INT
    , @EmployeeHierarchyId INT;

    -- Validate that the user executing the delete exists
    --SELECT @UsersidLoggedIn = recordid 
    --FROM [dbo].[users] 
    --WHERE UUID = @UsersUUIDLoggedIn;

    --IF @UsersidLoggedIn IS NULL
    --BEGIN
    --    SELECT NULL [UUID], 0 [isValid], 'User  does not exist' [Message]
    --    RETURN;
    --END

    -- Validate that the EmployeeHierarchy record exists and is not already deleted
    SELECT @EmployeeHierarchyId = Recordid 
    FROM [dbo].[EmployeeHierarchy] 
    WHERE UUID = @EmployeeHierarchyUUID AND isDeleted = 0;

    IF @EmployeeHierarchyId IS NULL
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Employee hierarchy does not exist' [Message]
        RETURN;
    END

    -- Soft delete the record
    UPDATE [dbo].[EmployeeHierarchy]
    SET isDeleted = 1
    WHERE UUID = @EmployeeHierarchyUUID;
       

    SELECT @EmployeeHierarchyUUID [UUID], 1 [isValid], 'Employee hierarchy successfully removed' [Message]
END;
