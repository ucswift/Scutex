ALTER TABLE [dbo].[Licenses]
    ADD CONSTRAINT [FK_Licenses_Services] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Services] ([ServiceId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

