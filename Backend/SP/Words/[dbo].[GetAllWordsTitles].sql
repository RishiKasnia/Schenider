CREATE PROCEDURE [dbo].[GetAllWordsTitles]

AS
BEGIN

SELECT Title FROM Word(NOLOCK) order by Title
END