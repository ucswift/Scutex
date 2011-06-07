CREATE TABLE [dbo].[Services] (
    [ServiceId]                    INT              IDENTITY (1, 1) NOT NULL,
    [Name]                         VARCHAR (100)    NOT NULL,
    [ClientUrl]                    VARCHAR (250)    NOT NULL,
    [ManagementUrl]                VARCHAR (250)    NOT NULL,
    [Token]                        VARCHAR (MAX)    NOT NULL,
    [InboundPublicKey]             VARCHAR (MAX)    NOT NULL,
    [InboundPrivateKey]            VARCHAR (MAX)    NOT NULL,
    [OutboundPublicKey]            VARCHAR (MAX)    NOT NULL,
    [OutboundPrivateKey]           VARCHAR (MAX)    NOT NULL,
    [ManagementInboundPublicKey]   VARCHAR (MAX)    NOT NULL,
    [ManagementInboundPrivateKey]  VARCHAR (MAX)    NOT NULL,
    [ManagementOutboundPublicKey]  VARCHAR (MAX)    NOT NULL,
    [ManagementOutboundPrivateKey] VARCHAR (MAX)    NOT NULL,
    [UniquePad]                    UNIQUEIDENTIFIER NOT NULL,
    [Initialized]                  BIT              NOT NULL,
    [Tested]                       BIT              NOT NULL,
    [LockToIp]                     BIT              NOT NULL,
    [ClientRequestToken]           VARCHAR (250)    NOT NULL,
    [ManagementRequestToken]       VARCHAR (250)    NOT NULL,
    [CreatedDate]                  DATETIME         NOT NULL
);

