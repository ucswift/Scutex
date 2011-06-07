ALTER TABLE [dbo].[LicenseKeys]
    ADD CONSTRAINT [DF_LicenseKeys_CreatedOn] DEFAULT (getdate()) FOR [CreatedOn];

