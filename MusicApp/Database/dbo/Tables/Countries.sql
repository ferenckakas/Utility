CREATE TABLE [dbo].[Countries]
(
	[ID] [char](2) NOT NULL,
	[Name] [nvarchar](18) NOT NULL,
	[Population] [int] NOT NULL, 
    CONSTRAINT [PK_Countries] PRIMARY KEY ([ID])
)
