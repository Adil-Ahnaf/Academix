CREATE OR ALTER PROCEDURE [dbo].[usp_DeleteClassesById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[Classes] WHERE Id = @Id;
END