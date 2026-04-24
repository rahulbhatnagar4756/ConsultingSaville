--to save a KPA
CREATE PROCEDURE [Goals].[spKPI_Save]
  @CompanyUUID NVARCHAR(200)
, @UsersUUIDLoggedIn NVARCHAR(200)
, @UsersUUID NVARCHAR(200)
, @KPIUUID NVARCHAR(200) = NULL
, @KPAUUID NVARCHAR(200)  

, @Name NVARCHAR(500)
, @Description NVARCHAR(MAX)

, @ToleranceSetsUUID NVARCHAR(200) = NULL
, @DateStart DATETIME = NULL
, @DateEnd DATETIME = NULL
, @Target FLOAT = 100

, @Weights float
, @TrackingUUID NVARCHAR(200) = NULL
, @isDebug bit = 0
AS
BEGIN

    SET NOCOUNT ON;

	DECLARE @ProcedureName NVARCHAR(255)
    , @isValid bit = 0

	DECLARE @TrackingUUIDNew NVARCHAR(200) = COALESCE(@TrackingUUID, NEWID());
    SET @ProcedureName = OBJECT_NAME(@@PROCID);

	EXEC [Goals].[spTempTable_StatusTable] @TrackingUUID

    DECLARE @CompanyId int = 0
    , @Usersid int = 0
    , @KPIid int = 0
    , @UsersidLoggedIn int = 0
    , @TableNameidKPI int = 5
    , @TableNameidKPAKPI int = 10
    , @ToleranceSetsid int = NULL 
    , @KPAKPIId int = 0

    SELECT @Usersid = [Recordid]
    FROM [dbo].[users] u
    WHERE u.[UUID] = @UsersUUID
    
    SELECT @UsersidLoggedIn = [Recordid]
    FROM [dbo].[users] u
    WHERE u.[UUID] = @UsersUUIDLoggedIn

    SELECT @CompanyId = [Recordid]
    FROM [dbo].[Companies] c
    WHERE c.[UUID] = @CompanyUUID 

    IF @ToleranceSetsUUID IS NOT NULL
    BEGIN
        SELECT @ToleranceSetsid = [Id]
        FROM [Goals].[ToleranceSets]
        WHERE [UUID] = @ToleranceSetsUUID
    END


    IF @KPIUUID IS NULL
    BEGIN

        INSERT INTO [Goals].[KPI]( [Companyid], [Usersid], [ToleranceSetsid], [Name], [Description], [DateStart], [DateEnd], [Target])
        VALUES(@Companyid, @Usersid, @ToleranceSetsid, @Name, @Description, @DateStart, @DateEnd, @Target)
        SET @KPIid = SCOPE_IDENTITY();

        SELECT @KPIUUID = [UUID]
        FROM [Goals].[KPI]
        WHERE [Id] = @KPIid

        --output result
        EXEC [Goals].[spTempTable_StatusTable_Save] @TrackingUUID = @TrackingUUIDNew,
                                                    @ProcedureName = @ProcedureName,
                                                    @UUID = @KPIUUID,
                                                    @isSuccessful = 1,
                                                    @Message = 'KPI successfully saved.';

         --Log that data has been inserted and by who 
        EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                             @UsersIdLoggedIn = @UsersIdLoggedIn,
                                             @TableName  = 'KPI',
                                             @TableNameid = @TableNameidKPI,
                                             @TableId = @KPIid, 
                                             @isInsert = 1;

    END
    ELSE
    BEGIN
        DECLARE @NameCurrent NVARCHAR(500) = NULL
        , @DescriptionCurrent NVARCHAR(MAX) = NULL
        , @ToleranceSetsIdCurrent NVARCHAR(200) = NULL
        , @DateStartCurrent DATETIME = NULL
        , @DateEndCurrent DATETIME = NULL
        , @TargetCurrent FLOAT = 100


        ------------------------------------------------------------
        --check what has changed then indicate the chages on the log
        ------------------------------------------------------------
        SELECT @isValid = CASE WHEN k.[Name] <> @Name OR k.[Description] <> @Description  THEN 1 ELSE 0 END
        , @NameCurrent = CASE WHEN ISNULL(k.[Name],'') <> ISNULL(@Name,'') THEN k.[Name] END
        , @DescriptionCurrent = CASE WHEN ISNULL(k.[Description],'') <> ISNULL(@Description,'') THEN k.[Description] END
        , @DateStartCurrent = CASE WHEN k.[DateStart] <> @DateStartCurrent THEN k.[DateStart] END  
        , @DateEndCurrent = CASE WHEN ISNULL(k.[DateEnd],GETDATE()) <> ISNULL(@DateEndCurrent, GETDATE()) THEN k.[DateEnd] END  
        , @TargetCurrent = CASE WHEN ISNULL(k.[Target], 0) <> ISNULL(@TargetCurrent, 0) THEN k.[Target] END  
        , @ToleranceSetsIdCurrent = CASE WHEN ISNULL(k.[ToleranceSetsid], 0) <> ISNULL(@ToleranceSetsIdCurrent, 0) THEN k.[ToleranceSetsid] END  
        , @KPIid = k.[Id]
        FROM [Goals].[KPI] k
        WHERE k.[isDeleted] = 0 AND k.[UUID] = @KPIUUID
        
        IF @isValid = 1
        BEGIN

            UPDATE [Goals].[KPA]
            SET  [Name] = @Name
               , [Description] = @Description
            WHERE [UUID] = @KPAUUID

            --Result Status Update
            EXEC [Goals].[spTempTable_StatusTable_Save] @TrackingUUID = @TrackingUUIDNew,
                                                        @ProcedureName = @ProcedureName,
                                                        @UUID = @KPIUUID,
                                                        @isSuccessful = 1,
                                                        @Message = 'KPI successfully updated.';

            --Log that data has been updated and by who 
            IF NOT @NameCurrent IS NULL
             BEGIN
                EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                                     @UsersIdLoggedIn = @UsersIdLoggedIn,
                                                     @TableName  = 'KPI',
                                                     @TableNameid = @TableNameidKPI,
                                                     @TableId = @KPIid, 
                                                     @ColumnName = 'Name',
                                                     @OldValue = @NameCurrent,
                                                     @NewValue = @Name,
                                                     @isUpdate = 1;
            END

            --Log that data has been updated and by who 
            IF NOT @DescriptionCurrent IS NULL
             BEGIN
                EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                                     @UsersIdLoggedIn = @UsersIdLoggedIn,
                                                     @TableName  = 'KPI',
                                                     @TableNameid = @TableNameidKPI,
                                                     @TableId = @KPIid, 
                                                     @ColumnName = 'Description',
                                                     @OldValue = @DescriptionCurrent,
                                                     @NewValue = @Description,
                                                     @isUpdate = 1;
            END

            IF NOT @DateStartCurrent IS NULL
             BEGIN
                EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                                     @UsersIdLoggedIn = @UsersIdLoggedIn,
                                                     @TableName  = 'KPI',
                                                     @TableNameid = @TableNameidKPI,
                                                     @TableId = @KPIid, 
                                                     @ColumnName = 'DateStart',
                                                     @OldValue = @DateStartCurrent,
                                                     @NewValue = @DateEnd,
                                                     @isUpdate = 1;
            END

            IF NOT @DateEndCurrent IS NULL
             BEGIN
                EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                                     @UsersIdLoggedIn = @UsersIdLoggedIn,
                                                     @TableName  = 'KPI',
                                                     @TableNameid = @TableNameidKPI,
                                                     @TableId = @KPIid, 
                                                     @ColumnName = 'DateEnd',
                                                     @OldValue = @DateEndCurrent,
                                                     @NewValue = @DateStart,
                                                     @isUpdate = 1;
            END

            IF NOT @TargetCurrent IS NULL
             BEGIN
                EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                                     @UsersIdLoggedIn = @UsersIdLoggedIn,
                                                     @TableName  = 'KPI',
                                                     @TableNameid = @TableNameidKPI,
                                                     @TableId = @KPIid, 
                                                     @ColumnName = 'Target',
                                                     @OldValue = @TargetCurrent,
                                                     @NewValue = @Target,
                                                     @isUpdate = 1;
            END

            IF NOT @ToleranceSetsIdCurrent IS NULL
             BEGIN
                EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                                     @UsersIdLoggedIn = @UsersIdLoggedIn,
                                                     @TableName  = 'KPI',
                                                     @TableNameid = @TableNameidKPI,
                                                     @TableId = @KPIid, 
                                                     @ColumnName = 'ToleranceSetsid',
                                                     @OldValue = @ToleranceSetsIdCurrent,
                                                     @NewValue = @ToleranceSetsid,
                                                     @isUpdate = 1;
            END
        END

    END

    --check if link between pillars and kpa exist
    IF NOT EXISTS(SELECT *
                  FROM [Goals].[KPAKPI] pk
                  INNER JOIN [Goals].[KPI] p ON p.[Id] = pk.[KPIid] AND p.[UUID] = @KPIUUID
                  INNER JOIN [Goals].[KPA] k ON k.[Id] = pk.[KPAid] AND k.[UUID] = @KPAUUID
                  WHERE pk.[isDeleted] = 0)
    BEGIN

        DECLARE @KPAid int = 0
            
        SELECT @KPAid = [id]
        FROM [Goals].[KPI] k
        WHERE k.[UUID] = @KPIUUID OR k.[Id] = @KPAid

        SELECT @KPAid = p.[Id]
        FROM [Goals].[KPA] p
        WHERE p.[UUID] = @KPAUUID

        --insert link the KPA to the Pillar
        INSERT INTO [Goals].[KPAKPI] ([KPIid], [KPAid], [Weight])
        VALUES (@KPIid, @KPAid, @Weights)
        SET @KPAKPIId = SCOPE_IDENTITY()

        --Log that data has been inserted and by who 
        EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                             @UsersIdLoggedIn = @UsersIdLoggedIn,
                                             @TableName  = 'KPAKPI',
                                             @TableNameid = @TableNameidKPAKPI,
                                             @TableId = @KPAKPIId,
                                             @isInsert = 1

    END
    ELSE
    BEGIN
        DECLARE @WeightsCurrent float

        SELECT @isValid = CASE WHEN pk.[Weight] = @Weights THEN 0 ELSE 1 END
        , @WeightsCurrent = CASE WHEN pk.[Weight] <> @Weights THEN pk.[Weight] END
        , @KPAKPIId = pk.[Id]
        FROM [Goals].[KPAKPI] pk
        INNER JOIN [Goals].[KPI] p ON p.[Id] = pk.[KPIid] AND p.[UUID] = @KPIUUID
        INNER JOIN [Goals].[KPA] k ON k.[Id] = pk.[KPAid] AND k.[UUID] = @KPAUUID
        WHERE pk.[isDeleted] = 0

        IF @isValid = 1
        BEGIN

            UPDATE pk
            SET pk.[Weight] = @Weights
            FROM [Goals].[PillarKPAs] pk
            WHERE pk.[Id] = @KPAKPIId

            IF NOT @WeightsCurrent IS NULL
            BEGIN
                EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                                     @UsersIdLoggedIn = @UsersIdLoggedIn,
                                                     @TableName  = 'KPAKPI',
                                                     @TableNameid = @TableNameidKPAKPI,
                                                     @TableId = @KPAKPIId,
                                                     @ColumnName = 'Weight',
                                                     @OldValue = @WeightsCurrent,
                                                     @NewValue = @Weights,
                                                     @isUpdate = 1
            
            END

        END
    END

    EXEC [Goals].[spTempTable_StatusTable_Display] @TrackingUUID = @TrackingUUID

END
