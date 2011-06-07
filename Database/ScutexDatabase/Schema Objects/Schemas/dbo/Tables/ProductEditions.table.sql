CREATE TABLE [dbo].[ProductEditions] (
    [ProductEditionId] INT           IDENTITY (1, 1) NOT NULL,
    [ProductId]        INT           NOT NULL,
    [Name]             NVARCHAR (50) NOT NULL
);

