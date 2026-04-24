CREATE PROCEDURE [Goals].[spTempTable_StatusTable]
    @TrackingUUID NVARCHAR(200) = NULL
    , @isDebug bit = 0
AS
BEGIN

    SET NOCOUNT ON;

     DECLARE @ProcedureName NVARCHAR(255)
     SET @ProcedureName = OBJECT_NAME(@@PROCID);

    IF OBJECT_ID('tempdb..#StatusTable') IS NULL
    BEGIN
        CREATE TABLE #StatusTable (
            [Id] INT IDENTITY(1,1) PRIMARY KEY,  
            [TrackingUUID] NVARCHAR(200),
            [UUID] NVARCHAR(200),
            [Procedure] NVARCHAR(200),
            [isSuccessful] BIT,
            [Message] NVARCHAR(500),
            [Code] NVARCHAR(50),
            [RowsAffected] INT,
            [isDebug] BIT default 0 
        );

        IF @isDebug = 1
        BEGIN
            INSERT INTO #StatusTable(TrackingUUID, [Procedure], [UUID], [isSuccessful], [Message], [RowsAffected])
            VALUES(@TrackingUUID, @ProcedureName, NULL, 1, 'Successfully setup Temp Table Status table', @@ROWCOUNT);
        END
    END
	ELSE
	BEGIN
		
		IF @TrackingUUID IS NULL
		BEGIN
			TRUNCATE TABLE #StatusTable;
			DBCC CHECKIDENT ('#StatusTable', RESEED, 0);
		END
	END
END
