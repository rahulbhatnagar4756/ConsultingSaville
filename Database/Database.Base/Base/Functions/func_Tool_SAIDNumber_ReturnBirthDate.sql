
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [base].[func_Tool_SAIDNumber_ReturnBirthDate]
(
	 @IDNumber VARCHAR(50) 
)
RETURNS date
AS
BEGIN
	 
	DECLARE @Date date = null;

	-- Check if the ID number has a valid format (13 characters, all numeric)
	IF LEN(@IDNumber) = 13 AND @IDNumber NOT LIKE '%[^0-9]%'
	BEGIN
		-- Extract components from the ID number
		DECLARE @BirthDate date = CONVERT(DATE, SUBSTRING(@IDNumber, 1, 6), 112)
		, @GenderCode int = CAST(SUBSTRING(@IDNumber, 7, 4) AS INT)
		, @Gender nvarchar(10)

		-- Determine gender based on the 11th digit
		IF @GenderCode BETWEEN 0 AND 4999
			SET @Gender = 'Female';
		ELSE IF @GenderCode BETWEEN 5000 AND 9999
			SET @Gender = 'Male';
		ELSE
			SET @Gender = null;

		-- Check if the birthdate is valid (not in the future)
		IF @BirthDate <= GETDATE()
		BEGIN
			-- Check if the gender code is valid (0 for female, 1 for male)
			IF NOT @Gender is null
			BEGIN
				-- Perform checksum validation
				DECLARE @Checksum INT = CAST(SUBSTRING(@IDNumber, 13, 1) AS INT)
				, @CalculatedChecksum3 int = 0
				DECLARE @CalculatedChecksum int =
					(CAST(SUBSTRING(@IDNumber, 1, 1) AS INT) +
					 CAST(SUBSTRING(@IDNumber, 3, 1) AS INT) +
					 CAST(SUBSTRING(@IDNumber, 5, 1) AS INT) +
					 CAST(SUBSTRING(@IDNumber, 7, 1) AS INT) +
					 CAST(SUBSTRING(@IDNumber, 9, 1) AS INT) +
					 CAST(SUBSTRING(@IDNumber, 11, 1) AS INT));

				DECLARE @CalculatedChecksum2 nvarchar(50) =
					CAST((CAST(SUBSTRING(@IDNumber, 2, 1)  +
						  SUBSTRING(@IDNumber, 4, 1)  +
						  SUBSTRING(@IDNumber, 6, 1)  +
						  SUBSTRING(@IDNumber, 8, 1)  +
						  SUBSTRING(@IDNumber, 10, 1) +
						  SUBSTRING(@IDNumber, 12, 1) AS INT) * 2) as nvarchar(50));	

				--add the result of @CalculatedChecksum2 together by char + char + 
				WITH DigitCTE AS (
					SELECT CAST(SUBSTRING(@CalculatedChecksum2, 1, 1) AS INT) AS Digit, 1 AS Position
					UNION ALL
					SELECT CAST(SUBSTRING(@CalculatedChecksum2, Position + 1, 1) AS INT), Position + 1
					FROM DigitCTE
					WHERE Position < LEN(@CalculatedChecksum2)
				)
				SELECT @CalculatedChecksum3 = SUM(Digit) 
				FROM DigitCTE;

				--add the odds and evens scores together
				SET @CalculatedChecksum += @CalculatedChecksum3;
				--get the last number for the total number if 41 will be 1 then minus it from 10
				SET @CalculatedChecksum = 10 - (@CalculatedChecksum % 10)

				-- Check if the calculated checksum matches the provided checksum
				IF @Checksum = @CalculatedChecksum
				BEGIN
					-- The ID number is valid, you can proceed to retrieve the Date of Birth
					SET @Date = @BirthDate
				END
			END
		END
	END

	return @Date

END
