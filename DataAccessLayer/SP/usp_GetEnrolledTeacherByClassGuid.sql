USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetEnrolledTeacherByClassGuid] 
	@ClassGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    SELECT T.FullName, T.Department, T.Gender
	FROM [dbo].[Classes] AS C
	INNER JOIN [dbo].[TeacherEnrollments] AS TE ON TE.ClassId = C.Id
	INNER JOIN [dbo].[Teachers] AS T ON T.Id = TE.TeacherId
	WHERE C.ClassGuid = @ClassGuid;
END
GO
