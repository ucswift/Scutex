CREATE TABLE [dbo].[Features] (
    [FeatureId] INT              IDENTITY (1, 1) NOT NULL,
    [ProductId] INT              NOT NULL,
    [Name]      NVARCHAR (150)   NOT NULL,
    [UniquePad] UNIQUEIDENTIFIER NOT NULL
);

