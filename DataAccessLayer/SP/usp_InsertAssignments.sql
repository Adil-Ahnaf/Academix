CREATE OR ALTER PROCEDURE [dbo].[usp_InsertAssignments]
	@TeacherEnrollmentId bigint,
	@Title nvarchar(200),
	@Description nvarchar(max),
	@FilePath nvarchar(500),
	@Marks int,
	@Deadline datetime,
	@IsPublish bit,
	@CreatedDate datetime,
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[Assignments](TeacherEnrollmentId, Title, [Description], FilePath, Marks, Deadline, IsPublish, AssignmentGuid, CreatedDate, IsActive)
	VALUES(@TeacherEnrollmentId, @Title, @Description, @FilePath, @Marks, @Deadline, @IsPublish, NEWID(), @CreatedDate, @IsActive);

	SELECT @@IDENTITY;
END