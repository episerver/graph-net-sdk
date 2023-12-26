--beginvalidatingquery 
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tblFindDatabaseVersion') 
    BEGIN 
    declare @major int = 13, @minor int = 2, @patch int = 3 
    IF EXISTS (SELECT 1 FROM dbo.tblFindDatabaseVersion WHERE Major = @major AND Minor = @minor AND Patch = @patch) 
        SELECT 0,'Already correct database version' 
    ELSE 
        SELECT 1, 'Upgrading database' 
    END 
ELSE 
    SELECT -1, 'Not an EPiServer database with Find' 
--endvalidatingquery 
 
GO 

ALTER PROCEDURE [dbo].[findIndexQueueLoadItems]
(
    @items INT,
    @currentTime datetime,
    @acceptLastReadOlderThan datetime
)
AS 
BEGIN


;WITH cte AS 
( 
    SELECT TOP (@items) * 
    FROM tblFindIndexQueue WHERE (LastRead IS NULL OR LastRead < @acceptLastReadOlderThan) ORDER BY TimeStamp
) 
UPDATE cte SET LastRead = @currentTime
OUTPUT deleted.Action ,deleted.[Cascade], 
deleted.EnableLanguageFilter, deleted.Item, deleted.[Language], deleted.TimeStamp, deleted.Hash
    
END
GO
 
INSERT INTO tblFindDatabaseVersion(Major, Minor, Patch) VALUES (13,2,3)

GO

PRINT N'Update complete.';