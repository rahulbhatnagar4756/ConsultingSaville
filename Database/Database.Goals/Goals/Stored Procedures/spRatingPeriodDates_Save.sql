USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spRatingPeriodDates_Save]    Script Date: 18/09/2025 15:10:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Goals].[spRatingPeriodDates_Save]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200) = NULL,
    @RatingPeriodsUUID NVARCHAR(200),
    @Name NVARCHAR(50),
    @DateStart DATETIME,
    @DateEnd DATETIME = NULL,
    @DateOpen DATETIME,
    @DateClose DATETIME = NULL,
    @isActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT,
            @UsersId INT,
            @RatingPeriodDatesId BIGINT,
            @RatingPeriodsId INT,
            @NewUUID NVARCHAR(200),
            @isHasAccess BIT = 0,
            @SecurityRoleAccessIdEdit INT = 7, -- Same security role as Pillars
            @AuditLogsId BIGINT,
            @isUpdate BIT = 0;

    -- Validate Company UUID and get Company ID
    SELECT @CompanyId = [recordID] 
    FROM [Base].[dbo].[Companies] 
    WHERE [UUID] = @CompanyUUID;

    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message];
        RETURN 0;
    END

    -- Get User ID
    SELECT TOP(1) @UsersId = [Id] 
    FROM [Base].[Users] 
    WHERE [UUID] = @UsersUUIDLoggedIn;

    -- Get Rating Period ID
    SELECT @RatingPeriodsId = [Id]
    FROM [Goals].[RatingPeriods]
    WHERE [UUID] = @RatingPeriodsUUID 
      AND [Companyid] = @CompanyId 
      AND [isDeleted] = 0;

    IF ISNULL(@RatingPeriodsId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid rating period not found' [Message];
        RETURN 0;
    END

  
    -- Check security access
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] 
        @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessIdEdit;

    IF @isHasAccess = 1
    BEGIN
   

        -- Validate required fields
        IF LTRIM(RTRIM(ISNULL(@Name, ''))) = ''
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Name is required' [Message];
            RETURN 0;
        END

        -- Validate date logic
        IF @DateEnd IS NOT NULL AND @DateStart >= @DateEnd
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Start date must be before end date' [Message];
            RETURN 0;
        END

        IF @DateClose IS NOT NULL AND @DateOpen >= @DateClose
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Open date must be before close date' [Message];
            RETURN 0;
        END

        -- Business rule: Auto-toggle isActive based on DateClose
        IF @DateClose IS NOT NULL
        BEGIN
            IF @DateClose > GETDATE()
                SET @isActive = 1;  -- Future close date = active
            ELSE
                SET @isActive = 0;  -- Past close date = inactive
        END

        IF @UUID IS NULL OR @UUID = ''
        BEGIN
            -- Insert new record
            SET @NewUUID = NEWID();

            INSERT INTO [Goals].[RatingPeriodDates] 
            (
                [UUID],
                [RatingPeriodsid],
                [Name],
                [DateStart],
                [DateEnd],
                [DateOpen],
                [DateClose],
                [isActive],
                [isDeleted]
            )
            VALUES 
            (
                @NewUUID,
                @RatingPeriodsId,
                @Name,
                @DateStart,
                @DateEnd,
                @DateOpen,
                @DateClose,
                @isActive,
                0
            );

            SET @RatingPeriodDatesId = SCOPE_IDENTITY();

            -- Log creation
            EXEC [audit].[spAuditLogs_Log_Insert] 
                @CompanyId, @UsersId, 'RatingPeriodDates', @RatingPeriodDatesId, NULL, @AuditLogsId OUTPUT;

            SELECT [UUID], 1 [isValid], 'Rating period date created successfully' as [Message]
            FROM [Goals].[RatingPeriodDates]
            WHERE [Id] = @RatingPeriodDatesId;
        END
        ELSE
        BEGIN
            -- Check if the record exists
            SELECT @RatingPeriodDatesId = rpd.[Id]
            FROM [Goals].[RatingPeriodDates] rpd
            INNER JOIN [Goals].[RatingPeriods] rp ON rpd.[RatingPeriodsid] = rp.[Id]
            WHERE rpd.[UUID] = @UUID 
              AND rp.[CompanyId] = @CompanyId 
              AND rpd.[isDeleted] = 0
              AND rp.[isDeleted] = 0;

            IF ISNULL(@RatingPeriodDatesId, 0) = 0
            BEGIN
                SELECT NULL [UUID], 0 [isValid], 'Rating period date not found or already deleted' [Message];
                RETURN 0;
            END

            -- Perform the update
            UPDATE [Goals].[RatingPeriodDates]
            SET 
                [RatingPeriodsid] = @RatingPeriodsId,
                [Name] = @Name,
                [DateStart] = @DateStart,
                [DateEnd] = @DateEnd,
                [DateOpen] = @DateOpen,
                [DateClose] = @DateClose,
                [isActive] = @isActive
            WHERE [Id] = @RatingPeriodDatesId;

            -- Log the update
            EXEC [audit].[spAuditLogs_Log_Insert] 
                @CompanyId, @UsersId, 'RatingPeriodDates', @RatingPeriodDatesId, NULL, @AuditLogsId OUTPUT;

            SELECT @UUID [UUID], 1 [isValid], 'Rating period date updated successfully' [Message];
        END

  
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message];
        RETURN 0;
    END
    
END
