CREATE TABLE [dbo].[SearchResults] (
    [ID]            NVARCHAR (256) NOT NULL,
    [Artist]        NVARCHAR (256) NULL,
    [Title]         NVARCHAR (256) NULL,
    [VideoImageURL] NVARCHAR (256) NULL,
    [Lyrics]        NTEXT          NULL,
    [VideoID]       CHAR (11)      NULL,
    CONSTRAINT [PK_SearchResults] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SearchResults_Videos] FOREIGN KEY ([VideoID]) REFERENCES [dbo].[Videos] ([ID]) ON UPDATE CASCADE
);


