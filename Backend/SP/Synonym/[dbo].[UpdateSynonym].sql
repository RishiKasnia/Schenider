CREATE PROCEDURE [dbo].[UpdateSynonym]
@SynonymId int,
@Title nvarchar(200)
AS
BEGIN

UPDATE [Synonym] SET Title = @Title WHERE SynonymId = @SynonymId
END