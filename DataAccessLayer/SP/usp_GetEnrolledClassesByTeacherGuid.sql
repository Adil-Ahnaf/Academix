USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetEnrolledClassesByTeacherGuid] 
	@TeacherGuid UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	-- Find total enrolled students of each class
	SELECT ClassId, COUNT(*) AS TotalEnrolled
	INTO #ClassEnrollment
	FROM [dbo].[StudentEnrollments]
	GROUP BY ClassId

	-- Find total assignment assigned of each class
	SELECT ClassId, COUNT(*) AS TotalAssignment
	INTO #AssignAssignment
	FROM [dbo].[Assignments]
	GROUP BY ClassId

    SELECT C.ClassName, SU.Name AS SubjectName, C.Section, CE.TotalEnrolled, AA.TotalAssignment, C.ClassGuid
	FROM [dbo].[Teachers] AS T
	INNER JOIN [dbo].[TeacherEnrollments] AS TE ON TE.TeacherId = T.Id
	INNER JOIN [dbo].[Classes] AS C ON C.Id = TE.ClassId
	INNER JOIN [dbo].[Subjects] AS SU ON SU.Id = C.SubjectId
	LEFT JOIN #ClassEnrollment AS CE ON CE.ClassId = C.Id
	LEFT JOIN #AssignAssignment AS AA ON AA.ClassId = C.Id
	WHERE T.TeacherGuid = @TeacherGuid;

	DROP TABLE #ClassEnrollment;
	DROP TABLE #AssignAssignment;
END
GO
