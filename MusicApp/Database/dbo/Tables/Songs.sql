CREATE TABLE [dbo].[Songs] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Hash]        NVARCHAR (256) NULL,
    [ArtistID]    INT            NULL,
    [ArtistName]  NVARCHAR (128) NULL,
    [Title]       NVARCHAR (256) NOT NULL,
    [Version]     NVARCHAR (64)  NULL,
    [Released]    DATE           NULL,
    [VideoID]     CHAR (11)      NULL,
    [SpotifyID]   VARCHAR (50)   NULL,
    [WikiURL]     VARCHAR (256)  NULL,
    [ImageURL]    VARCHAR (256)  NULL,
    [ConnectedID] INT            NULL,
    [IsLocked]    BIT            NOT NULL,
    CONSTRAINT [PK_Artists] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Songs_Artists] FOREIGN KEY ([ArtistID]) REFERENCES [dbo].[Artists] ([ID]),
    CONSTRAINT [FK_Songs_Videos] FOREIGN KEY ([VideoID]) REFERENCES [dbo].[Videos] ([ID])
);







