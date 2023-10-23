CREATE TABLE [dbo].[Session] (
    [Key]   NVARCHAR (255) NOT NULL,
    [Value] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED ([Key] ASC)
);

