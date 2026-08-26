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

	SELECT C.*, S.Name AS SubjectName
	FROM [dbo].[Classes] AS C
	INNER JOIN [dbo].[Subjects] AS S ON S.Id = C.SubjectId
	WHERE C.IsActive = 1 AND NOT EXISTS 
	(
		SELECT 1
		FROM [dbo].[StudentEnrollments] AS E
		INNER JOIN [dbo].[Students] AS ST ON ST.Id = E.StudentId
		WHERE E.ClassId = C.Id AND ST.StudentGuid = @StudentGuid
	);
END
GO
