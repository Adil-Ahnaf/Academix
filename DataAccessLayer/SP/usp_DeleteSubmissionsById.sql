CREATE OR ALTER PROCEDURE [dbo].[usp_DeleteSubmissionsById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[Submissions] WHERE Id = @Id;
END