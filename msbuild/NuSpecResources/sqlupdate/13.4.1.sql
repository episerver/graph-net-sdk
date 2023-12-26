--beginvalidatingquery 
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tblFindDatabaseVersion') 
    BEGIN 
    declare @major int = 13, @minor int = 4, @patch int = 1 
    IF EXISTS (SELECT 1 FROM dbo.tblFindDatabaseVersion WHERE Major = @major AND Minor = @minor AND Patch = @patch) 
        SELECT 0,'Already correct database version' 
    ELSE 
        SELECT 1, 'Upgrading database' 
    END 
ELSE 
    SELECT -1, 'Not an EPiServer database with Find' 
--endvalidatingquery 
 
GO 

ALTER PROCEDURE [dbo].[findIndexQueueSave]
(
    @action int,
    @cascade bit,
    @enableLanguageFilter bit,
    @item nvarchar(255),
    @itemlanguage nvarchar(255),
    @timeStamp datetime,
    @hash int
)
AS

BEGIN
    SET NOCOUNT ON
   
    if not exists(select Item from tblFindIndexQueue where Hash=@hash AND LastRead IS NULL)
        BEGIN
            insert into tblFindIndexQueue(Action, [Cascade], EnableLanguageFilter, Item, [Language], TimeStamp, [Hash]) values(@action, @cascade, @enableLanguageFilter, @item, @itemlanguage, @timeStamp, @hash)
        END
END

GO 

INSERT INTO tblFindDatabaseVersion(Major, Minor, Patch) VALUES (13,4,1)

GO 

PRINT N'Update complete.';