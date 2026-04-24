CREATE PROCEDURE [Goals].[spContractPillarKPAs_Save]
  @CompanyUUID NVARCHAR(200)
, @UsersUUIDLoggedIn NVARCHAR(200) 
, @KPAUUID NVARCHAR(200) = null
, @ContractPillarsUUID NVARCHAR(200)
, @ContractPillarKPAsUUID NVARCHAR(200) = NULL 
, @Weights float
, @TrackingUUID NVARCHAR(200) = NULL
, @isDebug bit = 0
AS
BEGIN

    SET NOCOUNT ON;

	EXEC [Goals].[spTempTable_StatusTable] @TrackingUUID

    DECLARE @CompanyId int = 0
    , @KPAid int = 0
    , @UsersidLoggedIn int = 0
    , @TableNameidContractPillarKPAs int = 12
    , @ContractPillarKPAsid int
    , @ContractPillarsid int = 0
    , @isValid bit = 0
    , @ProcedureName NVARCHAR(255)

    DECLARE @TrackingUUIDNew NVARCHAR(200) = COALESCE(@TrackingUUID, NEWID());
    SET @ProcedureName = OBJECT_NAME(@@PROCID);

    SELECT @UsersidLoggedIn = [Recordid]
    FROM [dbo].[users] u
    WHERE u.[UUID] = @UsersUUIDLoggedIn

    SELECT @CompanyId = [Recordid]
    FROM [dbo].[Companies] c
    WHERE c.[UUID] = @CompanyUUID 

    SELECT @KPAid = [id]
    FROM [Goals].[KPA] k
    WHERE k.[UUID] = @KPAUUID OR k.[Id] = @KPAid

    SELECT @ContractPillarsid = p.[Id]
    FROM [Goals].[ContractPillars] p
    WHERE p.[UUID] = @ContractPillarsUUID



    IF (NOT EXISTS(SELECT *
                   FROM [Goals].[ContractPillarKPAs] pk
                   INNER JOIN [Goals].[ContractPillars] p ON p.[Id] = pk.[ContractPillarsid] AND p.[UUID] = @ContractPillarsUUID
                   INNER JOIN [Goals].[KPA] k ON k.[Id] = pk.[KPAid] AND k.[UUID] = @KPAUUID
                   WHERE pk.[isDeleted] = 0)) OR @ContractPillarKPAsUUID IS NULL
    BEGIN

        --insert link the KPA to the Pillar
        INSERT INTO [Goals].[ContractPillarKPAs] ([ContractPillarsid], [KPAid], [Weight])
        VALUES (@ContractPillarsid, @KPAid, @Weights)
        SET @ContractPillarKPAsid = SCOPE_IDENTITY()

        --Log that data has been inserted and by who 
        EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                             @UsersIdLoggedIn = @UsersIdLoggedIn,
                                             @TableName  = 'PillarKPAs',
                                             @TableNameid = @TableNameidContractPillarKPAs,
                                             @TableId = @ContractPillarKPAsid,
                                             @isInsert = 1

    END
    ELSE
    BEGIN
        DECLARE @WeightsCurrent float
         
        IF(@ContractPillarKPAsUUID IS NULL)
        BEGIN
            SELECT @isValid = CASE WHEN pk.[Weight] = @Weights THEN 0 ELSE 1 END
            , @WeightsCurrent = CASE WHEN pk.[Weight] <> @Weights THEN pk.[Weight] END
            , @ContractPillarKPAsid = pk.[Id]
            FROM [Goals].[ContractPillarKPAs] pk
            INNER JOIN [Goals].[ContractPillars] p ON p.[Id] = pk.[ContractPillarsid] AND p.[UUID] = @ContractPillarsUUID
            INNER JOIN [Goals].[KPA] k ON k.[Id] = pk.[KPAid] AND k.[UUID] = @KPAUUID
            WHERE pk.[isDeleted] = 0
        END
        ELSE
        BEGIN
            SELECT @isValid = CASE WHEN pk.[Weight] = @Weights THEN 0 ELSE 1 END
            , @WeightsCurrent = CASE WHEN pk.[Weight] <> @Weights THEN pk.[Weight] END
            , @ContractPillarKPAsid = pk.[Id]
            FROM [Goals].[ContractPillarKPAs] pk
            WHERE pk.[UUID] = @ContractPillarKPAsUUID
        END

        IF @isValid = 1
        BEGIN

            UPDATE pk
            SET pk.[Weight] = @Weights
            FROM [Goals].[ContractPillarKPAs] pk
            WHERE pk.[Id] = @ContractPillarKPAsid

            IF NOT @WeightsCurrent IS NULL
            BEGIN
                EXEC [Goals].[spLogDataChanges_Save] @CompanyId = @companyid,
                                                     @UsersIdLoggedIn = @UsersIdLoggedIn,
                                                     @TableName  = 'PillarKPAs',
                                                     @TableNameid = @TableNameidContractPillarKPAs,
                                                     @TableId = @ContractPillarKPAsid,
                                                     @ColumnName = 'Weight',
                                                     @OldValue = @WeightsCurrent,
                                                     @NewValue = @Weights,
                                                     @isUpdate = 1
            
            END

        END
    END

    EXEC [Goals].[spTempTable_Display] @TrackingUUID;
END
