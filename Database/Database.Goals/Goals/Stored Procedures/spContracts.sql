CREATE PROCEDURE [Goals].[spContracts]
	@CompanyUUID NVARCHAR(200),
	@UsersUUIDLoggedIn NVARCHAR(200),
	@ContractsUUID NVARCHAR(200) 
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @json NVARCHAR(MAX)
	, @Companyid INT = 0
	, @Contractsid INT = 0

	SELECT @Companyid = [Recordid]
	FROM [Base].[Companies]
	WHERE [UUID] = @CompanyUUID; 
	
	SELECT @Contractsid = [id]
	FROM [Goals].[Contracts]
	WHERE [UUID] = @ContractsUUID;

	EXEC [Goals].[spContractPillars_Save] @ContractsUUID;

	SET @json = (
				SELECT c.[UUID] [ContractsUUID]
				, com.[UUID] [CompanyUUID]
				, com.[Name] [Company]
				, u.[UUID] [UsersUUID]
				, t.[UUID] [TemplatesUUID]
				, CASE WHEN c.[Usersid] IS NOT NULL THEN u.[Firstname] + ISNULL(' ' + u.[Lastname],'') 
					ELSE CASE WHEN c.Templatesid IS NOT NULL THEN t.[Name] + ISNULL(' - ' + t.[Code],'') ELSE '' END END [ContractOwnerName]
				, cp.[UUID] [ContractPeriodsUUID]
				, cp.[Name] [ContractPeriods]
				, cp.[DateStart]
				, cp.[DateEnd]
				, CASE WHEN c.[Usersid] IS NULL THEN 0 ELSE 1 END [isUser]
				, CASE WHEN c.Templatesid IS NULL THEN 0 ELSE 1 END [isTemplate]
				, c.[isActive]
				, c.[isDeleted]

				, ( 
					SELECT est.[UUID]
					, est.[Name]
					, est.[Icons]
					, est.[IconColor]
					, (
						SELECT p.[UUID]
						, cp.[UUID] [ContractPillarsUUID]
						, p.[Name]
						, p.[Icons]
						, p.[IconColor]
						, cp.[Weight]
						
						, (
							SELECT kpa.[UUID] 
						      , cp.[UUID] [ContractPillarsUUID]
							  , kpa.[StatusUUID]
							  , kpa.[Status]
							  , kpa.[RatingPeriodsUUID]
							  , kpa.[RatingPeriods]
							  , kpa.[RatingPeriodsDisplay] 
							  , kpa.[Name]
							  , kpa.[Description]
							  , cpk.[Weight]
							  , kpa.[isActive]

							  , (
								  SELECT kpi.[UUID]
								  , kpa.[UUID] [KPAUUID]
								  , l.[UUID] [KPAKPIUUID]
								  , kpi.[CompanyUUID]
								  , kpi.[UsersUUID]
							      , kpa.[RatingPeriodsUUID]
							      , kpa.[RatingPeriods]
							      , kpa.[RatingPeriodsDisplay] 
								  , kpi.[StatusUUID]
								  , kpi.[Status]
								  , kpi.[ToleranceSetsUUID]
								  , kpi.[ToleranceSets]
								  , kpi.[ToleranceSetsDescription]
								  , kpi.[Name]
								  , kpi.[Description]
								  , kpi.[DateStart]
								  , kpi.[DateEnd]
								  , kpi.[Target] 
								  , l.[Weight]
								  , kpi.[isScoreProcessing]
								  , kpi.[isActive]
								  FROM [Goals].[vwKPI] kpi
								  INNER JOIN [Goals].[KPAKPI] l ON l.[KPIid] = kpi.[Id]
									AND l.[KPAid] = kpa.[Id]
									AND l.[isDeleted] = 0
								  
								  WHERE kpi.[isDeleted] = 0
									AND kpi.[Usersid] = kpa.[Usersid]
								  FOR JSON PATH

							) AS [KPI]
							, (
								  SELECT *
								  FROM (SELECT kpi.[UUID]
									  , l.[UUID] [KPALinkUUID]
									  , kpi.[CompanyUUID]
									  , t.[Name] [KPALinkTypes]
									  , t.[Id] [KPALinkTypesid]
									  , kpi.[UsersUUID] 
									  , kpi.[Firstname] 
									  , kpi.[Lastname]
									  , kpi.[Email]
									  , kpi.[IDNumber]
									  , kpi.[EmployeeNumber]
									  , kpi.[Name]
									  , kpi.[Description] 
									  , l.[Weight]
									  FROM [Goals].[vwKPI] kpi
									  INNER JOIN [Goals].[KPALinks] l ON l.[LinkedId] = kpi.[Id]
										AND l.[KPAid] = kpa.[Id]
										AND l.[isDeleted] = 0
									  INNER JOIN [Goals].[KPALinkTypes] t ON t.[Id] = l.[KPALinkTypesid]
										AND t.[Id] = 2
									  WHERE kpi.[isDeleted] = 0 
									  UNION SELECT kpa2.[UUID]
									  , l.[UUID] [KPALinkUUID]
									  , kpa2.[CompanyUUID]
									  , t.[Name] [KPALinkTypes]
									  , t.[Id] [KPALinkTypesid]
									  , kpa2.[UsersUUID] 
									  , kpa2.[Firstname] 
									  , kpa2.[Lastname]
									  , kpa2.[Email]
									  , kpa2.[IDNumber]
									  , kpa2.[EmployeeNumber]
									  , kpa2.[Name]
									  , kpa2.[Description] 
									  , l.[Weight]
									  FROM [Goals].[vwKPA] kpa2
									  INNER JOIN [Goals].[KPALinks] l ON l.[LinkedId] =  kpa2.[Id]
										AND l.[KPAid] = kpa.[Id]
										AND l.[isDeleted] = 0
									  INNER JOIN [Goals].[KPALinkTypes] t ON t.[Id] = l.[KPALinkTypesid]
										AND t.[Id] = 1
									  WHERE kpa2.[isDeleted] = 0) x

								  FOR JSON PATH

							) AS [Linked]

							FROM [goals].[vwKPA] kpa
							INNER JOIN [Goals].[ContractPillarKPAs] cpk ON cpk.[KPAid] = kpa.[Id]
								AND cpk.[isDeleted] = 0
								AND cpk.[ContractPillarsid] = cp.[id]
							WHERE kpa.[isDeleted] = 0
								AND kpa.[CompanyUUID] = @CompanyUUID
							FOR JSON PATH
						) [KPA]

						FROM [Goals].[vwPillars] p
						LEFT OUTER JOIN [Goals].[ContractPillars] cp ON cp.[Pillarsid] = p.[id]
							AND cp.[EnterpriseStructureTypesid] = est.[Id]
							AND cp.[Contractid] = @Contractsid
							AND cp.[isDeleted] = 0
						WHERE p.[CompanyUUID] = @CompanyUUID
							AND p.[isDeleted] = 0
						ORDER BY p.[OrderVal]
						FOR JSON PATH
					   ) AS Pillars
					
					FROM [Goals].[vwEnterpriseStructureTypes] est
					WHERE [CompanyUUID] = @CompanyUUID 
						AND est.[isDeleted] = 0
					ORDER BY est.[Orderval], est.[Id]
					FOR JSON PATH

				   ) AS EnterpriseStructures



				  FROM [Goals].[Goals].[Contracts] c
				  INNER JOIN [Base].[Companies] com ON com.[recordID] = c.[Companyid] 
						AND com.[UUID] = @CompanyUUID
	
				  INNER JOIN [Goals].[ContractPeriods] cp ON cp.[Id] = c.[ContractPeriodsid]
  

				  LEFT OUTER JOIN [base].[Users] u ON u.[Id] = c.[Usersid] 
				  LEFT OUTER JOIN [Goals].[Templates] t ON t.[id] = c.[Templatesid]
  
				   

				  WHERE c.[UUID] = @ContractsUUID
				  FOR JSON PATH, ROOT('Data')





		) ;

		  SELECT @json AS JsonResult;
END