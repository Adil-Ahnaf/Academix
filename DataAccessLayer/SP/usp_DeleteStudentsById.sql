CREATE OR ALTER PROCEDURE [dbo].[usp_DeleteStudentsById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[Students] WHERE Id = @Id;
END