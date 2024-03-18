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