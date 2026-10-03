CREATE OR ALTER PROCEDURE [dbo].[usp_GetStudentEnrollmentsById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[StudentEnrollments] WHERE Id = @Id;
END