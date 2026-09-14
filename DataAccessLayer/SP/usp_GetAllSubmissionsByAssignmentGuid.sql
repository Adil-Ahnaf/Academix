USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllSubmissionsByAssignmentGuid]
	@AssignmentGuid UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	-- Get All Enrolled Students Submission File --
	SELECT S.StudentCode, S.FullName, SU.[FileName], SU.FilePath, SU.Marks, SU.Feedback, SU.SubmissionGuid
	FROM [dbo].[Assignments]  AS A
	INNER JOIN [dbo].[StudentEnrollments] AS SE ON SE.ClassId = A.ClassId
	INNER JOIN [dbo].[Students] AS S ON S.Id = SE.Id
	LEFT JOIN [dbo].[Submissions] AS SU ON SU.StudentId = S.Id
	WHERE A.AssignmentGuid = @AssignmentGuid;

END
GO
