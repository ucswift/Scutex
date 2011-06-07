ALTER TABLE [dbo].[LicenseKeys]
    ADD CONSTRAINT [DF_LicenseKeys_ActivationCount] DEFAULT ((0)) FOR [ActivationCount];

