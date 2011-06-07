ALTER TABLE [dbo].[LicenseKeys]
    ADD CONSTRAINT [DF_LicenseKeys_Deactivated] DEFAULT ((0)) FOR [Deactivated];

