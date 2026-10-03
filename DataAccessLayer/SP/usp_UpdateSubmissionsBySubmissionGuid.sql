USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateSubmissionsBySubmissionGuid]
	@FileName nvarchar(255),
	@FilePath nvarchar(500),
	@SubmissionGuid uniqueidentifier,
	@ModifiedDate datetime,
	@ModifiedBy nvarchar(450)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Submissions] SET [FileName] = @FileName, FilePath = @FilePath, ModifiedDate = @ModifiedDate, ModifiedBy = @ModifiedBy
	WHERE SubmissionGuid = @SubmissionGuid;
END
GO