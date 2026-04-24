CREATE PROCEDURE [Goals].[spContracts_Save]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedin NVARCHAR(200),
    @ContractPeriodsUUID NVARCHAR(200),
    @DateStart DATETIME = null,
    @ContractsUUID NVARCHAR(200) = null,
    @UsersUUID NVARCHAR(200) = null,
    @TemplatesUUID NVARCHAR(200) = null, 
    @DateEnd DATETIME = null,
    @isDepartmentTemplateSync BIT = 1,
    @isIndividualTemplateSync BIT = 0,
    @TrackingUUID NVARCHAR(200) = null,
    @isDebug BIT = 0 
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Companyid INT
    , @Usersid INT
    , @Templatesid INT
    , @ContractPeriodsid INT
    , @ExistingContractUUID NVARCHAR(200)
    , @isSuccess BIT = 0
    , @MessageCode NVARCHAR(50)
    , @Message NVARCHAR(255)
    , @isActiveContract bit = NULL
    , @isActivePeriod bit = NULL
    , @isActiveTemplate bit = NULL
    , @Contractsid int
    , @ProcedureName NVARCHAR(255);

    IF @isDepartmentTemplateSync IS NULL
        SET @isDepartmentTemplateSync = 1;

    IF @isIndividualTemplateSync IS NULL
        SET @isIndividualTemplateSync = 0;

    DECLARE @TrackingUUIDNew NVARCHAR(200) = COALESCE(@TrackingUUID, NEWID());
    SET @ProcedureName = OBJECT_NAME(@@PROCID);

    EXEC [Goals].[spTempTable_StatusTable] @TrackingUUID

    SELECT @ContractPeriodsid = [Id] 
    , @DateStart = ISNULL(@DateStart, p.[DateStart])
    , @DateEnd = CASE WHEN @DateStart IS NULL THEN p.[DateEnd] END
    FROM [Goals].[ContractPeriods] p
    WHERE p.[UUID] = @ContractPeriodsUUID;


    -- Resolve UUIDs to IDs
    SELECT @Companyid = [Recordid] 
    FROM [dbo].[Companies] 
    WHERE UUID = @CompanyUUID;
    
    SELECT @Usersid = [Recordid] 
    FROM [dbo].[Users] 
    WHERE UUID = @UsersUUID;

    SELECT @Templatesid = [Id] 
    FROM [Goals].[Templates] 
    WHERE UUID = @TemplatesUUID;

     
    
    -- Check if contract already exists for user in this period
    SELECT @ExistingContractUUID = UUID 
    , @isActiveContract = [isActive]
    FROM [Goals].[Contracts]
    WHERE Usersid = @Usersid 
        AND ContractPeriodsid = @ContractPeriodsid
        AND [isDeleted] = 0;
    
   
   IF @ExistingContractUUID IS NOT NULL AND @ContractsUUID IS NULL
    BEGIN 
        SET @Message = 'A contract already exists for this user in the selected period.';
        SET @isSuccess = 1;
        
        EXEC [Goals].[spTempTable_StatusTable_Save] @TrackingUUID = @TrackingUUIDNew,
                                                    @ProcedureName = @ProcedureName,
                                                    @UUID = @ExistingContractUUID,
                                                    @isSuccessful = @isSuccess,
                                                    @Message = @Message;

        EXEC [Goals].[spTempTable_StatusTable_Display] @TrackingUUID = @TrackingUUID                                                        
        RETURN 1;
    END

    ------------------------------------------------------------------------------
    --     Contract period is no longer active
    ------------------------------------------------------------------------------
    IF ISNULL(@isActiveContract, 1) = 0
    BEGIN
        SET @Message = 'Contract period is no longer active. No alteration can be made';

         EXEC [Goals].[spTempTable_StatusTable_Save] @TrackingUUID = @TrackingUUIDNew,
                                                    @ProcedureName = @ProcedureName,
                                                    @UUID = @ExistingContractUUID,
                                                    @isSuccessful = @isSuccess,
                                                    @Message = @Message;
        
        EXEC [Goals].[spTempTable_StatusTable_Display] @TrackingUUID = @TrackingUUID                                                        

        RETURN 1;
    END
   
    ------------------------------------------------------------------------------
    --     Check if contract already exists for template in this period
    ------------------------------------------------------------------------------

    SELECT @ExistingContractUUID = c.[UUID] 
    , @isActiveContract = c.[isActive]
    FROM [Goals].[Contracts] c
    WHERE [Templatesid] = @Templatesid 
        AND [ContractPeriodsid] = @ContractPeriodsid 
        AND [isDeleted] = 0;
    
    IF @ExistingContractUUID IS NOT NULL AND @ContractsUUID IS NULL
    BEGIN
        SET @Message = 'A contract already exists for this template in the selected period.';
        SET @isSuccess = 1;

         EXEC [Goals].[spTempTable_StatusTable_Save] @TrackingUUID = @TrackingUUIDNew,
                                                    @ProcedureName = @ProcedureName,
                                                    @UUID = @ExistingContractUUID,
                                                    @isSuccessful = @isSuccess,
                                                    @Message = @Message;
        
        EXEC [Goals].[spTempTable_StatusTable_Display] @TrackingUUID = @TrackingUUID     

        RETURN 1;
    END
    
    ------------------------------------------------------------------------------
    --     Contract period is no longer active
    ------------------------------------------------------------------------------

    IF ISNULL(@isActiveContract, 1) = 0
    BEGIN
        SET @Message = 'Contract period is no longer active. No alteration can be made';
        SET @isSuccess = 1;

        EXEC [Goals].[spTempTable_StatusTable_Save] @TrackingUUID = @TrackingUUIDNew,
                                                    @ProcedureName = @ProcedureName,
                                                    @UUID = @ExistingContractUUID,
                                                    @isSuccessful = @isSuccess,
                                                    @Message = @Message,
                                                    @RowsAffected = @@ROWCOUNT;
        
        EXEC [Goals].[spTempTable_StatusTable_Display] @TrackingUUID = @TrackingUUID     

        RETURN 1;
    END

    ------------------------------------------------------------------------------
    --     Save the Contract 
    ------------------------------------------------------------------------------

    IF @ContractsUUID IS NULL
    BEGIN
        -- Insert new contract
        INSERT INTO [Goals].[Contracts] (
            Companyid, Usersid, Templatesid, ContractPeriodsid,
            DateStart, DateEnd, isDepartmentTemplateSync, isIndividualTemplateSync
        )
        VALUES (
            @Companyid, @Usersid, @Templatesid, @ContractPeriodsid,
            @DateStart, @DateEnd, @isDepartmentTemplateSync, @isIndividualTemplateSync
        );
        
        -- Get new UUID
        SET @Contractsid = SCOPE_IDENTITY();

        SELECT @ContractsUUID = [UUID]
        FROM [Goals].[Contracts] c
        WHERE c.[Id] = @Contractsid;

        SET @isSuccess = 1;
        SET @Message = 'Contract created successfully.';
        
        EXEC [Goals].[spTempTable_StatusTable_Save] @TrackingUUID = @TrackingUUIDNew,
                                                    @ProcedureName = @ProcedureName,
                                                    @UUID = @ExistingContractUUID,
                                                    @isSuccessful = @isSuccess,
                                                    @Message = @Message,
                                                    @RowsAffected = @@ROWCOUNT;
    END
    ELSE
    BEGIN

        ------------------------------------------------------------------------------
        --     Update existing contract
        ------------------------------------------------------------------------------
        UPDATE [Goals].[Contracts]
        SET DateStart = @DateStart,
            DateEnd = @DateEnd,
            isDepartmentTemplateSync = @isDepartmentTemplateSync,
            isIndividualTemplateSync = @isIndividualTemplateSync
        WHERE UUID = @ContractsUUID;
        
        SET @isSuccess = 1;
        SET @Message = 'Contract updated successfully.';
        
        EXEC [Goals].[spTempTable_StatusTable_Save] @TrackingUUID = @TrackingUUIDNew,
                                                    @ProcedureName = @ProcedureName,
                                                    @UUID = @ExistingContractUUID,
                                                    @isSuccessful = @isSuccess,
                                                    @Message = @Message,
                                                    @RowsAffected = @@ROWCOUNT;
    END
    

    ------------------------------------------------------------------------------
    --     Setup Contract Pillars for Company Department and Individual
    ------------------------------------------------------------------------------
    IF @Contractsid IS NOT NULL
    BEGIN
        INSERT INTO [Goals].[ContractPillars] ([Contractid], [Pillarsid], [EnterpriseStructureTypesid] )
        SELECT @Contractsid, p.[Id], e.[Id] 
        FROM [Goals].[Goals].[Pillars] p        
        INNER JOIN [Goals].[EnterpriseStructureTypes] e ON e.[Companyid] = p.[Companyid] AND e.[isDeleted] = 0
        
        LEFT OUTER JOIN [Goals].[ContractPillars] cp ON cp.[Contractid] = @Contractsid 
            AND cp.[Pillarsid] = p.[Id]
            AND cp.[EnterpriseStructureTypesid] = e.[Id] 

        WHERE p.[isDeleted] = 0 AND p.[Companyid] = @Companyid AND cp.[Id] IS NULL


        INSERT INTO [Goals].[ContractEnterpriseStructureTypes]([Contractsid], [EnterpriseStructureTypesid])
        
        SELECT @Contractsid, e.[id]
        FROM [Goals].[EnterpriseStructureTypes] e 
        
        LEFT OUTER JOIN [Goals].[ContractEnterpriseStructureTypes] cp ON cp.[Contractsid] = @Contractsid 
            AND cp.[EnterpriseStructureTypesid] = e.[Id] 

        WHERE e.[Companyid] = @Companyid AND e.[isDeleted] = 0

    END

    -- Return output
    EXEC [Goals].[spTempTable_StatusTable_Display] @TrackingUUID = @TrackingUUID;

    RETURN 1
END
