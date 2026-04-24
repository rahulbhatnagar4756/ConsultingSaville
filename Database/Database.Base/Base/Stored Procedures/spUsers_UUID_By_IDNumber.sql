CREATE PROCEDURE [base].[spUsers_UUID_By_IDNumber]
	@IDNumber NVARCHAR(200)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT [UUID]
	FROM [dbo].[Users]
	WHERE [IDNumber] = @IDNumber
		 
	RETURN 0
END
