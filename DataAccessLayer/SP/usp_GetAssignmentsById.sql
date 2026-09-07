CREATE OR ALTER PROCEDURE [dbo].[usp_GetAssignmentsById]
	@AssignmentGuid UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Assignments] WHERE AssignmentGuid = @AssignmentGuid;
END