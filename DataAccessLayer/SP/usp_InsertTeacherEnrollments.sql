CREATE OR ALTER PROCEDURE [dbo].[usp_InsertTeacherEnrollments]
	@TeacherId bigint,
	@ClassId bigint,
	@CreatedDate datetime,
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[TeacherEnrollments](TeacherId, ClassId, CreatedDate, IsActive)
	VALUES(@TeacherId, @ClassId, @CreatedDate, @IsActive);

	SELECT @@IDENTITY;
END