CREATE TABLE [dbo].[LicenseSets] (
    [LicenseSetId] INT              IDENTITY (1, 1) NOT NULL,
    [LicenseId]    INT              NOT NULL,
    [Name]         NVARCHAR (50)    NOT NULL,
    [LicenseType]  INT              NOT NULL,
    [UniquePad]    UNIQUEIDENTIFIER NOT NULL,
    [MaxUsers]     INT              NULL
);

