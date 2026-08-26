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

    SELECT C.*, SU.Name AS SubjectName
	FROM [dbo].[StudentEnrollments] AS E
	INNER JOIN [dbo].[Students] AS S ON S.Id = E.StudentId
	INNER JOIN [dbo].[Classes] AS C ON C.Id = E.ClassId
	INNER JOIN [dbo].[Subjects] AS SU ON SU.Id = C.SubjectId
	WHERE S.StudentGuid = @StudentGuid AND C.IsActive = 1;
END
GO
