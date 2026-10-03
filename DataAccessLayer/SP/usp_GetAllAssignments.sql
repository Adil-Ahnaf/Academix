CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllAssignments]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Assignments];
END