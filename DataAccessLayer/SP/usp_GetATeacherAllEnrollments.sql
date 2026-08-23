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

    SELECT * 
	FROM [dbo].[TeacherEnrollments] AS E
	INNER JOIN [dbo].[Teachers] AS T ON T.Id = E.TeacherId
	WHERE T.TeacherGuid = @TeacherGuid AND E.IsActive = 1;
END
GO
