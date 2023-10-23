CREATE TABLE [dbo].[AlbumSongs] (
    [AlbumID] INT     NOT NULL,
    [SongID]  INT     NOT NULL,
    [Number]  TINYINT NULL,
    CONSTRAINT [PK_AlbumSongs] PRIMARY KEY CLUSTERED ([AlbumID] ASC, [SongID] ASC),
    CONSTRAINT [FK_AlbumSongs_Albums] FOREIGN KEY ([AlbumID]) REFERENCES [dbo].[Albums] ([ID]),
    CONSTRAINT [FK_AlbumSongs_Songs] FOREIGN KEY ([SongID]) REFERENCES [dbo].[Songs] ([ID])
);

