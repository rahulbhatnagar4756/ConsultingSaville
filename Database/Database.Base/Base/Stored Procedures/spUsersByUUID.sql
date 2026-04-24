
CREATE PROCEDURE [base].[spUsersByUUID]

 @CompanyUUID NVARCHAR(200) 
, @UsersUUIDLoggedin NVARCHAR(200) = ''
, @UsersUUID NVARCHAR(200) 

AS
BEGIN

	DECLARE @UUID NVARCHAR(200)
	, @Recordid INT
	, @Firstname NVARCHAR(300)
	, @Lastname NVARCHAR(300)
	, @Email NVARCHAR(350)
	, @URLAddress NVARCHAR(360)
	, @Path NVARCHAR(300)
	, @FileExists INT;

	SELECT TOP(1) @Recordid = u.[recordid]  
	, @UUID = u.[UUID]  
	, @Firstname = u.[firstname]
	, @Lastname = u.[lastname]
	, @Email = u.[email] 
	, @URLAddress = c.[URLAddress]
	, @Path = c.[RootPath] + '\Images\Avatars\' + CONVERT(NVARCHAR, u.[recordid]) + 'medium.jpg'
	FROM [dbo].[users] u
	CROSS JOIN [dbo].[Companies] c 
	WHERE c.[UUID] = @CompanyUUID AND u.[UUID] = @UsersUUID;


  EXEC xp_fileexist @Path, @FileExists OUTPUT;

  SELECT @UUID [UsersUUID]
  , @Firstname [Firstname]
  , @Lastname [Lastname]
  , @Email [Email]
  , CASE WHEN @FileExists = 1 THEN @URLAddress + 'Images/Avatars/' + CONVERT(NVARCHAR, @Recordid) + 'medium.jpg' ELSE @URLAddress + 'Images/Avatars/anonymous-avatar.jpg' END [UsersImageURL]


END
