USE [AcademixDB]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetTeacherEnrollmentByClassId]
	@ClassId BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT TOP 1 Id FROM [dbo].[TeacherEnrollments] 
	WHERE ClassId = @ClassId AND IsActive = 1;
END
GO
