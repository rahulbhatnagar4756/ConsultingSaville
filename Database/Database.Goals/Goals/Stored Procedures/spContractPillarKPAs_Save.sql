
CREATE   PROCEDURE [Goals].[spContractPillarKPAs_Save]
(  
    @Companyid int,
    @UsersidLoggedIn int,
    @UUID NVARCHAR(200) = NULL,
    @ContractPillarsid INT,
    @KPAId INT,
    @Weight DECIMAL(18,4) = 100.00,
    @isSuccessful BIT = 0 OUTPUT,
    @Message NVARCHAR(max) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ContractPillarKPAId INT;  
    DECLARE @ResultUUID NVARCHAR(200); 
    SET @Message = 'Contract Pillar KPA saved successfully.';
    
    BEGIN TRY
        
        -- Validate Weight
        IF @Weight < 0 
        BEGIN
            SET @isSuccessful = 0;
            SET @Message = 'Weight must be between larger than 0.';
            RETURN 0;
        END
        
        IF @UUID IS NULL
        BEGIN
            SELECT @UUID = UUID 
            FROM [Goals].[ContractPillarKPAs] 
            WHERE [ContractPillarsid] = @ContractPillarsId
                AND [KPAid] = @KPAId
                AND isDeleted = 0;
        END

        BEGIN TRANSACTION;
        -- Check if updating existing record
        IF @UUID IS NOT NULL AND @UUID != ''
        BEGIN
            SELECT @ContractPillarKPAId = Id 
            FROM [Goals].[ContractPillarKPAs] 
            WHERE UUID = @UUID 
                AND isDeleted = 0;
            
            IF @ContractPillarKPAId IS NOT NULL
            BEGIN
                -- Update existing record
                UPDATE [Goals].[ContractPillarKPAs]
                SET [Weight] = @Weight
                WHERE Id = @ContractPillarKPAId;
                
                SET @ResultUUID = @UUID;
            END
            ELSE
            BEGIN
                ROLLBACK TRANSACTION;
                SET @isSuccessful = 0;
                SET @Message = 'Contract Pillar KPA not found for update.';
                RETURN 0;
            END
        END
        ELSE
        BEGIN
            
            INSERT INTO [Goals].[ContractPillarKPAs] 
            ( 
                [ContractPillarsid],
                [KPAid],
                [Weight]
            )
            VALUES 
            ( 
                @ContractPillarsId,
                @KPAId,
                @Weight
            );
        END
        
        SET @isSuccessful = 1;
        SET @Message = 'Successfully linked contract pillar to KPA.';

        COMMIT TRANSACTION;
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @isSuccessful = 0;
        SET @Message = 'Error saving Contract Pillar KPA: ' + ERROR_MESSAGE();
        SET @ResultUUID = NULL;
    END CATCH
    
    ErrorHandler:
       
END