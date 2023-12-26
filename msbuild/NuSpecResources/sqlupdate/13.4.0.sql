--beginvalidatingquery 
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tblFindDatabaseVersion') 
    BEGIN 
    declare @major int = 13, @minor int = 4, @patch int = 0
    IF EXISTS (SELECT 1 FROM dbo.tblFindDatabaseVersion WHERE Major = @major AND Minor = @minor AND Patch = @patch) 
        SELECT 0,'Already correct database version' 
    ELSE 
        SELECT 1, 'Upgrading database' 
    END 
ELSE 
    SELECT -1, 'Not an EPiServer database with Find' 
--endvalidatingquery 
 
GO 

ALTER PROCEDURE [dbo].[netSchedulerListLog]
	(
		@pkID UNIQUEIDENTIFIER,
		@startIndex BIGINT = 0,
		@maxCount INT = 100
	)
	AS
	BEGIN
		DECLARE @MaximumTextLength INT = 5000;
		DECLARE @TotalCount BIGINT = (SELECT COUNT(*) FROM tblScheduledItemLog WHERE fkScheduledItemId = @pkID)
		;WITH Items_CTE AS
		(
			SELECT [pkID], [Exec], [Status], [Text], [Duration], [Trigger], [Server], ROW_NUMBER() OVER (ORDER BY [Exec] DESC) AS [RowIndex]
			FROM tblScheduledItemLog
			WHERE fkScheduledItemId = @pkID
		)
		SELECT TOP (@maxCount) [Exec], Status,
			CASE
				WHEN LEN([Text]) > @MaximumTextLength
				THEN CONCAT(SUBSTRING([Text], 1, @MaximumTextLength), '... The displaying message is limited to ', @MaximumTextLength, ' chars! MessageID: ', [pkID], '. For more details, please check application logs!')
				ELSE [Text]
			END AS [Text],
			[Duration], [Trigger], [Server], @TotalCount AS 'TotalCount'
		FROM Items_CTE
		WHERE RowIndex >= @startIndex
	END
	
GO
 

INSERT INTO tblFindDatabaseVersion(Major, Minor, Patch) VALUES (13,4,0)

GO

PRINT N'Update complete.';