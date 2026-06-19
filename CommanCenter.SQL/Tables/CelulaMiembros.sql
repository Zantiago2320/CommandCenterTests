CREATE TABLE [dbo].[CelulaMiembros]
(
    [Id] INT IDENTITY(1,1) NOT NULL,
    [CelulaId] INT NOT NULL,
    [ConsultorId] INT NOT NULL,
    [FechaAsignacion] DATETIME2 NOT NULL CONSTRAINT [DF_CelulaMiembros_FechaAsignacion] DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_CelulaMiembros] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CelulaMiembros_Celula] FOREIGN KEY ([CelulaId]) REFERENCES [dbo].[Celulas] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CelulaMiembros_Consultor] FOREIGN KEY ([ConsultorId]) REFERENCES [dbo].[Consultores] ([Id]) ON DELETE NO ACTION
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_CelulaMiembros_CelulaConsultor]
    ON [dbo].[CelulaMiembros] ([CelulaId] ASC, [ConsultorId] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_CelulaMiembros_ConsultorId]
    ON [dbo].[CelulaMiembros] ([ConsultorId] ASC);
GO
