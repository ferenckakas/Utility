CREATE TABLE [dbo].[Charts] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_Charts] PRIMARY KEY CLUSTERED ([ID] ASC)
);

