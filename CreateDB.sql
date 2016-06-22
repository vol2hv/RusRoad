CREATE TABLE [dbo].[CarOwner] (
    [CarOwner_Id] INT           IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50) NOT NULL,
    [Govnumber]   NVARCHAR(10)    NOT NULL,
    PRIMARY KEY CLUSTERED ([CarOwner_Id] ASC),
    CONSTRAINT [AK_Govnumber] UNIQUE NONCLUSTERED ([Govnumber] ASC)
);

SET IDENTITY_INSERT [dbo].[CarOwner] ON
INSERT INTO [dbo].[CarOwner] ([CarOwner_Id], [Name], [Govnumber]) VALUES (1, N'Путин Владимир Владимирович', N'О999ОО178 ')
INSERT INTO [dbo].[CarOwner] ([CarOwner_Id], [Name], [Govnumber]) VALUES (2, N'Бойченко Игорь Алексеевич', N'А888АА36  ')
INSERT INTO [dbo].[CarOwner] ([CarOwner_Id], [Name], [Govnumber]) VALUES (3, N'Пупкин Василий Алибабаевич', N'С666СС48  ')
INSERT INTO [dbo].[CarOwner] ([CarOwner_Id], [Name], [Govnumber]) VALUES (4, N'Мадорин Владимир Иванович', N'К999КК36  ')
SET IDENTITY_INSERT [dbo].[CarOwner] OFF


CREATE TABLE [dbo].[Highway] (
    [Highway_Id] INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (30) NOT NULL,
    [Speed]      INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Highway_Id] ASC)
);

SET IDENTITY_INSERT [dbo].[Highway] ON
INSERT INTO [dbo].[Highway] ([Highway_Id], [Name], [Speed]) VALUES (1, N'М1 Беларусь', 100)
INSERT INTO [dbo].[Highway] ([Highway_Id], [Name], [Speed]) VALUES (2, N'М2  Крым', 90)
INSERT INTO [dbo].[Highway] ([Highway_Id], [Name], [Speed]) VALUES (3, N'М3 Украина', 110)
INSERT INTO [dbo].[Highway] ([Highway_Id], [Name], [Speed]) VALUES (4, N'М4 Дон', 120)
SET IDENTITY_INSERT [dbo].[Highway] OFF

CREATE TABLE [dbo].[Passage] (
    [Passage_Id]  BIGINT   IDENTITY (1, 1) NOT NULL,
    [Time]        DATETIME NOT NULL,
    [CarOwner_Id] INT      NOT NULL,
    [Highway_Id]  INT      NOT NULL,
    [Speed]       INT      NOT NULL,
    CONSTRAINT [PK_Passage] PRIMARY KEY CLUSTERED ([Passage_Id] ASC),
    CONSTRAINT [AK_Passage] UNIQUE NONCLUSTERED ([Time] ASC, [CarOwner_Id] ASC, [Highway_Id] ASC),
    CONSTRAINT [FK_Passage_CarOwner] FOREIGN KEY ([CarOwner_Id]) REFERENCES [dbo].[CarOwner] ([CarOwner_Id]),
    CONSTRAINT [FK_Passage_Highway] FOREIGN KEY ([Highway_Id]) REFERENCES [dbo].[Highway] ([Highway_Id])
);

SET IDENTITY_INSERT [dbo].[Passage] ON
INSERT INTO [dbo].[Passage] ([Passage_Id], [Time], [CarOwner_Id], [Highway_Id], [Speed]) VALUES (1, N'2014-01-01 00:00:00', 1, 1, 188)
INSERT INTO [dbo].[Passage] ([Passage_Id], [Time], [CarOwner_Id], [Highway_Id], [Speed]) VALUES (2, N'2015-01-01 00:01:01', 1, 2, 199)
SET IDENTITY_INSERT [dbo].[Passage] OFF
