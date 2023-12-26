--beginvalidatingquery 
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tblFindDatabaseVersion') 
    BEGIN 
    declare @major int = 12, @minor int = 2, @patch int = 8    
    IF EXISTS (SELECT 1 FROM dbo.tblFindDatabaseVersion WHERE Major = @major AND Minor = @minor AND Patch = @patch) 
        select 0,'Already correct database version' 
    ELSE 
        select 1, 'Upgrading database' 
    END 
ELSE 
    select -1, 'Not an EPiServer database with Find' 
--endvalidatingquery 
 
GO 

-- This should check tblFindIndexQueue table exist before create new table
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tblFindIndexQueue' AND TABLE_SCHEMA = N'dbo')
BEGIN
	CREATE TABLE [dbo].[tblFindIndexQueue](
		[Action] [smallint] NOT NULL,
		[Cascade] [bit] NOT NULL,
		[EnableLanguageFilter] [bit] NOT NULL,
		[Item] [nvarchar](255) NOT NULL,
		[Language] [nvarchar](255) NULL,
		[TimeStamp] [datetime] NOT NULL,
		[Hash] [int] NULL
	) ON [PRIMARY]
END
GO 

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[findIndexQueueLoadAll]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE [dbo].[findIndexQueueLoadAll]
GO 


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[findIndexQueueLoadAll]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) 
BEGIN
    DROP PROCEDURE [dbo].[findIndexQueueLoadAll]
END
GO 

CREATE PROCEDURE [dbo].[findIndexQueueLoadAll]
AS
    BEGIN        
        SET NOCOUNT ON        
        select Action, [Cascade], EnableLanguageFilter, Item, Language, TimeStamp, Hash from tblFindIndexQueue ORDER BY TimeStamp
    END
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[findIndexQueueLoadItems]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) 
BEGIN
    DROP PROCEDURE [dbo].[findIndexQueueLoadItems]
END
GO 
CREATE PROCEDURE [dbo].[findIndexQueueLoadItems]
    (
        @items INT
    )
    AS
    BEGIN        
        SET NOCOUNT ON        
        select top (@items) Action, [Cascade], EnableLanguageFilter, Item, [Language], TimeStamp, Hash from tblFindIndexQueue ORDER BY TimeStamp
    END
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[findIndexQueueDeleteItem]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) 
BEGIN
    DROP PROCEDURE [dbo].[findIndexQueueDeleteItem]
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
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[findIndexQueueDeleteItems]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) 
BEGIN
    DROP PROCEDURE [dbo].[findIndexQueueDeleteItems]
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

GO 

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[findIndexQueueSave]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) 
BEGIN
    DROP PROCEDURE [dbo].[findIndexQueueSave]
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
        if not exists(select * from tblFindIndexQueue where Hash=@hash) 
        BEGIN 

            insert into tblFindIndexQueue(Action, [Cascade], EnableLanguageFilter, Item, [Language], TimeStamp, [Hash]) values(@action, @cascade, @enableLanguageFilter, @item, @itemlanguage, @timeStamp, @hash) 
        
        END 

    END
GO

DELETE tblFindDatabaseVersion
WHERE Major = 12 AND Minor = 4 AND Patch = 2
GO

insert into tblFindDatabaseVersion(Major, Minor, Patch) values(12,2,8)
GO

PRINT N'Update complete.';