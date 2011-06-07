ALTER TABLE [dbo].[Licenses]
    ADD CONSTRAINT [FK_Licenses_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

