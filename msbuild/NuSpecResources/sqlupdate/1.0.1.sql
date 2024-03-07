--beginvalidatingquery
if exists (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tblFindDatabaseVersion')
    begin
		select 0, 'Already correct database version'
	end
	else
		select 1, 'Upgrading database'
--endvalidatingquery

GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tblFindDatabaseVersion' AND TABLE_SCHEMA = N'dbo')
BEGIN
	CREATE TABLE [dbo].[tblFindDatabaseVersion](
		[Id] int Identity(1,1) PRIMARY KEY,
		[Major] [int] NOT NULL,
		[Minor] [int] NOT NULL,
		[Patch] [int] NOT NULL
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.types WHERE is_table_type = 1 AND name ='findIDTable') 
BEGIN
	CREATE TYPE [dbo].[findIDTable] AS TABLE(
		[ID] [int] NOT NULL
	)
END
GO

insert into tblFindDatabaseVersion(Major, Minor, Patch) values(1,0,1)


GO

PRINT N'Update complete.';