CREATE OR ALTER PROCEDURE [dbo].[usp_GetTeachersById]
	@Id bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Teachers] WHERE Id = @Id;
END