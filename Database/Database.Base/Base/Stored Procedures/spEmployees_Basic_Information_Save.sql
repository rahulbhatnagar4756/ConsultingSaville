CREATE PROCEDURE [base].[spEmployees_Basic_Information_Save]
	@UserCompanyUUID NVARCHAR(50) = NULL,
	@CompanyUUID NVARCHAR(50),
	@UsersUUIDLoggedIn NVARCHAR(200),
	@UsersUUID NVARCHAR(50),
	@EmployeeNumber NVARCHAR(50) = NULL,
	@DateOfEmployment DATETIME = NULL,
	@PhoneNumber NVARCHAR(50) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @isEmployee bit = 1
	, @UserCompanyid int = 0

	--if @UserCompanyUUID is null then check if you can find @UserCompanyUUID from table [Base].[UsersCompanys] using companyUUID and usersUUID
	IF @UserCompanyUUID IS NULL
	BEGIN
		SELECT @UserCompanyUUID = uc.[UUID]
		FROM [Base].[UsersCompanys] uc
		INNER JOIN [dbo].[Users] u ON u.[recordid] = uc.[UsersId] AND u.[UUID] = @UsersUUID
		INNER JOIN [dbo].[Companies] c ON c.[recordID] = uc.[Companyid] AND c.[UUID] = @CompanyUUID
	END


	--if @UserCompanyUUID is still null then insert a new record in [Base].[UsersCompanys] and get the @UserCompanyUUID
	--otherwise update the record in [Base].[UsersCompanys]
	IF @UserCompanyUUID IS NULL
	BEGIN

		DECLARE @Usersid int = 0
		, @Companyid int = 0
		, @UsersidLoggedIn int = 0

		SELECT @Companyid = [Recordid]
		FROM [dbo].[Companies] c
		WHERE c.[UUID] = @CompanyUUID 

		SELECT @Usersid = [Recordid]
		FROM [dbo].[users] c
		WHERE c.[UUID] = @UsersUUID

		SELECT @UsersidLoggedIn   = [Recordid]
		FROM [dbo].[users] c
		WHERE c.[UUID] = @UsersUUIDLoggedIn  

		INSERT INTO [base].[UsersCompanys] ( [Companyid], [Usersid], [UsersidCreated] , [DateStarted], [EmployeeNo], [ContactNumber], [isEmployee] )
		VALUES( @Companyid, @Usersid, @UsersidLoggedIn, @DateOfEmployment, @EmployeeNumber , @PhoneNumber, @isEmployee )
		SELECT @UserCompanyid = SCOPE_IDENTITY()

		SELECT @UserCompanyUUID = uc.[UUID]
		FROM [Base].[UsersCompanys] uc
		WHERE uc.[Id] = @UserCompanyid

 		--get the UUID of the newly inserted record
		SELECT @UserCompanyUUID [UUID], 1 [isValid], 'Employee information has been saved successfully' [Message];
		Return 1;
	END
	ELSE
	BEGIN

		UPDATE [base].[UsersCompanys]
		   SET  [DateStarted] = @DateOfEmployment
			  , [EmployeeNo] = @EmployeeNumber
			  , [ContactNumber] = @PhoneNumber
			  , [isActive] = 1
			  , [isDeleted] = 0
		WHERE [UUID] = @UserCompanyUUID
	 
		--get the UUID of the updated record
		SELECT @UserCompanyUUID [UUID], 1 [isValid], 'Employee information has been updated successfully' [Message];
		Return 1;
	END

	SELECT @UserCompanyUUID [UUID], 0 [isValid], 'Employee information has not been saved' [Message]; 
	return 0;

END
