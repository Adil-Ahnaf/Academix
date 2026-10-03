CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllSubjects]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Subjects];
END