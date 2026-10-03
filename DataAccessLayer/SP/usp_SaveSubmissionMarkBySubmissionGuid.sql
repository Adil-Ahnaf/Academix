USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_SaveSubmissionMarkBySubmissionGuid] 
	@Marks DECIMAL(5,2),
	@Feedback NVARCHAR(MAX),
	@ModifiedDate DATETIME,
	@ModifiedBy NVARCHAR(450),
	@SubmissionGuid UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE [dbo].[Submissions]
	SET Marks = @Marks, Feedback = @Feedback, ModifiedDate = @ModifiedDate, ModifiedBy = @ModifiedBy
	WHERE SubmissionGuid = @SubmissionGuid
END
GO
