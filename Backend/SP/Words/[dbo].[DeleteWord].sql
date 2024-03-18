CREATE PROCEDURE [dbo].[DeleteWord]
@WordId int
AS
BEGIN


DELETE FROM [Synonym] WHERE WordId = @WordId
DELETE FROM Word WHERE WordId = @WordId
 
END