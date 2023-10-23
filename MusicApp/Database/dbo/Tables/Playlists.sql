CREATE TABLE [dbo].[Playlists] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (300) NULL,
    [IsFolder]    BIT            NOT NULL,
    [ParentID]    INT            NULL,
    [UserID]      INT            NULL,
    CONSTRAINT [PK_Playlists] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Playlists_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([ID])
);



