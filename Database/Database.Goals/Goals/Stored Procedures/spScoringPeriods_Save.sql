USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spScoringPeriods_Save]    Script Date: 19/09/2025 11:36:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =====================================================================================
-- Author:      System Generated
-- Create date: 2025-09-19
-- Description: Creates or updates scoring periods with proper lifecycle management
--              
-- Business Rules:
-- 1. Cannot create scoring periods for invalid companies
-- 2. Only logged-in users can perform operations
-- 3. Deactivated scoring periods CANNOT be reactivated (create new instead)
-- 4. Active periods can be deactivated
-- 5. DateDeactivated is auto-set when deactivating, cleared when reactivating
--
-- Parameters:
-- @CompanyUUID         - Company identifier (required)
-- @UsersUUIDLoggedIn   - User performing the operation (required)
-- @UUID                - NULL for INSERT, value for UPDATE
-- @Name                - Period name (required)
-- @Description         - Optional description
-- @StartDay            - Day of start (1-31, required)
-- @StartMonth          - Month of start (1-12, required)
-- @EndDay              - Day of end (1-31, required)
-- @EndMonth            - Month of end (1-12, required)
-- @isActive            - NULL preserves current, 1=active, 0=inactive
-- @DateDeactivated     - NULL for auto-calculation, specific date to override
-- =====================================================================================

ALTER PROCEDURE [Goals].[spScoringPeriods_Save]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200) = NULL,
    @Name NVARCHAR(200),
    @Description NVARCHAR(MAX) = NULL,
    @StartDay TINYINT,
    @StartMonth TINYINT,
    @EndDay TINYINT,
    @EndMonth TINYINT,
    @isActive BIT = NULL,             -- Nullable: allows preserving current state if not passed
    @DateDeactivated DATETIME = NULL  -- Nullable: auto-calculated if not passed
