CREATE VIEW [dbo].[SearchResultsView]
AS
SELECT     TOP (100) PERCENT dbo.SearchResults.ID, dbo.SearchResults.Artist, dbo.SearchResults.Title, dbo.SearchResults.VideoImageURL, dbo.SearchResults.Lyrics, 
                      dbo.SearchResults.VideoID, dbo.Videos.VideoImageURL AS Expr1, dbo.Videos.LyricID, dbo.Lyrics.Text
FROM         dbo.SearchResults INNER JOIN
                      dbo.Videos ON dbo.SearchResults.VideoID = dbo.Videos.ID INNER JOIN
                      dbo.Lyrics ON dbo.Videos.LyricID = dbo.Lyrics.ID
ORDER BY dbo.SearchResults.ID