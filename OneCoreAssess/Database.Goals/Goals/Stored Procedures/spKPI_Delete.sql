CREATE PROCEDURE [Goals].[spKPI_Delete]
  @CompanyUUID NVARCHAR(200)
, @UsersUUIDLoggedIn NVARCHAR(200) 
, @KPIUUID NVARCHAR(200) = null 
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
    , @TableNameidKPI int = 5
    
    SELECT @UsersidLoggedIn = [Recordid]
    FROM [dbo].[users] u
    WHERE u.[UUID] = @UsersUUIDLoggedIn

    SELECT @CompanyId = [Recordid]
    FROM [dbo].[Companies] c
    WHERE c.[UUID] = @CompanyUUID 

    IF @KPIUUID IS NOT NULL
    BEGIN

        UPDATE [Goals].[KPI] 
        SET [isDeleted] = 0
        , [DateEnd] = COALESCE([DateEnd], GETUTCDATE())
        WHERE [UUID] = @KPIUUID 

        SELECT @KPAid = [Id] 
        , @Usersid = [Usersid]
        FROM [Goals].[KPA]
        WHERE [UUID] = @KPIUUID

         --output result
        EXEC [Goals].[spTempTable_StatusTable_Save] @TrackingUUID = @TrackingUUIDNew,
                                                    @ProcedureName = @ProcedureName,
                                                    @UUID = @KPIUUID,
                                                    @isSuccessful = 1,
                                                    @Message = 'KPA successfully deleted.';

         --Log that data has been inserted and by who 
        EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                             @UsersIdLoggedIn = @UsersIdLoggedIn,
                                             @TableName  = 'KPI',
                                             @TableNameId = @TableNameidKPI,
                                             @TableId = @KPAid, 
                                             @isDelete = 1;


        UPDATE [Goals].[KPAKPI]
        SET [isDeleted] = 1
        WHERE [KPIid] = @KPAid AND [isDeleted] = 0;

        UPDATE [Goals].[KPILinks]
        SET [isDeleted] = 1
        WHERE [KPIid] = @KPAid AND [isDeleted] = 0;

    END

END
