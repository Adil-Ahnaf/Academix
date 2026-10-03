CREATE OR ALTER PROCEDURE [dbo].[usp_DeleteTeachersById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[Teachers] WHERE Id = @Id;
END