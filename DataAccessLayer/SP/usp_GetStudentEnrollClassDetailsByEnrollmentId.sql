USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetStudentEnrollClassDetailsByEnrollmentId]
	@EnrollmentId BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    -- Store total enrollment for each class in temporary table
	SELECT DISTINCT ClassId, COUNT(*) OVER (PARTITION BY ClassId) AS TotalEnrolled
	INTO #ClassEnrollment
	FROM [dbo].[StudentEnrollments];

	SELECT C.ClassGuid, C.AcademicYear, C.ClassName, S.Name AS SubjectName, C.Section, C.MaxCapacity, CE.TotalEnrolled 
	FROM [dbo].[StudentEnrollments] AS E
	INNER JOIN [dbo].[Classes] AS C ON C.Id = E.ClassId
	INNER JOIN [dbo].[Subjects] AS S ON S.Id = C.SubjectId
	INNER JOIN #ClassEnrollment AS CE ON CE.ClassId = C.Id
	WHERE E.Id = @EnrollmentId;

	DROP TABLE #ClassEnrollment;
END
GO
