USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spScoringPeriods_Get]    Script Date: 19/09/2025 15:02:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Goals].[spScoringPeriods_Get]
   @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @ScoringPeriodUUID NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        sp.[UUID],
        sp.[Companyid],
        sp.[Name],
        sp.[Description],
        sp.[DateCreated],
        sp.[StartDay],
        sp.[StartMonth],
        sp.[EndDay],
        sp.[EndMonth],
        sp.[DateDeactivated],
        sp.[isActive],
        sp.[isDeleted]
    FROM [Goals].[ScoringPeriods] sp
       INNER JOIN [Base].[dbo].[Companies] c 
        ON sp.[Companyid] = c.[recordID] 
       AND c.[UUID] = @CompanyUUID
    WHERE sp.[isDeleted] = 0
      AND (@ScoringPeriodUUID IS NULL OR sp.[UUID] = @ScoringPeriodUUID);
END
