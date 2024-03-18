CREATE PROCEDURE [dbo].[InsertSynonym]
@SynonymEntries [dbo].[IdTitleList] READONLY
AS
BEGIN


INSERT INTO [Synonym] SELECT Id , Title from  @SynonymEntries
END