CREATE TABLE [dbo].[TrialSettings] (
    [TrialSettingId]          INT          IDENTITY (1, 1) NOT NULL,
    [LicenseId]               INT          NOT NULL,
    [TrialExpirationOptionId] INT          NOT NULL,
    [ExpirationData]          VARCHAR (50) NOT NULL
);

