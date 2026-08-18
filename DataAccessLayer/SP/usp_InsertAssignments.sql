CREATE OR ALTER PROCEDURE [dbo].[usp_InsertAssignments]
	@TeacherEnrollmentId bigint,
	@Title nvarchar(200),
	@Description nvarchar(max),
	@Marks int,
	@Deadline datetime,
	@IsPublish int,
	@AssignmentGuid uniqueidentifier,
	@CreatedDate datetime,
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[Assignments](TeacherEnrollmentId, Title, Description, Marks, Deadline, IsPublish, AssignmentGuid, CreatedDate, IsActive)
	VALUES(@TeacherEnrollmentId, @Title, @Description, @Marks, @Deadline, @IsPublish, @AssignmentGuid, @CreatedDate, @IsActive);

	SELECT @@IDENTITY;
END