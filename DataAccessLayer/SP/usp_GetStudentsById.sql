CREATE OR ALTER PROCEDURE [dbo].[usp_GetStudentsById]
	@StudentGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Students] WHERE StudentGuid = @StudentGuid;
END