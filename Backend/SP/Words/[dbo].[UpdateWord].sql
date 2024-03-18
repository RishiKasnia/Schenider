CREATE PROCEDURE [dbo].[UpdateWord]
@WordId int,
@Title nvarchar(200),
@Description nvarchar(500)
AS
BEGIN

UPDATE Word SET Title = @Title, [Description]=@Description WHERE WordId = @WordId

END