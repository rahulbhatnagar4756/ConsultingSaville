

CREATE PROCEDURE [base].[spLookup_Gender]
	 @CompanyUUID NVARCHAR(200) 
	, @UsersUUIDLoggedIn NVARCHAR(200)  = ''
AS
BEGIN

	DECLARE @LookupTypesidGender INT = 10

	SELECT l.[Recordid] [Id]
	, l.[Value] [Name]
	, l.[OrderVal] 
	FROM [iag].[dbo].[Lookup] l
	INNER JOIN [dbo].[Companies] c ON c.[recordID] = l.[Companyid] AND c.[UUID] = @CompanyUUID 

	WHERE l.[LookupTypeid] = @LookupTypesidGender AND l.[isDeleted] = 0

END
