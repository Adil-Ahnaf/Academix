CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateAssignmentsById]
	@Id bigint,
	@Title nvarchar(200),
	@Description nvarchar(max),
	@FilePath nvarchar(500),
	@Marks int,
	@Deadline datetime,
	@IsPublish int,
	@ModifiedDate datetime
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Assignments] 
	SET 
		Title = @Title, 
		[Description] = @Description,
		FilePath = @FilePath,
		Marks = @Marks, 
		Deadline = @Deadline, 
		IsPublish = @IsPublish, 
		ModifiedDate = @ModifiedDate
	WHERE Id = @Id;
END