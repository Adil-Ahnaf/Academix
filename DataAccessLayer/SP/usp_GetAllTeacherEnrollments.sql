CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllTeacherEnrollments]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[TeacherEnrollments];
END