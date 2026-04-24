
CREATE   PROCEDURE [Goals].[spKPA_Delete]
(
    @UUID NVARCHAR(200),
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200)
)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT;
    DECLARE @UsersId INT;
    DECLARE @KPAId INT;
    DECLARE @IsValid BIT = 1;
    DECLARE @Message NVARCHAR(MAX) = 'KPA deleted successfully.';
    
    BEGIN TRY
       
        
        -- Validate and get CompanyId
        SELECT @CompanyId = [recordID] 
        FROM [Base].[Companies] 
        WHERE UUID = @CompanyUUID 
            AND isDeleted = 0;

        IF @CompanyId IS NULL
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'Invalid Company.';
            GOTO ErrorHandler;
        END
        
        -- Validate and get UsersId
        SELECT @UsersId = Id 
        FROM [Base].[Users] 
        WHERE UUID = @UsersUUIDLoggedIn 
            AND isDeleted = 0;

        IF @UsersId IS NULL
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'Invalid User.';
            GOTO ErrorHandler;
        END
        
        -- Validate KPA exists and belongs to company
        SELECT @KPAId = Id 
        FROM [Goals].[KPA] 
        WHERE UUID = @UUID 
            AND CompanyId = @CompanyId 
            AND isDeleted = 0;
        
        IF @KPAId IS NULL
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'KPA not found or already deleted.';
            GOTO ErrorHandler;
        END
        
        BEGIN TRANSACTION;

        -- Soft delete the KPA
        UPDATE [Goals].[KPA]
        SET 
            isDeleted = 1,
            DateLastActive = GETUTCDATE()
        WHERE Id = @KPAId;
               
        COMMIT TRANSACTION;
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @IsValid = 0;
        SET @Message = 'Error deleting KPA: ' + ERROR_MESSAGE();
    END CATCH
    
    ErrorHandler:
    -- Return result
    SELECT 
        @UUID AS [UUID],
        @IsValid AS [isValid],
        @Message AS [Message];
END