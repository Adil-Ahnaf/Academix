USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllAssignmentsInfo] 
AS
BEGIN
	SET NOCOUNT ON;

    SELECT C.ClassName, C.Section, A.Title, A.Marks, A.Deadline, A.AssignmentGuid
	FROM [dbo].[Assignments] AS A
	INNER JOIN [dbo].[Classes] AS C ON C.Id = A.ClassId

END
GO
