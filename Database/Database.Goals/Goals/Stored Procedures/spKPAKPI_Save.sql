
CREATE PROCEDURE [Goals].[spKPAKPI_Save]
 @KPIid INT 
, @KPAid INT
, @Weight DECIMAL(18,4)
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @KPAKPIid INT = 0
    , @isWeightDiff bit = 1


    SELECT @KPAKPIid = [Id]
    , @isWeightDiff = CASE WHEN kk.[Weight] = @Weight THEN 0 ELSE 1 END
    FROM [Goals].[KPAKPI] kk
    WHERE kk.[KPAid] = @KPAid
        AND kk.[KPIid] = @KPIid
        AND kk.[isDeleted] = 0;


    IF ISNULL(@KPAKPIid, 0) > 0 
    BEGIN
        IF @isWeightDiff = 1
        BEGIN
            UPDATE [Goals].[KPAKPI]
            SET [Weight] = @Weight
            WHERE [id] = @KPAKPIid;
        END
        RETURN 1;
    END
    ELSE
    BEGIN

 
        INSERT INTO [Goals].[KPAKPI]
           ( 
             [KPAid]
           , [KPIid]
           , [Weight] 
           )
        VALUES
           ( 
             @KPAid
           , @KPIid
           , @Weight 
           );
        RETURN 1;

    END

END