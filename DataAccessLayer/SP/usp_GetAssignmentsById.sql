CREATE OR ALTER PROCEDURE [dbo].[usp_GetAssignmentByAssignmentGuid]
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Assignments] WHERE Id = @Id;
END