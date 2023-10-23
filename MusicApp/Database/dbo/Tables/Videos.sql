CREATE TABLE [dbo].[Videos] (
    [ID]            CHAR (11)      NOT NULL,
    [VideoImageURL] NVARCHAR (256) NOT NULL,
    [LyricID]       INT            NULL,
    CONSTRAINT [PK_Videos] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Videos_Lyrics] FOREIGN KEY ([LyricID]) REFERENCES [dbo].[Lyrics] ([ID]) ON DELETE SET NULL ON UPDATE CASCADE
);


