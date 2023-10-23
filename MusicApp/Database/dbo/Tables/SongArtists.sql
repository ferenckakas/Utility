CREATE TABLE [dbo].[SongArtists] (
    [SongID]      INT     NOT NULL,
    [ArtistID]    INT     NOT NULL,
    [IsFeaturing] BIT     NULL,
    [Number]      TINYINT NULL,
    CONSTRAINT [PK_SongArtists] PRIMARY KEY CLUSTERED ([SongID] ASC, [ArtistID] ASC),
    CONSTRAINT [FK_SongArtists_Artists] FOREIGN KEY ([ArtistID]) REFERENCES [dbo].[Artists] ([ID]),
    CONSTRAINT [FK_SongArtists_Songs] FOREIGN KEY ([SongID]) REFERENCES [dbo].[Songs] ([ID])
);

