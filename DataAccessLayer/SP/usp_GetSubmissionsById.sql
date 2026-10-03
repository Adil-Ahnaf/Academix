CREATE OR ALTER PROCEDURE [dbo].[usp_GetSubmissionsById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Submissions] WHERE Id = @Id;
END