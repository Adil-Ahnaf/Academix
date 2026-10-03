USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[usp_GetStudentEnrollmentByClassAndStudent] 
	@ClassGuid uniqueidentifier,
	@StudentGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    SELECT E.*
	FROM [dbo].[StudentEnrollments] AS E
	INNER JOIN [dbo].[Classes] AS C ON C.Id = E.ClassId
	INNER JOIN [dbo].[Students] AS S ON S.Id = E.StudentId
	WHERE E.IsActive = 1 AND C.ClassGuid = @ClassGuid AND S.StudentGuid = @StudentGuid;
END
GO
