CREATE TABLE [dbo].[Celulas]
(
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Nombre] NVARCHAR(100) NOT NULL,
    [Descripcion] NVARCHAR(500) NULL,
    [Color] NVARCHAR(20) NULL CONSTRAINT [DF_Celulas_Color] DEFAULT ('#28a745'),
    [ImagenUrl] NVARCHAR(500) NULL,
    [Activo] BIT NOT NULL CONSTRAINT [DF_Celulas_Activo] DEFAULT ((1)),
    [FechaCreacion] DATETIME2 NOT NULL CONSTRAINT [DF_Celulas_FechaCreacion] DEFAULT (GETUTCDATE()),
    [FechaModificacion] DATETIME2 NULL,
    [CreadoPor] NVARCHAR(450) NULL,
    [ModificadoPor] NVARCHAR(450) NULL,
    CONSTRAINT [PK_Celulas] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_Celulas_Activo]
    ON [dbo].[Celulas] ([Activo] ASC);
GO
