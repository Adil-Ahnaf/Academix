CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateSubmissionsById]
	@Id bigint,
	@AssignmentId bigint,
	@StudentId bigint,
	@FileName nvarchar(255),
	@FilePath nvarchar(500),
	@Marks decimal,
	@Feedback nvarchar(max),
	@SubmissionGuid uniqueidentifier,
	@ModifiedDate datetime,
	@ModifiedBy nvarchar(450),
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Submissions] SET AssignmentId = @AssignmentId, StudentId = @StudentId, FileName = @FileName, FilePath = @FilePath, Marks = @Marks, Feedback = @Feedback, SubmissionGuid = @SubmissionGuid, ModifiedDate = @ModifiedDate, ModifiedBy = @ModifiedBy, IsActive = @IsActive
	WHERE Id = @Id;
END