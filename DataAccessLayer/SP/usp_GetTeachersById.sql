CREATE OR ALTER PROCEDURE [dbo].[usp_GetTeachersById]
	@TeacherGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Teachers] WHERE TeacherGuid = @TeacherGuid;
END