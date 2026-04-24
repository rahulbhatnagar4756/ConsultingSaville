CREATE PROCEDURE [Goals].[spKPA_Delete]
  @CompanyUUID NVARCHAR(200)
, @UsersUUIDLoggedIn NVARCHAR(200) 
, @KPAUUID NVARCHAR(200) = null 
, @TrackingUUID NVARCHAR(200) = NULL
, @isDebug bit = 0
AS
BEGIN

    SET NOCOUNT ON;

	DECLARE @ProcedureName NVARCHAR(255)
    , @isValid bit = 0

	DECLARE @TrackingUUIDNew NVARCHAR(200) = COALESCE(@TrackingUUID, NEWID());
    SET @ProcedureName = OBJECT_NAME(@@PROCID);

    --create the Status tracking temp table
	EXEC [Goals].[spTempTable_StatusTable] @TrackingUUID

    DECLARE @CompanyId int = 0
    , @KPAid int = 0
    , @Usersid int = 0
    , @UsersidLoggedIn int = 0
    , @TableNameidKPA int = 8
    , @TableNameidPillarKPAs int = 12
    
    SELECT @UsersidLoggedIn = [Recordid]
    FROM [dbo].[users] u
    WHERE u.[UUID] = @UsersUUIDLoggedIn

    SELECT @CompanyId = [Recordid]
    FROM [dbo].[Companies] c
    WHERE c.[UUID] = @CompanyUUID 

    IF @KPAUUID IS NOT NULL
    BEGIN

        UPDATE [Goals].[KPA] 
        SET [isDeleted] = 0
        , [DateEnded] = COALESCE([DateEnded], GETUTCDATE())
        WHERE [UUID] = @KPAUUID 

        SELECT @KPAid = [Id] 
        , @Usersid = [Usersid]
        FROM [Goals].[KPA]
        WHERE [UUID] = @KPAUUID

        --output result
        EXEC [Goals].[spTempTable_StatusTable_Save] @TrackingUUID = @TrackingUUIDNew,
                                                    @ProcedureName = @ProcedureName,
                                                    @UUID = @KPAUUID,
                                                    @isSuccessful = 1,
                                                    @Message = 'KPA successfully deleted.';

         --Log that data has been inserted and by who 
        EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                             @UsersIdLoggedIn = @UsersIdLoggedIn,
                                             @TableName = 'KPA',
                                             @TableNameId = @TableNameidKPA,
                                             @TableId = @KPAid, 
                                             @isDelete = 1;

        --get all the KPI's linked to the KPA and delete them 
        --check that kpi is only connected to the user who KPA was deleted
        --check that Kpi is not linked to other KPA then delete it
        DECLARE @KPAKPIid INT
        , @KPIUUID NVARCHAR(200);

        DECLARE KPI_Cursor CURSOR 
        FOR 
        SELECT k.[Id] [KPAKPIid]
        , CASE WHEN i.[Usersid] = @Usersid THEN i.[UUID] END [KPIid]
        FROM [Goals].[KPAKPI] k
        INNER JOIN [Goals].[KPI] i ON i.[Id] = k.[KPIid] AND i.[isDeleted] = 0
        WHERE k.[isDeleted] = 0 AND k.[KPAid] = @KPAid


        OPEN KPI_Cursor;

        -- Fetch the first row
        FETCH NEXT FROM KPI_Cursor INTO @KPAKPIid, @KPIUUID;

        -- Loop through the cursor
        WHILE @@FETCH_STATUS = 0
        BEGIN

            -- Delete link between KPA and KPI
            UPDATE [Goals].[KPAKPI]
            SET [isDeleted] = 1
            WHERE [Id] = @KPAKPIid

            IF @KPIUUID IS NOT NULL
            BEGIN

                EXEC [Goals].[spKPI_Delete] @CompanyUUID = @CompanyUUID
                                          , @UsersUUIDLoggedIn = @UsersUUIDLoggedIn 
                                          , @KPIUUID = @KPIUUID 
                                          , @TrackingUUID = @TrackingUUIDNew
                                          , @isDebug = @isDebug

            END


            -- Fetch the next row
            FETCH NEXT FROM KPI_Cursor INTO @KPAKPIid, @KPIUUID;
        END;

        -- Close and deallocate the cursor
        CLOSE KPI_Cursor;
        DEALLOCATE KPI_Cursor;




    END
    ELSE
    BEGIN
        EXEC [Goals].[spTempTable_StatusTable_Save] @TrackingUUID = @TrackingUUIDNew,
                                                    @ProcedureName = @ProcedureName,
                                                    @UUID = @KPAUUID,
                                                    @isSuccessful = 0,
                                                    @Message = 'Failed to delete, Please supply a valid KPA UUID.';
    END


    EXEC [Goals].[spTempTable_StatusTable_Display] @TrackingUUID = @TrackingUUID
END 
