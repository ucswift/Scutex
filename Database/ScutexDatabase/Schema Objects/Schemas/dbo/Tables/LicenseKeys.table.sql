CREATE TABLE [dbo].[LicenseKeys] (
    [LicenseKeyId]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [LicenseSetId]     INT            NOT NULL,
    [LicenseKey]       NVARCHAR (500) NOT NULL,
    [HashedLicenseKey] VARCHAR (500)  NOT NULL,
    [CreatedOn]        DATETIME       NOT NULL
);



