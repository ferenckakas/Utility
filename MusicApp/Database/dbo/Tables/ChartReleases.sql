CREATE TABLE [dbo].[ChartReleases] (
    [ID]       INT  IDENTITY (1, 1) NOT NULL,
    [ChartID]  INT  NOT NULL,
    [Released] DATE NOT NULL,
    CONSTRAINT [PK_ChartReleases] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ChartReleases_Charts] FOREIGN KEY ([ChartID]) REFERENCES [dbo].[Charts] ([ID])
);

