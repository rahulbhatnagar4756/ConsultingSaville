

CREATE PROCEDURE [Goals].[spTempTable_StatusTable_Save]
    @TrackingUUID NVARCHAR(200),
    @ProcedureName NVARCHAR(200),
    @UUID NVARCHAR(200),
    @isSuccessful BIT,
    @Message NVARCHAR(500),
    @Code NVARCHAR(50) = NULL,  -- Optional parameter
    @RowsAffected INT = NULL,   -- Optional parameter
    @isDebug BIT = 0            -- Default value 0
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert into the temporary table
    INSERT INTO #StatusTable (TrackingUUID, [Procedure], [UUID], [isSuccessful], [Message], [Code], [RowsAffected], isDebug)
    VALUES (@TrackingUUID, @ProcedureName, @UUID, @isSuccessful, @Message, @Code, @RowsAffected, @isDebug);

END;
