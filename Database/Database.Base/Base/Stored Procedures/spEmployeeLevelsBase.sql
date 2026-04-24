
CREATE PROCEDURE [base].[spEmployeeLevelsBase]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT;

    -- Resolve CompanyId from UUID
    SELECT @CompanyId = [recordID]
    FROM [dbo].[Companies]
    WHERE [UUID] = @CompanyUUID;

    -- Return all non-deleted levels for the company, ordered by LevelOrder
    SELECT el.[UUID],
        el.[Name],
        el.[Description],
        el.[LevelOrder]
    FROM [dbo].[EmployeeLevels] el
    WHERE el.[Companyid] = @CompanyId
      AND el.[isDeleted] = 0
    ORDER BY el.[LevelOrder] ASC;

END