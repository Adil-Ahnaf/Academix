CREATE OR ALTER PROCEDURE [dbo].[usp_DeleteSubjectsById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[Subjects] WHERE Id = @Id;
END