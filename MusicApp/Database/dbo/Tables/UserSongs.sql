CREATE TABLE [dbo].[UserSongs] (
    [UserID]   INT     NOT NULL,
    [SongID]   INT     NOT NULL,
    [Listened] BIT     NULL,
    [Rating]   TINYINT NULL,
    [ITunesID] INT     NULL,
    CONSTRAINT [PK_UserSongs] PRIMARY KEY CLUSTERED ([UserID] ASC, [SongID] ASC),
    CONSTRAINT [FK_UserSongs_Songs] FOREIGN KEY ([SongID]) REFERENCES [dbo].[Songs] ([ID]),
    CONSTRAINT [FK_UserSongs_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([ID])
);

