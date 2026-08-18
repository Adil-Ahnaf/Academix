CREATE OR ALTER PROCEDURE [dbo].[usp_GetTeacherEnrollmentsById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[TeacherEnrollments] WHERE Id = @Id;
END