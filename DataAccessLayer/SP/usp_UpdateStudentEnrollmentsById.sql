CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateStudentEnrollmentsById]
	@Id bigint,
	@StudentId bigint,
	@ClassId bigint,
	@ModifiedDate datetime,
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[StudentEnrollments] SET StudentId = @StudentId, ClassId = @ClassId, ModifiedDate = @ModifiedDate, IsActive = @IsActive
	WHERE Id = @Id;
END