AS
BEGIN
    -- Prevent extra result sets from affecting calling applications
    SET NOCOUNT ON;

    -- ====================================================================
    -- VARIABLE DECLARATIONS
    -- ====================================================================
    DECLARE @CompanyId INT,
            @UsersId INT,
            @ScoringPeriodId BIGINT,
            @NewUUID NVARCHAR(200),
            @isHasAccess BIT = 0,
            @SecurityRoleAccessIdEdit INT = 7, -- Security Role: Same as RatingPeriods
            @AuditLogsId BIGINT,
            @CurrentIsActive BIT,              -- For UPDATE: current database state
            @NewDateDeactivated DATETIME;      -- For UPDATE: calculated deactivation date

    -- ====================================================================
    -- 1️⃣ COMPANY VALIDATION
    -- Business Rule: Cannot create/update scoring periods for invalid company
    -- ====================================================================
    SELECT @CompanyId = [recordID] 
    FROM [Base].[dbo].[Companies] 
    WHERE [UUID] = @CompanyUUID;

    -- Exit if company not found
    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SELECT NULL AS [UUID], 0 AS [isValid], 'Valid company not supplied' AS [Message];
        RETURN 0;
    END

    -- ====================================================================
    -- 2️⃣ USER VALIDATION
    -- Business Rule: Only logged-in users can perform operations
    -- ====================================================================
    SELECT TOP(1) @UsersId = [Id] 
    FROM [Base].[Users] 
    WHERE [UUID] = @UsersUUIDLoggedIn;

    -- Note: Currently not validating user exists, but could add check here
    -- IF ISNULL(@UsersId, 0) = 0 BEGIN ... END

    -- ====================================================================
    -- 3️⃣ SECURITY ROLE VALIDATION (Currently Disabled)
    -- TODO: Enable when security system is ready
    -- Business Rule: User must have edit permissions for scoring periods
    -- ====================================================================
  
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] 
        @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessIdEdit;

    IF @isHasAccess = 0
    BEGIN
        SELECT NULL AS [UUID], 0 AS [isValid], 'Access denied' AS [Message];
        RETURN 0;
    END


    -- ====================================================================
    -- 4️⃣ INPUT VALIDATION
    -- Validate all required fields before proceeding
    -- ====================================================================
    
    -- Name is mandatory and cannot be empty/whitespace
    IF LTRIM(RTRIM(ISNULL(@Name, ''))) = ''
    BEGIN
        SELECT NULL AS [UUID], 0 AS [isValid], 'Name is required' AS [Message];
        RETURN 0;
    END

    -- Start/End day and month are mandatory for period definition
    IF @StartDay IS NULL OR @StartMonth IS NULL OR @EndDay IS NULL OR @EndMonth IS NULL
    BEGIN
        SELECT NULL AS [UUID], 0 AS [isValid], 'StartDay, StartMonth, EndDay, EndMonth are required' AS [Message];
        RETURN 0;
    END

    -- Optional: Add range validation for day/month values
    IF @StartDay < 1 OR @StartDay > 31 OR @EndDay < 1 OR @EndDay > 31
    BEGIN
        SELECT NULL AS [UUID], 0 AS [isValid], 'Day values must be between 1 and 31' AS [Message];
        RETURN 0;
    END

    IF @StartMonth < 1 OR @StartMonth > 12 OR @EndMonth < 1 OR @EndMonth > 12
    BEGIN
        SELECT NULL AS [UUID], 0 AS [isValid], 'Month values must be between 1 and 12' AS [Message];
        RETURN 0;
    END

    -- ====================================================================
    -- 5️⃣ INSERT LOGIC (CREATE NEW SCORING PERIOD)
    -- When @UUID is NULL or empty, create a new record
    -- ====================================================================
    IF @UUID IS NULL OR LTRIM(RTRIM(@UUID)) = ''
    BEGIN
        -- Generate new UUID for the record
        SET @NewUUID = NEWID();

        -- Business Rule: Auto-set DateDeactivated if creating inactive period
        -- If user explicitly creates inactive period, mark deactivation time
        IF @isActive = 0 AND @DateDeactivated IS NULL
            SET @DateDeactivated = GETDATE();

        -- Insert new scoring period record
        INSERT INTO [Goals].[ScoringPeriods]
        (
            [UUID],
              [CompanyId], 
            [Name],
            [Description],
            [DateCreated],
            [StartDay],
            [StartMonth],
            [EndDay],
            [EndMonth],
            [isActive],
            [isDeleted],
            [DateDeactivated]
        )
        VALUES
        (
            @NewUUID,
               @CompanyId,  
            @Name,
            @Description,
            GETDATE(),              -- Record creation timestamp
            @StartDay,
            @StartMonth,
            @EndDay,
            @EndMonth,
            ISNULL(@isActive, 1),   -- Default to active if not specified
            0,                      -- New records are never deleted
            @DateDeactivated        -- NULL for active, date for inactive
        );

        -- Get the identity value for the new record
        SET @ScoringPeriodId = SCOPE_IDENTITY();

        -- Log the creation in audit trail
        EXEC [audit].[spAuditLogs_Log_Insert] 
            @CompanyId, @UsersId, 'ScoringPeriods', @ScoringPeriodId, NULL, @AuditLogsId OUTPUT;

        -- Return success response with the new UUID
        SELECT [UUID], 1 AS [isValid], 'Scoring period created successfully' AS [Message]
        FROM [Goals].[ScoringPeriods]
        WHERE [Id] = @ScoringPeriodId;
    END
    ELSE
    BEGIN
        -- ================================================================
        -- 6️⃣ UPDATE LOGIC (MODIFY EXISTING SCORING PERIOD)
        -- When @UUID is provided, update existing record
        -- ================================================================
        
        -- First, retrieve current record information to determine current state
        -- This is essential for applying business rules and status transitions
        SELECT 
            @ScoringPeriodId = [Id],
            @CurrentIsActive = [isActive]
        FROM [Goals].[ScoringPeriods]
        WHERE [UUID] = @UUID AND [isDeleted] = 0;

        -- Verify record exists and is not deleted
        IF ISNULL(@ScoringPeriodId, 0) = 0
        BEGIN
            SELECT NULL AS [UUID], 0 AS [isValid], 'Scoring period not found or already deleted' AS [Message];
            RETURN 0;
        END

        -- ================================================================
        -- STATUS PRESERVATION LOGIC
        -- If @isActive is NULL, preserve the current status (allows partial updates)
        -- ================================================================
        IF @isActive IS NULL
            SET @isActive = @CurrentIsActive;

        -- ================================================================
        -- BUSINESS RULE ENFORCEMENT
        -- Critical Rule: Deactivated periods cannot be reactivated
        -- Users must create new periods instead of reactivating old ones
        -- ================================================================
        IF @CurrentIsActive = 0 AND @isActive = 1
        BEGIN
            SELECT NULL AS [UUID], 0 AS [isValid], 
                   'Cannot reactivate a deactivated scoring period. Create a new one.' AS [Message];
            RETURN 0;
        END

        -- ================================================================
        -- DEACTIVATION DATE CALCULATION
        -- Handle different scenarios for managing DateDeactivated field
        -- ================================================================
        SET @NewDateDeactivated = CASE
            -- Scenario 1: Currently active, being deactivated (1 → 0)
            -- Set deactivation timestamp to current time
            WHEN @isActive = 0 AND @CurrentIsActive = 1 THEN GETDATE()
            
            -- Scenario 2: Currently inactive, being reactivated (0 → 1)
            -- Clear the deactivation date (should not happen due to business rule above)
            WHEN @isActive = 1 AND @CurrentIsActive = 0 THEN NULL
            
            -- Scenario 3: User provided specific deactivation date
            -- Use the explicitly provided date
            WHEN @DateDeactivated IS NOT NULL THEN @DateDeactivated
            
            -- Scenario 4: No status change or no date provided
            -- Preserve the existing DateDeactivated value
            ELSE (SELECT [DateDeactivated] FROM [Goals].[ScoringPeriods] WHERE [Id] = @ScoringPeriodId)
        END;

        -- ================================================================
        -- PERFORM UPDATE OPERATION
        -- Update all modifiable fields with new values
        -- ================================================================
        UPDATE [Goals].[ScoringPeriods]
        SET 
            [Name] = @Name,
            [Description] = @Description,
            [StartDay] = @StartDay,
            [StartMonth] = @StartMonth,
            [EndDay] = @EndDay,
            [EndMonth] = @EndMonth,
            [isActive] = @isActive,
            [DateDeactivated] = @NewDateDeactivated
        WHERE [Id] = @ScoringPeriodId;

        -- ================================================================
        -- AUDIT TRAIL LOGGING
        -- Record the update operation for compliance and tracking
        -- ================================================================
        EXEC [audit].[spAuditLogs_Log_Insert] 
            @CompanyId, @UsersId, 'ScoringPeriods', @ScoringPeriodId, NULL, @AuditLogsId OUTPUT;

        -- ================================================================
        -- SUCCESS RESPONSE
        -- Return contextual success message based on the operation performed
        -- ================================================================
        SELECT @UUID AS [UUID], 1 AS [isValid], 
               CASE 
                   WHEN @isActive = 1 THEN 'Scoring period updated successfully'
                   WHEN @CurrentIsActive = 1 AND @isActive = 0 THEN 'Scoring period deactivated successfully'
                   ELSE 'Scoring period updated successfully'
               END AS [Message];
    END

    -- ====================================================================
    -- PROCEDURE COMPLETION
    -- All execution paths above should return appropriate response to caller
    -- No additional code should be added after this point
    -- ====================================================================
END