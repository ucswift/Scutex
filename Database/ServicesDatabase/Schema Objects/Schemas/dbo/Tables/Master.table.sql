CREATE TABLE [dbo].[Master] (
    [MasterId]              INT              IDENTITY (1, 1) NOT NULL,
    [ServiceId]             UNIQUEIDENTIFIER NOT NULL,
    [ClientInboundKey]      VARCHAR (MAX)    NOT NULL,
    [ClientOutboundKey]     VARCHAR (MAX)    NOT NULL,
    [ManagementInboundKey]  VARCHAR (MAX)    NOT NULL,
    [ManagementOutboundKey] VARCHAR (MAX)    NOT NULL,
    [Token]                 VARCHAR (MAX)    NOT NULL,
    [Initialized]           BIT              NOT NULL
);



