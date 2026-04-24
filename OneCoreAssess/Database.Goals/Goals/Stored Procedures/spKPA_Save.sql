--to save a KPA
CREATE PROCEDURE [Goals].[spKPA_Save]
  @CompanyUUID NVARCHAR(200)
, @UsersUUIDLoggedIn NVARCHAR(200)
, @UsersUUID NVARCHAR(200)
, @KPAUUID NVARCHAR(200) = null
, @ContractPillarsUUID NVARCHAR(200)
, @ContractPillarKPAUUID NVARCHAR(200)
, @EnterpriseStructureTypesUUID NVARCHAR(200)
, @Name NVARCHAR(500)
, @Description NVARCHAR(MAX)
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
    , @KPAid int = 0
    , @UsersidLoggedIn int = 0
    , @TableNameidKPA int = 8
    , @TableNameidPillarKPAs int = 12

    SELECT @Usersid = [Recordid]
    FROM [dbo].[users] u
    WHERE u.[UUID] = @UsersUUID
    
    SELECT @UsersidLoggedIn = [Recordid]
    FROM [dbo].[users] u
    WHERE u.[UUID] = @UsersUUIDLoggedIn

    SELECT @CompanyId = [Recordid]
    FROM [dbo].[Companies] c
    WHERE c.[UUID] = @CompanyUUID 

    IF @KPAUUID IS NULL
    BEGIN

        INSERT INTO [Goals].[KPA] ([CompanyId], [Usersid], [Name], [Description])
        VALUES(@CompanyId, @Usersid, @Name, @Description)
        SET @KPAid = SCOPE_IDENTITY();

        SELECT @KPAUUID = [UUID]
        FROM [Goals].[KPA]
        WHERE [Id] = @KPAid

        --output result
        EXEC [Goals].[spTempTable_StatusTable_Save] @TrackingUUID = @TrackingUUIDNew,
                                                    @ProcedureName = @ProcedureName,
                                                    @UUID = @KPAUUID,
                                                    @isSuccessful = 1,
                                                    @Message = 'KPA successfully saved.';

         --Log that data has been inserted and by who 
        EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                             @UsersIdLoggedIn = @UsersIdLoggedIn,
                                             @TableName  = 'KPA',
                                             @TableNameid = @TableNameidKPA,
                                             @TableId = @KPAid, 
                                             @isInsert = 1;

    END
    ELSE
    BEGIN
        DECLARE @NameCurrent NVARCHAR(500)
        , @DescriptionCurrent NVARCHAR(MAX)


        SELECT @isValid = CASE WHEN ISNULL(k.[Name],'') <> ISNULL(@Name,'') OR ISNULL(k.[Description],'') <> ISNULL(@Description,'')  THEN 1 ELSE 0 END
        , @NameCurrent = CASE WHEN ISNULL(k.[Name],'') <> ISNULL(@Name,'') THEN k.[Name] END
        , @DescriptionCurrent = CASE WHEN ISNULL(k.[Description],'') <> ISNULL(@Description,'') THEN k.[Description] END
        , @KPAid = k.[Id]
        FROM [Goals].[KPA] k
        WHERE k.[isDeleted] = 0 AND k.[UUID] = @KPAUUID
        
        IF @isValid = 1
        BEGIN

            UPDATE [Goals].[KPA]
            SET  [Name] = @Name
               , [Description] = @Description
            WHERE [UUID] = @KPAUUID

            --Result Status Update
            EXEC [Goals].[spTempTable_StatusTable_Save] @TrackingUUID = @TrackingUUIDNew,
                                                        @ProcedureName = @ProcedureName,
                                                        @UUID = @KPAUUID,
                                                        @isSuccessful = 1,
                                                        @Message = 'KPA successfully updated.';

            --Log that data has been updated and by who 
            IF NOT @NameCurrent IS NULL
             BEGIN
                EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                                     @UsersIdLoggedIn = @UsersIdLoggedIn,
                                                     @TableName  = 'KPA',
                                                     @TableNameid = @TableNameidKPA,
                                                     @TableId = @KPAid, 
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
                                                     @TableName  = 'KPA',
                                                     @TableNameid = @TableNameidKPA,
                                                     @TableId = @KPAid, 
                                                     @ColumnName = 'Description',
                                                     @OldValue = @DescriptionCurrent,
                                                     @NewValue = @Description,
                                                     @isUpdate = 1;
            END

        END

    END

    --check if link between pillars and kpa exist
    EXEC [Goals].[spContractPillarKPAs_Save]  @CompanyUUID = @CompanyUUID 
                                            , @UsersUUIDLoggedIn = @UsersUUIDLoggedIn 
                                            , @KPAUUID = @KPAUUID
                                            , @ContractPillarsUUID = @ContractPillarsUUID
                                            , @ContractPillarKPAsUUID = @ContractPillarKPAUUID
                                            , @Weights = @Weights
                                            , @TrackingUUID = @TrackingUUIDNew
                                            , @isDebug = @isDebug
     

    EXEC [Goals].[spTempTable_StatusTable_Display] @TrackingUUID = @TrackingUUID

END
