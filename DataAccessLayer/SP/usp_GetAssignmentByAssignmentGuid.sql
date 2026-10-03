USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAssignmentByAssignmentGuid] 
	@AssignmentGuid UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Assignments] WHERE AssignmentGuid = @AssignmentGuid;
END
GO
