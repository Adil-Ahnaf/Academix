CREATE OR ALTER PROCEDURE [dbo].[usp_GetAssignmentsById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Assignments] WHERE Id = @Id;
END