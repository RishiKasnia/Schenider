CREATE PROCEDURE [dbo].[InsertSynonym]
@Title nvarchar(200),
@WordId int
AS
BEGIN

SET NOCOUNT ON

INSERT INTO [Synonym](WordId, Title) Values( @WordId, @Title)
SELECT SCOPE_IDENTITY()
END