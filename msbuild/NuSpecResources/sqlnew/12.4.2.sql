--beginvalidatingquery 
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tblFindDatabaseVersion') 
    BEGIN 
    declare @major int = 12, @minor int = 4, @patch int = 2   
    IF EXISTS (SELECT 1 FROM dbo.tblFindDatabaseVersion WHERE Major = @major AND Minor = @minor AND Patch = @patch) 
        select 0,'Already correct database version' 
    ELSE 
        select 1, 'Upgrading database' 
    END 
ELSE 
    select -1, 'Not an EPiServer database with Find' 
--endvalidatingquery 
 
GO 

CREATE TABLE [dbo].[tblFindIndexQueue](
	[Action] [smallint] NOT NULL,
	[Cascade] [bit] NOT NULL,
	[EnableLanguageFilter] [bit] NOT NULL,
	[Item] [nvarchar](255) NOT NULL,
	[Language] [nvarchar](255) NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Hash] [int] NULL,
	[LastRead] [datetime] NULL
) ON [PRIMARY]

GO 

CREATE PROCEDURE [dbo].[findIndexQueueLoadAll]
(
    @currentTime datetime
)
 AS 

BEGIN
	
	SET NOCOUNT ON
	
	UPDATE tblFindIndexQueue SET LastRead = @currentTime
	SELECT Action, [Cascade], EnableLanguageFilter, Item, Language, TimeStamp, Hash FROM tblFindIndexQueue ORDER BY TimeStamp

END 



GO 



CREATE PROCEDURE [dbo].[findIndexQueueLoadItems]
(
    @items INT,
    @currentTime datetime,
    @acceptLastReadOlderThan datetime
)
AS 

BEGIN
	
	SET NOCOUNT ON
	
	DECLARE @hashes TABLE(tempHash INT);

	INSERT INTO @hashes 
		SELECT TOP (@items) Hash FROM tblFindIndexQueue WHERE (LastRead IS NULL OR LastRead < @acceptLastReadOlderThan) ORDER BY TimeStamp

	UPDATE tblFindIndexQueue SET LastRead = @currentTime WHERE Hash in(SELECT tempHash FROM @hashes)
	SELECT Action, [Cascade], EnableLanguageFilter, Item, [Language], TimeStamp, Hash FROM tblFindIndexQueue WHERE Hash in(SELECT tempHash FROM @hashes)
END 


GO 


CREATE PROCEDURE [dbo].[findIndexQueueDeleteItem]
(
    @hash INT
)
AS 

BEGIN

	SET NOCOUNT ON
	
	delete from tblFindIndexQueue where [Hash] = @hash 

END 

GO 



CREATE PROCEDURE [dbo].[findIndexQueueDeleteItems]
(
    @hashes findIDTable READONLY
)
AS
 

BEGIN
	
	SET NOCOUNT ON
	
	delete from tblFindIndexQueue where [Hash] in(select [ID] from @hashes)

END 



GO



CREATE PROCEDURE [dbo].[findIndexQueueSave]
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
	
	if not exists(select * from tblFindIndexQueue where Hash=@hash AND LastRead IS NULL) 
		BEGIN 
			insert into tblFindIndexQueue(Action, [Cascade], EnableLanguageFilter, Item, [Language], TimeStamp, [Hash]) values(@action, @cascade, @enableLanguageFilter, @item, @itemlanguage, @timeStamp, @hash) 
		END 
	else
		BEGIN
			update tblFindIndexQueue set TimeStamp = @timeStamp where Hash=@hash
		END
END 


GO

-- From git history I saw that we only have an other new sql file, for version 12.2.8, it has been removed and replaced by v12.4.2.
-- But in update script we still check exit for v12.2.8 before run update script, so I insert a record for v12.2.8 in v12.4.2 new script,
-- to prevent v12.2.8 update script execution in the future
IF NOT EXISTS (SELECT 1 FROM dbo.tblFindDatabaseVersion WHERE Major = 12 AND Minor = 2 AND Patch = 8)
	INSERT INTO dbo.tblFindDatabaseVersion(Major, Minor, Patch) VALUES(12,2,8)

insert into tblFindDatabaseVersion(Major, Minor, Patch) values(12,4,2)

GO

PRINT N'Update complete.';