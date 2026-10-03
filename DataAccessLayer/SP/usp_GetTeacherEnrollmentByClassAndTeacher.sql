USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[usp_GetTeacherEnrollmentByClassAndTeacher] 
	@ClassGuid uniqueidentifier,
	@TeacherGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    SELECT E.*
	FROM [dbo].[TeacherEnrollments] AS E
	INNER JOIN [dbo].[Classes] AS C ON C.Id = E.ClassId
	INNER JOIN [dbo].[Teachers] AS T ON T.Id = E.TeacherId
	WHERE E.IsActive = 1 AND C.ClassGuid = @ClassGuid AND T.TeacherGuid = @TeacherGuid;
END
GO
