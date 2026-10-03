CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllTeachers]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Teachers];
END