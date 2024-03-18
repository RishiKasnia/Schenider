CREATE PROCEDURE [dbo].[GetSynonymById]
@SynonymId int
AS
BEGIN

SET NOCOUNT ON
SELECT * FROM [Synonym] WHERE SynonymId = @SynonymId
END