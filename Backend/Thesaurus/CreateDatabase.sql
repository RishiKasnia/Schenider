USE [master]
GO
/****** Object:  Database [ThesaurusDB]    Script Date: 18-06-2021 11:09:51 ******/
CREATE DATABASE [ThesaurusDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ThesaurusDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ThesaurusDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ThesaurusDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ThesaurusDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [ThesaurusDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ThesaurusDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ThesaurusDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ThesaurusDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ThesaurusDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ThesaurusDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ThesaurusDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [ThesaurusDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ThesaurusDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ThesaurusDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ThesaurusDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ThesaurusDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ThesaurusDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ThesaurusDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ThesaurusDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ThesaurusDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ThesaurusDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ThesaurusDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ThesaurusDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ThesaurusDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ThesaurusDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ThesaurusDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ThesaurusDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ThesaurusDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ThesaurusDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ThesaurusDB] SET  MULTI_USER 
GO
ALTER DATABASE [ThesaurusDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ThesaurusDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ThesaurusDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ThesaurusDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ThesaurusDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ThesaurusDB] SET QUERY_STORE = OFF
GO
USE [ThesaurusDB]
GO
/****** Object:  User [ThesaurusDBUser]    Script Date: 18-06-2021 11:09:51 ******/
CREATE USER [ThesaurusDBUser] FOR LOGIN [ThesaurusDBUser] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [ThesaurusDBUser]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [ThesaurusDBUser]
GO
/****** Object:  UserDefinedTableType [dbo].[IdTitleList]    Script Date: 18-06-2021 11:09:51 ******/
CREATE TYPE [dbo].[IdTitleList] AS TABLE(
	[Id] [int] NOT NULL,
	[Title] [nvarchar](200) NULL
)
GO
/****** Object:  Table [dbo].[Synonym]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Synonym](
	[SynonymId] [int] IDENTITY(1,1) NOT NULL,
	[WordId] [int] NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Synonym] PRIMARY KEY CLUSTERED 
(
	[SynonymId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Word]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Word](
	[WordId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_Word] PRIMARY KEY CLUSTERED 
(
	[WordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UX_Word] UNIQUE NONCLUSTERED 
(
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Synonym]  WITH CHECK ADD  CONSTRAINT [FK_Synonym_Word] FOREIGN KEY([WordId])
REFERENCES [dbo].[Word] ([WordId])
GO
ALTER TABLE [dbo].[Synonym] CHECK CONSTRAINT [FK_Synonym_Word]
GO
/****** Object:  StoredProcedure [dbo].[DeleteSynonym]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteSynonym]
@SynonymId int
AS
BEGIN


DELETE FROM [Synonym] WHERE SynonymId = @SynonymId

END
GO
/****** Object:  StoredProcedure [dbo].[DeleteSynonymByWord]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteSynonymByWord]
@WordId int
AS
BEGIN

DELETE FROM [Synonym] WHERE WordId = @WordId
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteWord]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteWord]
@WordId int
AS
BEGIN


DELETE FROM [Synonym] WHERE WordId = @WordId
DELETE FROM Word WHERE WordId = @WordId
 
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllSynonym]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllSynonym]
AS
BEGIN

SET NOCOUNT ON
SELECT * FROM [Synonym]
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllWords]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllWords]
 @PageSize INT,
 @PageNum  INT
AS
BEGIN

SET NOCOUNT ON

select * from (select * from Word  order by Title OFFSET (@PageNum-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY)  as wr
inner join  [synonym] sy
on wr.WordId = sy.WordId

END
GO
/****** Object:  StoredProcedure [dbo].[GetAllWordsCount]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllWordsCount]
AS
BEGIN

SELECT COUNT(1) from Word

END
GO
/****** Object:  StoredProcedure [dbo].[GetAllWordsTitles]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllWordsTitles]

AS
BEGIN

SELECT Title FROM Word(NOLOCK) order by Title
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllWordTitles]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllWordTitles]

AS
BEGIN

SET NOCOUNT ON

SELECT Word.Title FROM Word (NOLOCK) order by Title

END
GO
/****** Object:  StoredProcedure [dbo].[GetSynonymById]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetSynonymById]
@SynonymId int
AS
BEGIN

SET NOCOUNT ON
SELECT * FROM [Synonym] WHERE SynonymId = @SynonymId
END
GO
/****** Object:  StoredProcedure [dbo].[GetWordById]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetWordById]
@WordId int
AS
BEGIN

SET NOCOUNT ON

SELECT Wr.*, Sy.* FROM Word Wr INNER JOIN [Synonym] Sy ON Wr.WordId = Sy.WordId where Wr.WordId = @WordId

END
GO
/****** Object:  StoredProcedure [dbo].[GetWordByTitle]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetWordByTitle]
@Title nvarchar(200)
AS
BEGIN

SET NOCOUNT ON

SELECT Wr.*, Sy.* FROM Word Wr INNER JOIN [Synonym] Sy ON Wr.WordId = Sy.WordId where Wr.Title = @Title

END
GO
/****** Object:  StoredProcedure [dbo].[InsertSynonym]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertSynonym]
@SynonymEntries [dbo].[IdTitleList] READONLY
AS
BEGIN


INSERT INTO [Synonym] SELECT Id , Title from  @SynonymEntries
END
GO
/****** Object:  StoredProcedure [dbo].[InsertSynonyms]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertSynonyms]
@SynonymEntries [dbo].[IdTitleList] READONLY
AS
BEGIN

SET NOCOUNT ON

INSERT INTO [Synonym] SELECT Id , Title from  @SynonymEntries
END
GO
/****** Object:  StoredProcedure [dbo].[InsertWord]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertWord]
@Title nvarchar(200),
@Description nvarchar(500),
@SynonymEntries [dbo].[IdTitleList] READONLY
AS
BEGIN


DECLARE @WordId int

INSERT INTO Word (Title, [Description]) VALUES (@Title, @Description)

select @WordId = convert(int,scope_identity());

INSERT INTO [Synonym] (WordId,Title) SELECT @WordId ,Title from @SynonymEntries
 
 select @WordId 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateSynonym]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateSynonym]
@SynonymId int,
@Title nvarchar(200)
AS
BEGIN

UPDATE [Synonym] SET Title = @Title WHERE SynonymId = @SynonymId
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateSynonyms]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateSynonyms]
@SynonymEntries [dbo].[IdTitleList] READONLY
AS
BEGIN


MERGE [Synonym] AS TARG
USING @SynonymEntries AS SOUR 
ON (TARG.SynonymId = SOUR.Id) 
--When records are matched, update the records if there is any change
WHEN MATCHED AND TARG.Title <> SOUR.Title 
THEN UPDATE SET TARG.Title = SOUR.Title;

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateWord]    Script Date: 18-06-2021 11:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateWord]
@WordId int,
@Title nvarchar(200),
@Description nvarchar(500)
AS
BEGIN

UPDATE Word SET Title = @Title, [Description]=@Description WHERE WordId = @WordId

END
GO
USE [master]
GO
ALTER DATABASE [ThesaurusDB] SET  READ_WRITE 
GO
