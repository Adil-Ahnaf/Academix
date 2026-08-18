CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateAssignmentsById]
	@Id bigint,
	@TeacherEnrollmentId bigint,
	@Title nvarchar(200),
	@Description nvarchar(max),
	@Marks int,
	@Deadline datetime,
	@IsPublish int,
	@AssignmentGuid uniqueidentifier,
	@ModifiedDate datetime,
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Assignments] SET TeacherEnrollmentId = @TeacherEnrollmentId, Title = @Title, Description = @Description, Marks = @Marks, Deadline = @Deadline, IsPublish = @IsPublish, AssignmentGuid = @AssignmentGuid, ModifiedDate = @ModifiedDate, IsActive = @IsActive
	WHERE Id = @Id;
END