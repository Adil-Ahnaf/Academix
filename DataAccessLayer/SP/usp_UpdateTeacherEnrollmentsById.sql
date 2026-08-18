CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateTeacherEnrollmentsById]
	@Id bigint,
	@TeacherId bigint,
	@ClassId bigint,
	@ModifiedDate datetime,
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[TeacherEnrollments] SET TeacherId = @TeacherId, ClassId = @ClassId, ModifiedDate = @ModifiedDate, IsActive = @IsActive
	WHERE Id = @Id;
END