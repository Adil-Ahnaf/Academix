CREATE OR ALTER PROCEDURE [dbo].[usp_GetClassesById]
	@ClassGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Classes] WHERE ClassGuid = @ClassGuid;
END