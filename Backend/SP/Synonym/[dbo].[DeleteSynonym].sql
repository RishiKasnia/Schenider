CREATE PROCEDURE [dbo].[DeleteSynonym]
@SynonymId int
AS
BEGIN


DELETE FROM [Synonym] WHERE SynonymId = @SynonymId
END