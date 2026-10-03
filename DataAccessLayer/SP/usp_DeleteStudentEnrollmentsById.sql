CREATE OR ALTER PROCEDURE [dbo].[usp_DeleteStudentEnrollmentsById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[StudentEnrollments] WHERE Id = @Id;
END