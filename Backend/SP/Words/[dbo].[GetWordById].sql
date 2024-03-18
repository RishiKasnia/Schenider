CREATE PROCEDURE [dbo].[GetWordById]
@WordId int
AS
BEGIN

SET NOCOUNT ON

SELECT Wr.*, Sy.* FROM Word Wr INNER JOIN [Synonym] Sy ON Wr.WordId = Sy.WordId where Wr.WordId = @WordId

END