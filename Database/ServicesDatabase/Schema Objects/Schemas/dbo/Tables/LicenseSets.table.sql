CREATE TABLE [dbo].[LicenseSets] (
    [LicenseSetId] INT           NOT NULL,
    [LicenseId]    INT           NOT NULL,
    [Name]         VARCHAR (250) NOT NULL,
    [LicenseType]  INT           NOT NULL,
    [MaxUsers]     INT           NULL
);

