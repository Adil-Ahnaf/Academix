CREATE OR ALTER PROCEDURE [dbo].[usp_GetClassesById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Classes] WHERE Id = @Id;
END