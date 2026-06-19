CREATE TABLE [dbo].[CelulaLideres]
(
    [Id] INT IDENTITY(1,1) NOT NULL,
    [CelulaId] INT NOT NULL,
    [ConsultorId] INT NOT NULL,
    [FechaAsignacion] DATETIME2 NOT NULL CONSTRAINT [DF_CelulaLideres_FechaAsignacion] DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_CelulaLideres] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CelulaLideres_Celula] FOREIGN KEY ([CelulaId]) REFERENCES [dbo].[Celulas] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CelulaLideres_Consultor] FOREIGN KEY ([ConsultorId]) REFERENCES [dbo].[Consultores] ([Id]) ON DELETE NO ACTION
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_CelulaLideres_CelulaConsultor]
    ON [dbo].[CelulaLideres] ([CelulaId] ASC, [ConsultorId] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_CelulaLideres_ConsultorId]
    ON [dbo].[CelulaLideres] ([ConsultorId] ASC);
GO
