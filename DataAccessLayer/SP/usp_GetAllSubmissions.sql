CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllSubmissions]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Submissions];
END