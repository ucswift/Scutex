ALTER TABLE [dbo].[Features]
    ADD CONSTRAINT [FK_Features_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

