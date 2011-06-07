ALTER TABLE [dbo].[TrialSettings]
    ADD CONSTRAINT [FK_TrialSettings_Licenses] FOREIGN KEY ([LicenseId]) REFERENCES [dbo].[Licenses] ([LicenseId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

