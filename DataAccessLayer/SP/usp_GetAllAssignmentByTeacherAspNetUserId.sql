USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllAssignmentByTeacherAspNetUserId]
	@AspNetUserId NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT A.*
	FROM [dbo].[Assignments] AS A
	INNER JOIN [dbo].[Classes] AS C ON C.Id = A.ClassId
	INNER JOIN [dbo].[TeacherEnrollments] AS TE ON TE.ClassId = C.Id
	INNER JOIN [dbo].[Teachers] AS T ON T.Id = TE.TeacherId
	WHERE T.AspNetUserId = @AspNetUserId AND C.IsActive = 1;
END
GO
