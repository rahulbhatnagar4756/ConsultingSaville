USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spLinkedUserTemplate_GetAll]    Script Date: 03-12-2025 17:05:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [Goals].[spLinkedUserTemplate_GetAll]
    @TemplatesUUID NVARCHAR(200),
    @ContractPeriodsUUID NVARCHAR(200)
AS
BEGIN
    DECLARE @TemplateId INT;
    DECLARE @ContractPeriodId INT;

    -- Start error handling
    BEGIN TRY
        -- Validate Template ID
        SELECT @TemplateId = Id 
        FROM [Goals].[Templates]
        WHERE UUID = @TemplatesUUID 
          AND isDeleted = 0;

        IF @TemplateId IS NULL
        BEGIN
            SELECT NULL AS [UUID], 0 AS [isValid], 'Invalid Template UUID or Template not found.' AS [Message];
            RETURN 0;
        END

        -- Validate Contract Period ID
        SELECT @ContractPeriodId = Id 
        FROM [Goals].[ContractPeriods]
        WHERE UUID = @ContractPeriodsUUID 
          AND isDeleted = 0;

        IF @ContractPeriodId IS NULL
        BEGIN
            SELECT NULL AS [UUID], 0 AS [isValid], 'Invalid Contract Period UUID or Contract Period not found.' AS [Message];
            RETURN 0;
        END

        -- Fetch linked user templates, including User UUID and Name
        SELECT 
            u.UUID AS UUID,
            (u.firstname + ' ' + u.lastname) AS Name
        FROM [Goals].[UserTemplates] ut
        INNER JOIN [BASE].[dbo].[users] u 
            ON u.recordid = ut.Usersid
        WHERE ut.[Templatesid] = @TemplateId
          AND ut.[ContractPeriodsid] = @ContractPeriodId
          AND ut.[isDeleted] = 0
          AND u.isactive = 1;

    END TRY
    BEGIN CATCH
        SELECT NULL AS [UUID], 0 AS [isValid], 
               'An error occurred: ' + ERROR_MESSAGE() AS [Message];
        RETURN -1;
    END CATCH
END
