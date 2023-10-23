CREATE TABLE [dbo].[PlaylistSongs] (
    [PlaylistID] INT      NOT NULL,
    [SongID]     INT      NOT NULL,
    [Number]     SMALLINT NULL,
    CONSTRAINT [PK_PlaylistSongs] PRIMARY KEY CLUSTERED ([PlaylistID] ASC, [SongID] ASC),
    CONSTRAINT [FK_PlaylistSongs_Playlists] FOREIGN KEY ([PlaylistID]) REFERENCES [dbo].[Playlists] ([ID]),
    CONSTRAINT [FK_PlaylistSongs_Songs] FOREIGN KEY ([SongID]) REFERENCES [dbo].[Songs] ([ID])
);



