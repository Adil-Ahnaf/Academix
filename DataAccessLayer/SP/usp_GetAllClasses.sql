CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllClasses]
AS
BEGIN
	SET NOCOUNT ON;

	-- Store total enrollment for each class in temporary table
	SELECT ClassId, COUNT(*) AS TotalEnrolled
	INTO #ClassEnrollment
	FROM [dbo].[StudentEnrollments]
	GROUP BY ClassId;

	SELECT C.ClassGuid, C.AcademicYear, C.ClassName, S.Name AS SubjectName, C.Section, C.MaxCapacity, E.TotalEnrolled, C.IsActive 
	FROM [dbo].[Classes] AS C
	INNER JOIN [dbo].[Subjects] AS S ON C.SubjectId = S.Id
	LEFT JOIN #ClassEnrollment AS E ON E.ClassId = C.Id;

	DROP TABLE #ClassEnrollment;
END