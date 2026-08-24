USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllActiveClassesForATeacher] 
	@TeacherGuid uniqueidentifier
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
		INNER JOIN [dbo].[Teachers] AS T ON T.Id = E.TeacherId
		WHERE E.ClassId = C.Id AND T.TeacherGuid = @TeacherGuid
	);
END
GO
