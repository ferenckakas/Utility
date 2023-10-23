CREATE TABLE [dbo].[Lyrics]
(
	[ID] [int] NOT NULL IDENTITY,
	[Text] [nvarchar](max) NOT NULL, 
    CONSTRAINT [PK_Lyrics] PRIMARY KEY ([ID])
)
