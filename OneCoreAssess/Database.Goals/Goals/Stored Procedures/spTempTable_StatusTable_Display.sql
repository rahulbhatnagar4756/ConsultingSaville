CREATE PROCEDURE [Goals].[spTempTable_StatusTable_Display]
    @TrackingUUID NVARCHAR(200) = NULL
AS
BEGIN

    IF @TrackingUUID IS NULL
        SELECT *
        FROM #StatusTable;

END
