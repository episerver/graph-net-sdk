--beginvalidatingquery 
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tblFindDatabaseVersion') 
    BEGIN 
    declare @major int = 13, @minor int = 2, @patch int = 8 
    IF EXISTS (SELECT 1 FROM dbo.tblFindDatabaseVersion WHERE Major = @major AND Minor = @minor AND Patch = @patch) 
        SELECT 0,'Already correct database version' 
    ELSE 
        SELECT 1, 'Upgrading database' 
    END 
ELSE 
    SELECT -1, 'Not an EPiServer database with Find' 
--endvalidatingquery 
 
GO 

ALTER PROCEDURE [dbo].[findIndexQueueDeleteItem]
    (
        @hash INT
    )
    AS 
    BEGIN
        SET NOCOUNT ON	
	    delete from tblFindIndexQueue where [Hash] = @hash AND LastRead IS NOT NULL
    END

GO


ALTER PROCEDURE [dbo].[findIndexQueueDeleteItems]
    (
        @hashes findIDTable READONLY
    )
    AS
    BEGIN	
        SET NOCOUNT ON		 
	    delete from tblFindIndexQueue where [Hash] in(select [ID] from @hashes) AND LastRead IS NOT NULL
    END

GO


DROP INDEX [IDX_tblFindIndexQueue_Indexed] ON [dbo].[tblFindIndexQueue]
GO

CREATE NONCLUSTERED INDEX [IDX_tblFindIndexQueue_Indexed] ON [dbo].[tblFindIndexQueue]
(
	[Hash] ASC, [LastRead] ASC
)
GO
 

INSERT INTO tblFindDatabaseVersion(Major, Minor, Patch) VALUES (13,2,8)

GO

PRINT N'Update complete.';