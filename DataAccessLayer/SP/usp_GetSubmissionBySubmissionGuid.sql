USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetSubmissionBySubmissionGuid] 
	@SubmissionGuid UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Submissions] WHERE SubmissionGuid = @SubmissionGuid;
END
GO
