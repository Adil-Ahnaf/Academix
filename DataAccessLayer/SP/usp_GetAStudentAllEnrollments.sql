USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAStudentAllEnrollments] 
	@StudentGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	-- Store total enrollment for each class in temporary table
	SELECT DISTINCT ClassId, COUNT(*) OVER (PARTITION BY ClassId) AS TotalEnrolled
	INTO #ClassEnrollment
	FROM [dbo].[StudentEnrollments];

	-- Get student's enrolled classes
    SELECT C.ClassGuid, C.AcademicYear, C.ClassName, SU.Name AS SubjectName, C.Section, C.MaxCapacity, CE.TotalEnrolled
	FROM [dbo].[StudentEnrollments] AS E
	INNER JOIN [dbo].[Students] AS S ON S.Id = E.StudentId
	INNER JOIN [dbo].[Classes] AS C ON C.Id = E.ClassId
	INNER JOIN [dbo].[Subjects] AS SU ON SU.Id = C.SubjectId
	INNER JOIN #ClassEnrollment AS CE ON CE.ClassId = E.ClassId
	WHERE S.StudentGuid = @StudentGuid AND C.IsActive = 1;

	-- Delete temporary table
	DROP TABLE #ClassEnrollment;
END
GO
