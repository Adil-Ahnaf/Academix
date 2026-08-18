CREATE OR ALTER PROCEDURE [dbo].[usp_GetStudentsById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Students] WHERE Id = @Id;
END