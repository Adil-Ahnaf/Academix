CREATE OR ALTER PROCEDURE [dbo].[usp_DeleteTeacherEnrollmentsById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[TeacherEnrollments] WHERE Id = @Id;
END