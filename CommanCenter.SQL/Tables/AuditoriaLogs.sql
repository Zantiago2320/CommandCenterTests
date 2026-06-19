CREATE TABLE [dbo].[AuditoriaLogs]
(
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Modulo] NVARCHAR(100) NOT NULL,
    [Accion] NVARCHAR(50) NOT NULL,
    [Entidad] NVARCHAR(100) NOT NULL,
    [EntidadId] NVARCHAR(100) NULL,
    [CampoModificado] NVARCHAR(MAX) NULL,
    [ValorAnterior] NVARCHAR(MAX) NULL,
    [ValorNuevo] NVARCHAR(MAX) NULL,
    [UsuarioId] NVARCHAR(450) NULL,
    [UsuarioEmail] NVARCHAR(200) NULL,
    [UsuarioRol] NVARCHAR(MAX) NULL,
    [Razon] NVARCHAR(MAX) NULL,
    [IpAddress] NVARCHAR(50) NULL,
    [UserAgent] NVARCHAR(500) NULL,
    [Exitoso] BIT NOT NULL CONSTRAINT [DF_AuditoriaLogs_Exitoso] DEFAULT ((1)),
    [MensajeError] NVARCHAR(MAX) NULL,
    [Activo] BIT NOT NULL CONSTRAINT [DF_AuditoriaLogs_Activo] DEFAULT ((1)),
    [FechaCreacion] DATETIME2 NOT NULL CONSTRAINT [DF_AuditoriaLogs_FechaCreacion] DEFAULT (GETUTCDATE()),
    [FechaModificacion] DATETIME2 NULL,
    [CreadoPor] NVARCHAR(450) NULL,
    [ModificadoPor] NVARCHAR(450) NULL,
    CONSTRAINT [PK_AuditoriaLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_AuditoriaLogs_Modulo]
    ON [dbo].[AuditoriaLogs] ([Modulo] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_AuditoriaLogs_FechaCreacion]
    ON [dbo].[AuditoriaLogs] ([FechaCreacion] DESC);
GO

CREATE NONCLUSTERED INDEX [IX_AuditoriaLogs_UsuarioId]
    ON [dbo].[AuditoriaLogs] ([UsuarioId] ASC);
GO
