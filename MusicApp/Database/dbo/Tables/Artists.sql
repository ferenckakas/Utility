CREATE TABLE [dbo].[Artists] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (128) NOT NULL,
    [WikiURL]         VARCHAR (256)  NULL,
    [ImageURL]        VARCHAR (256)  NULL,
    [ConnectedID]     INT            NULL,
    [RefreshInterval] INT            NULL,
    [Refreshed]       DATETIME2 (7)  NULL,
    CONSTRAINT [PK_Artists_1] PRIMARY KEY CLUSTERED ([ID] ASC)
);

