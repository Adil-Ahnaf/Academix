CREATE OR ALTER PROCEDURE [dbo].[usp_GetSubjectsById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Subjects] WHERE Id = @Id;
END