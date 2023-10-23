CREATE TABLE [dbo].[ChartReleaseSongs] (
    [ChartReleaseID] INT      NOT NULL,
    [SongID]         INT      NOT NULL,
    [Number]         SMALLINT NOT NULL,
    CONSTRAINT [PK_ChartReleaseSongs] PRIMARY KEY CLUSTERED ([ChartReleaseID] ASC, [SongID] ASC),
    CONSTRAINT [FK_ChartReleaseSongs_ChartReleases] FOREIGN KEY ([ChartReleaseID]) REFERENCES [dbo].[ChartReleases] ([ID]),
    CONSTRAINT [FK_ChartReleaseSongs_Songs] FOREIGN KEY ([SongID]) REFERENCES [dbo].[Songs] ([ID])
);

