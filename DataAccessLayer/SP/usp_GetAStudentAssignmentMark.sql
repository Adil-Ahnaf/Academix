USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAStudentAssignmentMark]
	@AssignmentGuid UNIQUEIDENTIFIER,
	@AspNetUserId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

    SELECT SU.Marks, SU.Feedback
	FROM [dbo].[Submissions] AS SU
	INNER JOIN [dbo].[Assignments] AS A ON A.Id = SU.AssignmentId
	INNER JOIN [dbo].[Students] AS S ON S.Id = SU.StudentId
	WHERE S.AspNetUserId = @AspNetUserId AND A.AssignmentGuid = @AssignmentGuid;
END
GO
