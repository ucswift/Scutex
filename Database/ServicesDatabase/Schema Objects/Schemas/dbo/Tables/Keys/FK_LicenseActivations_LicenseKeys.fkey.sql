ALTER TABLE [dbo].[LicenseActivations]
    ADD CONSTRAINT [FK_LicenseActivations_LicenseKeys] FOREIGN KEY ([LicenseKeyId]) REFERENCES [dbo].[LicenseKeys] ([LicenseKeyId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

