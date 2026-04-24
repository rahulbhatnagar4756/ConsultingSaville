


CREATE VIEW [Goals].[vwContracts_Employees]
AS
        SELECT ROW_NUMBER() OVER(PARTITION BY comp.[recordID], c.[Usersid] ORDER BY c.[isDeleted] ASC, c.[isActive] DESC, c.[DateStart] DESC) [No] 
        , comp.[recordID] [Companyid]
        , comp.[UUID] [CompanyUUID]
        , t.[Id] [Templatesid]
        , t.[UUID] [TemplatesUUID]
        , t.[Name] [Templates]

        , u.[UUID] [usersUUID]
        , u.[firstname]
        , u.[lastname]
        , u.[email] 
        , u.[mobile]
        , u.[IDNumber]
        , u.[Race]
        , u.[Gender]
        , u.[DateOfBirth]
        , comp.[URLAddress] + 'images/Avatars/' + CONVERT(NVARCHAR, u.[id]) + 'Medium.jpg' [UsersImageURL]

        , c.[Id] [Contractsid]
        , c.[UUID] [ContractsUUID]
        , c.[DateStart]
        , c.[DateEnd]
        , c.[isDepartmentTemplateSync]
        , c.[isIndividualTemplateSync]
        , c.[isActive] [isActiveContracts]
        , ct.[isActive] [isActiveContractsSync]
        FROM [base].[users] u
        INNER JOIN [base].[employees] e ON e.[Usersid] = u.[Id] 
            AND e.[isActive] = 1 
            AND e.[isdeleted] = 0
        INNER JOIN [base].[Companies] comp ON comp.[recordID] = e.[Companyid]  
        INNER JOIN [Goals].[Contracts] c ON c.[Usersid] = u.[Id] AND c.[isDeleted] = 0 

        LEFT OUTER JOIN [Goals].[ContractUsersSync] cu ON cu.[Usersid] = u.[Id] 
            AND cu.[isDeleted] = 0 
        LEFT OUTER JOIN [Goals].[Contracts] ct ON ct.[Id] =cu.[ContractidSyncFrom] 
            AND ct.[isDeleted] = 0 
            AND ct.[Templatesid] IS NOT NULL 
        LEFT OUTER JOIN [Goals].[Templates] t ON t.[Id] = ct.[Templatesid] AND t.[isDeleted] = 0

        WHERE u.[isdeleted] = 0 AND u.[isactive] = 1
