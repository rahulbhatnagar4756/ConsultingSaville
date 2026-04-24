CREATE OR ALTER PROCEDURE [Goals].[spUsers_GetAll]
(
    @TemplatesUUID NVARCHAR(36),
    @ContractPeriodsUUID NVARCHAR(36)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TemplateId INT,
            @ContractPeriodId INT;

    -- 1️⃣ Get Template ID
    SELECT @TemplateId = Id
    FROM [Goals].[Templates]
    WHERE UUID = @TemplatesUUID AND isDeleted = 0;

    IF @TemplateId IS NULL
    BEGIN
        RAISERROR('Template not found', 16, 1);
        RETURN;
    END

    -- 2️⃣ Get Contract Period ID
    SELECT @ContractPeriodId = Id
    FROM [Goals].[ContractPeriods]
    WHERE UUID = @ContractPeriodsUUID AND isDeleted = 0;

    IF @ContractPeriodId IS NULL
    BEGIN
        RAISERROR('Contract Period not found', 16, 1);
        RETURN;
    END

    -- 3️⃣ Return active users NOT assigned to this template & contract period
    SELECT 
        u.UUID,
        (u.firstname + ' ' + u.lastname) AS Name
    FROM [BASE].[dbo].[users] u
    LEFT JOIN [Goals].[UserTemplates] ut
        ON u.recordid = ut.UsersId
       AND ut.TemplatesId = @TemplateId
       AND ut.ContractPeriodsId = @ContractPeriodId
    WHERE u.isactive = 1
      AND ut.UsersId IS NULL;  -- Only users not in UserTemplates
END
