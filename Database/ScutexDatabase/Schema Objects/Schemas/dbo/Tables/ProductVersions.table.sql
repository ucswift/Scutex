CREATE TABLE [dbo].[ProductVersions] (
    [ProductVersionId] INT           IDENTITY (1, 1) NOT NULL,
    [ProductId]        INT           NOT NULL,
    [VersionDisplay]   NVARCHAR (50) NOT NULL,
    [VersionMask]      NVARCHAR (50) NOT NULL
);

