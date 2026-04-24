CREATE PROCEDURE [base].[spUsers_Save_UUID]
	  @CompanyUUID NVARCHAR(200)
	, @UsersUUIDLoggedIn NVARCHAR(200)
	, @UUID NVARCHAR(200) = NULL
	, @FirstName NVARCHAR(200)
	, @LastName NVARCHAR(200)
	, @IDNumber NVARCHAR(200)
	, @Email NVARCHAR(200)
	, @Mobile NVARCHAR(200)
	, @Gender NVARCHAR(200)
	, @Ethnicity NVARCHAR(200)
	, @DateOfBirth DATETIME
	, @UsersImageURL NVARCHAR(500)
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @Usersid INT = 0

	IF EXISTS (SELECT * 
			   FROM [dbo].[Users] 
			   WHERE UUID = @UUID)
	BEGIN

		UPDATE [dbo].[Users]
		SET [FirstName] = @FirstName
		, [LastName] = @LastName
		, [IDNumber] = @IDNumber
		, [Email] = @Email
		, [Mobile] = @Mobile
		, [Gender] = @Gender
		, [Race] = @Ethnicity
		, [DateOfBirth] = @DateOfBirth
		WHERE [UUID] = @UUID

	END
	ELSE
	BEGIN

		INSERT INTO [dbo].[Users] ([FirstName], [LastName], [IDNumber], [Email], [Mobile], [Gender], [Race], [DateOfBirth])
		VALUES (@FirstName, @LastName, @IDNumber, @Email, @Mobile, @Gender, @Ethnicity, @DateOfBirth)
		SELECT @Usersid = SCOPE_IDENTITY();

		SELECT TOP(1) @UUID = UUID
		FROM [dbo].[users] u
		WHERE u.[recordid] = @Usersid 

	END

	--poulate the resultmodel UUID, isValid, Message
	SELECT TOP(1) @UUID AS UUID
	, 1 AS isValid
	, 'Successfully saved users information' [Message]
	 


END
