
CREATE PROCEDURE [base].[spUsers_Basic_ByUUID]

   @CompanyUUID NVARCHAR(200) 
, @UsersUUIDLoggedin NVARCHAR(200) 
, @UsersUUID NVARCHAR(200) 

AS
BEGIN

	DECLARE   @Usersid INT 
	, @DateOfBirth DATETIME = NULL
	, @URLAddress NVARCHAR(360)
	, @Path NVARCHAR(300)
	, @IDNumber NVARCHAR(300)
	, @FileExists INT;

	SELECT TOP(1) @Path = c.[RootPath] + '\Images\Avatars\' + CONVERT(NVARCHAR, u.[recordid]) + 'medium.jpg'
	, @URLAddress = c.[URLAddress]
	, @Usersid = u.[recordid] 
	, @DateOfBirth = u.[DateOfBirth] 
	, @IDNumber = u.[IDNumber]
	FROM [dbo].[users] u
	CROSS JOIN [dbo].[Companies] c 
	WHERE c.[UUID] = @CompanyUUID AND u.[UUID] = @UsersUUID;

  --check that file exist other wise the default avatar
  EXEC xp_fileexist @Path, @FileExists OUTPUT;

  --if the date of birth is null the see if the id number is a valid SA No and extract the date of bith from the id number
  IF @DateOfBirth IS NULL
  BEGIN

	SET @DateOfBirth = [base].[func_Tool_SAIDNumber_ReturnBirthDate] (@IDNumber)

	IF @DateOfBirth IS NOT NULL
	BEGIN
		UPDATE [dbo].[users]
		SET [DateOfBirth] = @DateOfBirth
		WHERE [recordid] = @Usersid; 
	END
  END 


  SELECT u.[UUID]
  , u.[firstname] 
  , u.[lastname]
  , u.[IDNumber]
  , u.[email]
  , u.[mobile]
  , u.[Gender]
  , u.[Race] [Ethnicity]
  , u.[DateOfBirth]


  , CASE WHEN @FileExists = 1 THEN @URLAddress + 'Images/Avatars/' + CONVERT(NVARCHAR, @Usersid) + 'medium.jpg' ELSE @URLAddress + 'Images/Avatars/anonymous-avatar.jpg' END [UsersImageURL]
  FROM [dbo].[users] u
  WHERE u.[recordid] = @Usersid 

END
