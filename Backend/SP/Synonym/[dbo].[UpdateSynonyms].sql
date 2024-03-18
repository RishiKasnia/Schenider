CREATE PROCEDURE [dbo].[UpdateSynonyms]
@SynonymEntries [dbo].[IdTitleList] READONLY
AS
BEGIN


MERGE [Synonym] AS TARG
USING @SynonymEntries AS SOUR 
ON (TARG.SynonymId = SOUR.Id) 
--When records are matched, update the records if there is any change
WHEN MATCHED AND TARG.Title <> SOUR.Title 
THEN UPDATE SET TARG.Title = SOUR.Title;

END