CREATE OR ALTER PROCEDURE [dbo].[usp_GetClassesByClassGuid]
	@ClassGuid UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT C.*, S.Name AS SubjectName
	FROM [dbo].[Classes] AS C
	INNER JOIN [dbo].[Subjects] AS S ON S.Id = C.SubjectId
	WHERE C.ClassGuid = @ClassGuid;
END