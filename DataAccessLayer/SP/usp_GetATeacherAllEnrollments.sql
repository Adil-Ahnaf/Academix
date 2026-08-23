USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetATeacherAllEnrollments] 
	@TeacherGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    SELECT C.* 
	FROM [dbo].[TeacherEnrollments] AS E
	INNER JOIN [dbo].[Teachers] AS T ON T.Id = E.TeacherId
	INNER JOIN [dbo].[Classes] AS C ON C.Id = E.ClassId
	WHERE T.TeacherGuid = @TeacherGuid AND C.IsActive = 1;
END
GO
