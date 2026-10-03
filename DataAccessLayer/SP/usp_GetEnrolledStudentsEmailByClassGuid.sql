USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetEnrolledStudentsEmailByClassGuid]
	@ClassGuid UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

    SELECT U.Email As EmailAddress
	FROM [dbo].[StudentEnrollments] AS SE
	INNER JOIN [dbo].[Classes] AS C ON C.Id = SE.ClassId
	INNER JOIN [dbo].[Students] AS S ON S.Id = SE.StudentId
	INNER JOIN [dbo].[AspNetUsers] AS U ON U.Id = S.AspNetUserId
	WHERE C.ClassGuid = @ClassGuid;
END
GO
