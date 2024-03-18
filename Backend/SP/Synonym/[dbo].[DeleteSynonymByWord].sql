CREATE PROCEDURE [dbo].[DeleteSynonymByWord]
@WordId int
AS
BEGIN

DELETE FROM [Synonym] WHERE WordId = @WordId
END