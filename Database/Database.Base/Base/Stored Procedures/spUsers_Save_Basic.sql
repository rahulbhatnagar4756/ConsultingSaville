
CREATE PROCEDURE [base].[spUsers_Save_Basic]
	  @CompanyUUID	NVARCHAR(200)
	, @UUID			NVARCHAR(200) = null OUTPUT 
	, @FirstName	NVARCHAR(200)
	, @LastName		NVARCHAR(200)
	, @IDNumber		NVARCHAR(200)
	, @Email		NVARCHAR(200)
	, @Mobile		NVARCHAR(200)
	, @Gender		NVARCHAR(200)
	, @Ethnicity	NVARCHAR(200)
	, @DateOfBirth	DATETIME 
	, @isSuccess	BIT OUTPUT
    , @Message		NVARCHAR(MAX) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @Usersid INT = 0

	--make sure company exists
	IF NOT EXISTS (SELECT 1 
				   FROM [dbo].[Companies] 
				   WHERE [UUID] = @CompanyUUID 
					AND [isDeleted] = 0)
	BEGIN
		SET @Message = 'Company does not exist or is deleted';
		SET @isSuccess = 0;
		RETURN 0;
	END;

	--check if the user exist idnumber or email
	IF LEN(ISNULL(@UUID,'')) = 0
	BEGIN
		SELECT @Usersid = [Recordid]
		, @UUID = [UUID]
		FROM [dbo].[Users] 
		WHERE (IDNumber = @IDNumber 
			   OR Email = @Email) 
	END
	

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
		, [isDeleted] = 0
		WHERE [UUID] = @UUID

		SET @isSuccess = 1;
		SET @Message = 'User updated successfully';
		RETURN 1;
	END
	ELSE
	BEGIN

		INSERT INTO [dbo].[Users] ([FirstName], [LastName], [IDNumber], [Email], [Mobile], [Gender], [Race], [DateOfBirth])
		VALUES (@FirstName, @LastName, @IDNumber, @Email, @Mobile, @Gender, @Ethnicity, @DateOfBirth)
		SELECT @Usersid = SCOPE_IDENTITY();

		SELECT @UUID = [UUID]
		FROM [dbo].[users] u
		WHERE u.[recordid] = @Usersid;

		SET @isSuccess = 1;
		SET @Message = 'User created successfully';
		RETURN 1;
	END

	 


END