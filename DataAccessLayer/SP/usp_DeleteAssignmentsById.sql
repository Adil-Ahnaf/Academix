CREATE OR ALTER PROCEDURE [dbo].[usp_DeleteAssignmentsById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[Assignments] WHERE Id = @Id;
END