
CREATE   PROCEDURE [Goals].[spKPA_Duplicate]
(
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @KPAUUID NVARCHAR(200)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @CompanyId INT,
        @UsersidLoggedIn INT,
        @KPAId INT,
        @OldName NVARCHAR(500),
        @OldDescription NVARCHAR(MAX),
        @StatusId INT,
        @RatingPeriodsid INT,
        @KPAidTemplate INT,
        @ResultUUID NVARCHAR(200),
        @ContractPillarsId INT,
        @Weight DECIMAL(18,4),
        @Message NVARCHAR(MAX) = 'KPA duplicated successfully.',
        @IsValid BIT = 1,
        @isSuccessful BIT = 0,
        @AuditLogsid BIGINT;

    BEGIN TRY
        -- Validate Company
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

        -- Validate User
        SELECT @UsersidLoggedIn = Id 
        FROM [Base].[Users] 
        WHERE UUID = @UsersUUIDLoggedIn 
            AND isDeleted = 0;

        IF @UsersidLoggedIn IS NULL
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'Invalid User.';
            GOTO ErrorHandler;
        END

        -- Get existing KPA details
        SELECT 
            @KPAId = Id,
            @OldName = [Name],
            @OldDescription = [Description],
            @StatusId = [Statusid],
            @RatingPeriodsid = [RatingPeriodsid],
            @KPAidTemplate = [Id]
        FROM [Goals].[KPA]
        WHERE UUID = @KPAUUID
            AND isDeleted = 0;

        IF @KPAId IS NULL
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'KPA not found.';
            GOTO ErrorHandler;
        END

        -- Get associated Contract Pillar and Weight
        SELECT 
            @ContractPillarsId = CP.Id,
            @Weight = CK.Weight
        FROM [Goals].[ContractPillarKPAs] CK
        INNER JOIN [Goals].[ContractPillars] CP ON CK.ContractPillarsId = CP.Id
        WHERE CK.KPAId = @KPAId
            AND CK.isDeleted = 0;

        -- Begin transaction to duplicate KPA
        BEGIN TRANSACTION;

        -- Insert duplicated KPA
        SET @ResultUUID = NEWID();

        INSERT INTO [Goals].[KPA] 
        (
            [UUID],
            [CompanyId],
            [Usersid],
            [Statusid], 
            [RatingPeriodsid],
            [Name],
            [Description],
            [isActive],
            [KPAidTemplate]
        )
        VALUES 
        (
            @ResultUUID,
            @CompanyId,
            @UsersidLoggedIn,
            @StatusId,
            @RatingPeriodsid,
            @OldName + ' - Copy',
            @OldDescription,
            1, -- default active
            @KPAidTemplate
        );

        DECLARE @NewKPAId INT = SCOPE_IDENTITY();

        -- Log creation
        EXEC [audit].[spAuditLogs_Log_Insert] 
            @CompanyId, 
            @UsersidLoggedIn, 
            'KPA', 
            @NewKPAId, 
            NULL, 
            @AuditLogsid OUTPUT;

        -- Link to the same Contract Pillar with the same Weight
        EXEC [Goals].[spContractPillarKPAs_Save] 
            @CompanyId, 
            @UsersidLoggedIn, 
            NULL, 
            @ContractPillarsId, 
            @NewKPAId, 
            @Weight, 
            @isSuccessful OUTPUT, 
            @Message OUTPUT;

        IF @isSuccessful = 0
        BEGIN
            ROLLBACK TRANSACTION;
            GOTO ErrorHandler;
        END

        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @IsValid = 0;
        SET @Message = 'Error duplicating KPA: ' + ERROR_MESSAGE();
        SET @ResultUUID = NULL;
    END CATCH

ErrorHandler:
    -- Return result
    SELECT 
        @ResultUUID AS [UUID],
        @IsValid AS [isValid],
        @Message AS [Message];
END