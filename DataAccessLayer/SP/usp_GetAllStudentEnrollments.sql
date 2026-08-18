CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllStudentEnrollments]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[StudentEnrollments];
END