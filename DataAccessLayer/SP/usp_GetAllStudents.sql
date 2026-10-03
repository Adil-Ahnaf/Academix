CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllStudents]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Students];
END