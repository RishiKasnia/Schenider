CREATE PROCEDURE [dbo].[GetAllWordsCount]
AS
BEGIN

SELECT COUNT(1) from Word

END