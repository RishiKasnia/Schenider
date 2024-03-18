ALTER PROCEDURE [dbo].[GetAllWords]
 @PageSize INT,
 @PageNum  INT
AS
BEGIN

SET NOCOUNT ON

select * from (select * from Word  order by Title OFFSET (@PageNum-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY)  as wr
inner join  [synonym] sy
on wr.WordId = sy.WordId

END