CREATE TABLE [dbo].[Notificaciones]
(
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Modulo] NVARCHAR(100) NOT NULL,
    [Tipo] NVARCHAR(50) NOT NULL,
    [Destinatario] NVARCHAR(200) NOT NULL,
    [Asunto] NVARCHAR(300) NOT NULL,
    [Cuerpo] NVARCHAR(MAX) NOT NULL,
    [Enviado] BIT NOT NULL CONSTRAINT [DF_Notificaciones_Enviado] DEFAULT ((0)),
    [FechaEnvio] DATETIME2 NULL,
    [FechaProgramada] DATETIME2 NULL,
    [Intentos] INT NOT NULL CONSTRAINT [DF_Notificaciones_Intentos] DEFAULT ((0)),
    [ErrorMensaje] NVARCHAR(MAX) NULL,
    [AdjuntoUrl] NVARCHAR(500) NULL,
    [Activo] BIT NOT NULL CONSTRAINT [DF_Notificaciones_Activo] DEFAULT ((1)),
    [FechaCreacion] DATETIME2 NOT NULL CONSTRAINT [DF_Notificaciones_FechaCreacion] DEFAULT (GETUTCDATE()),
    [FechaModificacion] DATETIME2 NULL,
    [CreadoPor] NVARCHAR(450) NULL,
    [ModificadoPor] NVARCHAR(450) NULL,
    CONSTRAINT [PK_Notificaciones] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_Notificaciones_Enviado]
    ON [dbo].[Notificaciones] ([Enviado] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Notificaciones_FechaProgramada]
    ON [dbo].[Notificaciones] ([FechaProgramada] ASC);
GO
