USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllAvailableClassesForAStudent] 
	@StudentGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	-- Store total enrollment for each class in temporary table
	SELECT ClassId, COUNT(*) AS TotalEnrolled
	INTO #ClassEnrollment
	FROM [dbo].[StudentEnrollments]
	GROUP BY ClassId;

	SELECT C.ClassGuid, C.AcademicYear, C.ClassName, S.Name AS SubjectName, C.Section, C.MaxCapacity, CE.TotalEnrolled
	FROM [dbo].[Classes] AS C
	INNER JOIN [dbo].[Subjects] AS S ON S.Id = C.SubjectId
	LEFT JOIN #ClassEnrollment AS CE ON CE.ClassId = C.Id
	WHERE C.IsActive = 1 AND NOT EXISTS 
	(
		SELECT 1
		FROM [dbo].[StudentEnrollments] AS E
		INNER JOIN [dbo].[Students] AS ST ON ST.Id = E.StudentId
		WHERE E.ClassId = C.Id AND ST.StudentGuid = @StudentGuid
	);

	DROP TABLE #ClassEnrollment;
END
GO
