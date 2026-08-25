USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllAvailableClassesForATeacher] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT C.*, S.Name AS SubjectName
	FROM [dbo].[Classes] AS C
	INNER JOIN [dbo].[Subjects] AS S ON S.Id = C.SubjectId
	WHERE C.IsActive = 1 AND NOT EXISTS 
	(
		SELECT 1
		FROM [dbo].[TeacherEnrollments] AS E
		WHERE E.ClassId = C.Id
	);
END
GO
