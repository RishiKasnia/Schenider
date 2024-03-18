CREATE PROCEDURE [dbo].[GetWordByTitle]
@Title nvarchar(200)
AS
BEGIN

SET NOCOUNT ON

SELECT Wr.*, Sy.* FROM Word Wr INNER JOIN [Synonym] Sy ON Wr.WordId = Sy.WordId where Wr.Title = @Title

END