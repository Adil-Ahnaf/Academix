CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllClasses]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *, S.Name AS SubjectName 
	FROM [dbo].[Classes] AS C
	INNER JOIN [dbo].[Subjects] AS S ON C.SubjectId = S.Id;
END