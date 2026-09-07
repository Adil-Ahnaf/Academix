USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllAssignmentByClassGuid]
	@ClassGuid UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	-- find total submission of each assignment
	SELECT AssignmentId, COUNT(*) AS TotalSubmission
	INTO #TempSubmission
	FROM [dbo].[Submissions]
	GROUP BY AssignmentId;

    SELECT A.*, S.TotalSubmission
	FROM [dbo].[Assignments] AS A
	INNER JOIN [dbo].[Classes] AS C ON C.Id = A.ClassId
	LEFT JOIN #TempSubmission AS S ON S.AssignmentId = A.Id
	WHERE C.ClassGuid = @ClassGuid AND C.IsActive = 1;

	DROP TABLE [#TempSubmission];
END
GO
