--beginvalidatingquery 
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tblFindDatabaseVersion') 
    BEGIN 
    declare @major int = 13, @minor int = 0, @patch int = 4   
    IF EXISTS (SELECT 1 FROM dbo.tblFindDatabaseVersion WHERE Major = @major AND Minor = @minor AND Patch = @patch) 
        SELECT 0,'Already correct database version' 
    ELSE 
        SELECT 1, 'Upgrading database' 
    END 
ELSE 
    SELECT -1, 'Not an EPiServer database with Find' 
--endvalidatingquery 
 
GO 

IF NOT EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IDX_tblFindIndexQueue_Indexed')
BEGIN
    CREATE NONCLUSTERED INDEX [IDX_tblFindIndexQueue_Indexed] 
    ON [dbo].[tblFindIndexQueue] ([Hash], [LastRead])
END
GO  
 
INSERT INTO tblFindDatabaseVersion(Major, Minor, Patch) VALUES (13,0,4)

GO

PRINT N'Update complete.';