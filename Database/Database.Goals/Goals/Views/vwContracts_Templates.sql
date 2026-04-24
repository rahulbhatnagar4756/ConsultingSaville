




CREATE VIEW [Goals].[vwContracts_Templates]
AS
SELECT ROW_NUMBER() OVER(PARTITION BY c.[Companyid], c.[Templatesid] ORDER BY c.[isDeleted] ASC, c.[isActive] DESC, c.[DateStart] DESC) [No]
      , c.[Id]
      , c.[UUID]
      , c.[Companyid]
      , cc.[UUID] [CompanyUUID]
      , cc.[Name] [Company]
      , c.[Templatesid]   
      , t.[UUID] [TemplatesUUID]
      , t.[Name] [Templates]
      , t.[Code] [TemplatesCode]
      , c.[ContractPeriodsid]
      , cp.[UUID] [ContractPeriodsUUID]
      , cp.[Name] [ContractPeriods]
      , cp.[DateStart] [ContractPeriodsDateStart]
      , cp.[DateEnd] [ContractPeriodsDateEnd]
      , cp.[Year] [ContractPeriodsYear]
      , c.[DateCreated]
      , c.[DateStart]
      , c.[DateEnd]
      , c.[isDepartmentTemplateSync]
      , c.[isIndividualTemplateSync]
      , c.[isActive]
      , c.[isDeleted]
  FROM [Goals].[Contracts] c
  INNER JOIN [Base].[Companies] cc ON cc.[recordID] = c.[Companyid] 
  INNER JOIN [Goals].[Templates] t ON t.[Id] = c.[Templatesid] 
    AND t.[isDeleted] = 0
  INNER JOIN [Goals].[ContractPeriods] cp ON cp.[Id] = c.[ContractPeriodsid] 
