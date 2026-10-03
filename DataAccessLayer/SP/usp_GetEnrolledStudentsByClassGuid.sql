USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetEnrolledStudentsByClassGuid]
	@ClassGuid UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

    SELECT S.*
	FROM [dbo].[Classes] AS C
	INNER JOIN [dbo].[StudentEnrollments] AS SE ON SE.ClassId = C.Id
	INNER JOIN [dbo].[Students] AS S ON S.Id = SE.StudentId
	WHERE C.ClassGuid = @ClassGuid;
END
GO
