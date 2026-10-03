CREATE OR ALTER PROCEDURE [dbo].[usp_InsertSubmissions]
	@AssignmentId bigint,
	@StudentId bigint,
	@FileName nvarchar(255),
	@FilePath nvarchar(500),
	@SubmissionGuid uniqueidentifier,
	@CreatedDate datetime,
	@CreatedBy nvarchar(450),
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[Submissions](AssignmentId, StudentId, [FileName], FilePath, SubmissionGuid, CreatedDate, CreatedBy, IsActive)
	VALUES(@AssignmentId, @StudentId, @FileName, @FilePath, @SubmissionGuid, @CreatedDate, @CreatedBy, @IsActive);

	SELECT @@IDENTITY;
END