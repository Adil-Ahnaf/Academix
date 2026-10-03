USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetSubmissionsByStudentAspNetUserId] 
	@AspNetUserId NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT SU.*, S.AspNetUserId
	FROM [dbo].[AspNetUsers] AS U
	INNER JOIN [Students] AS S ON S.AspNetUserId = U.Id
	INNER JOIN [Submissions] AS SU ON SU.StudentId = S.Id
	WHERE U.Id = @AspNetUserId;
END
GO
