--beginvalidatingquery 
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tblFindDatabaseVersion') 
    BEGIN 
    declare @major int = 13, @minor int = 2, @patch int = 5
    IF EXISTS (SELECT 1 FROM dbo.tblFindDatabaseVersion WHERE Major = @major AND Minor = @minor AND Patch = @patch) 
        SELECT 0,'Already correct database version' 
    ELSE 
        SELECT 1, 'Upgrading database' 
    END 
ELSE 
    SELECT -1, 'Not an EPiServer database with Find' 
--endvalidatingquery 
 
GO 

DROP INDEX [IDX_tblFindIndexQueue_Indexed] ON [dbo].[tblFindIndexQueue]
GO

CREATE NONCLUSTERED INDEX [IDX_tblFindIndexQueue_Indexed] ON [dbo].[tblFindIndexQueue]
(
	[Hash] ASC
)
GO

CREATE CLUSTERED INDEX [IDX_tblFindIndexQueue_Clustered] ON [dbo].[tblFindIndexQueue]
(
	[TimeStamp] ASC
)
GO

 
INSERT INTO tblFindDatabaseVersion(Major, Minor, Patch) VALUES (13,2,5)

GO

PRINT N'Update complete.';