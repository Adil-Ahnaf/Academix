CREATE OR ALTER PROCEDURE [dbo].[usp_InsertAssignments]
	@ClassId bigint,
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

    INSERT INTO [dbo].[Assignments](ClassId, Title, [Description], FilePath, Marks, Deadline, IsPublish, AssignmentGuid, CreatedDate, IsActive)
	VALUES(@ClassId, @Title, @Description, @FilePath, @Marks, @Deadline, @IsPublish, NEWID(), @CreatedDate, @IsActive);

	SELECT @@IDENTITY;
END