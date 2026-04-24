CREATE PROCEDURE [Goals].[spContracts_Users_Latest]
  @CompanyUUID NVARCHAR(200) 
, @UsersUUIDLoggedIn NVARCHAR(200) 
, @UsersUUID NVARCHAR(200) 
, @ContractsUUID NVARCHAR(200) OUTPUT 
, @isSuccessful BIT = 0 OUTPUT
, @Message NVARCHAR(MAX) OUTPUT
AS
BEGIN

    DECLARE
      @CompanyId INT = null
    , @Usersid INT = null
    , @ContractPeriodsid INT = null
    , @ContractPeriodsDateStart DATE = null
    , @ContractPeriodsDateEnd DATE = null
    
    --GET THE latest contract for teh user

    SELECT @CompanyId = [recordID] 
    FROM [Base].[Companies] 
    WHERE UUID = @CompanyUUID 
        AND isDeleted = 0;

    IF @CompanyId IS NULL
    BEGIN
        SET @Message = 'Invalid Company.';
        RETURN 0;
    END
        
    -- Validate and get UsersId
    SELECT @Usersid = Id 
    FROM [Base].[Users] 
    WHERE UUID = @UsersUUID 
        AND isDeleted = 0;

    IF @Usersid IS NULL
    BEGIN
        SET @Message = 'Invalid User.';
        RETURN 0;
    END

    SELECT @ContractPeriodsid = cp.[Id]
    , @ContractPeriodsDateStart = cp.[DateStart]
    , @ContractPeriodsDateEnd = cp.[DateEnd]
    FROM [Goals].[ContractPeriods] cp
    WHERE cp.[isActive] = 1
        AND cp.[Year] = YEAR(GETDATE())
        AND cp.[Companyid] = @CompanyId;

    IF @ContractPeriodsid IS NULL
    BEGIN
        SET @Message = 'No Active Contract Periods.';
        RETURN 0;
    END

    ;WITH wh_Contract AS (
                            SELECT c.[Id]
                            , c.[UUID] 
                            , c.ContractPeriodsid
                            , c.[DateCreated]
                            , c.[DateStart]
                            , c.[DateEnd] 
                            , c.[isActive]
                            , c.[isDeleted]
                            FROM [Goals].[Contracts] c
                            WHERE c.[isActive] = 1
                                AND c.[isDeleted] = 0
                                AND c.Companyid = @CompanyId  
                                AND c.[Usersid] = @Usersid
                          )
    SELECT @ContractsUUID = c.[UUID]
    FROM [Goals].[ContractPeriods] cp
    LEFT OUTER JOIN [wh_Contract] c ON c.[ContractPeriodsid] = cp.[Id]
    WHERE cp.[id] = @ContractPeriodsid;

    IF @ContractsUUID IS NULL
    BEGIN

        SET @ContractsUUID = NEWID();


        INSERT INTO [Goals].[Contracts]
           ([UUID]
           ,[Companyid]
           ,[Usersid]
           ,[ContractPeriodsid] 
           ,[DateStart]
           ,[DateEnd]
           ,[isActive]
           ,[isDeleted])
        VALUES
           (@ContractsUUID
           , @Companyid
           , @Usersid
           , @ContractPeriodsid
           , @ContractPeriodsDateStart
           , @ContractPeriodsDateEnd
           , 1
           , 0)
           
           SET @isSuccessful = 1;
           SET @Message = 'Successfully.';
    END

      
END