CREATE PROCEDURE [Goals].[spContracts_Clone]
      @CompanyUUID nvarchar(200)
    , @UsersUUIDLoggedIn nvarchar(200)
    , @UsersUUID nvarchar(200)
    , @ContractsUUIDToSync nvarchar(200)
    , @TrackingUUID NVARCHAR(200) = null
AS
BEGIN

    DECLARE @isValid bit = 0
    , @Contractid INT = null
    , @Usersid int = null
    , @ContractsIdToSync int = null
    , @ContractsUUID nvarchar(200)
    , @ProcedureName NVARCHAR(255)

    DECLARE @TrackingUUIDNew NVARCHAR(200) = COALESCE(@TrackingUUID, NEWID());
    SET @ProcedureName = OBJECT_NAME(@@PROCID);

	EXEC [Goals].[spTempTable_StatusTable] @TrackingUUID

    SELECT TOP(1) @Usersid = [recordid]
    FROM [dbo].[users] u
    WHERE u.[UUID] = @UsersUUID

    SELECT TOP(1) @ContractsIdToSync = c.[Id]
    FROM [Goals].[Contracts] c
    WHERE c.[UUID] = @ContractsUUIDToSync 

    SELECT top(1) @Contractid = s.[Contractid]
    FROM [Goals].[ContractUsersSync] s
    INNER JOIN [users] u ON u.[recordid] = s.[Usersid] 
    INNER JOIN [Goals].[Contracts] c ON c.[UUID] = @ContractsUUIDToSync
    WHERE s.[isDeleted] = 0

    IF ISNULL(@Contractid, 0) = 0
    BEGIN

        INSERT INTO [Goals].[Contracts]([Companyid], [Usersid], [ContractPeriodsid]
        , [DateStart], [DateEnd], [isDepartmentTemplateSync], [isIndividualTemplateSync])
        SELECT c.[Companyid]
        , @Usersid
        , c.[ContractPeriodsid]
        , c.[DateStart]
        , c.[DateEnd]
        , c.[isDepartmentTemplateSync]
        , c.[isIndividualTemplateSync]
        FROM [Goals].[Contracts] c
        WHERE c.[UUID] = @ContractsUUIDToSync    
        SELECT @Contractid = SCOPE_IDENTITY();

        INSERT INTO [Goals].[ContractUsersSync]([Usersid], [Contractid], [ContractidSyncFrom])
        VALUES (@Usersid, @Contractid, @ContractsIdToSync )

    END

    --need to sync the KPA and KPI's

    SELECT @ContractsUUID = c.[UUID]
    FROM [Goals].[Contracts] c
    WHERE c.[Id] = @Contractid

    INSERT INTO #StatusTable(TrackingUUID, [Procedure], [UUID], [isSuccessful], [Message])
    VALUES(@TrackingUUIDNew, @ProcedureName, @ContractsUUID, 1, 'Contract successfully synchronized.');

     IF @TrackingUUID IS NULL
        SELECT *
        FROM #StatusTable;

    RETURN 1
END
