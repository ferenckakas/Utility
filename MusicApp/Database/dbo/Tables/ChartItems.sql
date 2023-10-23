CREATE TABLE [dbo].[ChartItems] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [ChartReleaseID] INT            NOT NULL,
    [ArtistName]     NVARCHAR (128) NOT NULL,
    [Title]          NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_ChartItems] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ChartItems_ChartReleases] FOREIGN KEY ([ChartReleaseID]) REFERENCES [dbo].[ChartReleases] ([ID])
);

