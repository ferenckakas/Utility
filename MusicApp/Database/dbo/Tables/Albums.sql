CREATE TABLE [dbo].[Albums] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (256) NOT NULL,
    [ArtistID] INT            NOT NULL,
    [Released] DATE           NULL,
    [WikiURL]  VARCHAR (256)  NULL,
    [ImageURL] VARCHAR (256)  NULL,
    CONSTRAINT [PK_Albums] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Albums_Artists] FOREIGN KEY ([ArtistID]) REFERENCES [dbo].[Artists] ([ID])
);

