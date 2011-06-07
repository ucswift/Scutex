ALTER TABLE [dbo].[TrialSettings]
    ADD CONSTRAINT [FK_TrialSettings_TrialExpirationOptions] FOREIGN KEY ([TrialExpirationOptionId]) REFERENCES [dbo].[TrialExpirationOptions] ([TrialExpirationOptionId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

