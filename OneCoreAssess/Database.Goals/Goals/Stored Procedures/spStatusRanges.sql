CREATE PROCEDURE [Goals].[spStatusRanges]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @StatusUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
     
    -- Get all status ranges in hierarchical order
    SELECT sr.[UUID]
    , s.[UUID] [StatusUUID]
    , sr.[Name]
    , ISNULL(i.[Name], 'clock_loader_40') as Icons
    , sr.[Color]
    , sr.[RangeStart]
    , LEAD(sr.[RangeStart]) OVER (ORDER BY sr.[RangeStart]) AS [RangeEnd]
    , sr.[isNegative] 
    FROM [Goals].[StatusRanges] sr
    INNER JOIN [Goals].[Status] s ON sr.[Statusid] = s.[Id]
        AND s.[UUID] = @StatusUUID
        AND s.[isDeleted] = 0
    INNER JOIN [Base].[Companies] c ON s.[Companyid] = c.[recordID]
        AND c.[UUID] = @CompanyUUID
    LEFT OUTER JOIN [admin].[Icon] i ON sr.[Iconsid] = i.[Id]
    WHERE sr.[isDeleted] = 0
        AND sr.[StatusRangesid] IS NULL 
    ORDER BY CASE WHEN [RangeStart] IS NULL THEN 1 ELSE 10 END, [RangeStart]
END
