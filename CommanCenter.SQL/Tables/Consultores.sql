CREATE TABLE [dbo].[Consultores]
(
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Cedula] NVARCHAR(30) NULL,
    [Nombre] NVARCHAR(100) NOT NULL,
    [Apellido] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(200) NOT NULL,
    [Telefono] NVARCHAR(20) NULL,
    [Celular] NVARCHAR(20) NULL,
    [Cargo] NVARCHAR(150) NULL,
    [Rol] NVARCHAR(100) NULL,
    [Tecnologia] NVARCHAR(100) NULL,
    [NivelSeniority] NVARCHAR(50) NULL,
    [Capacidad] NVARCHAR(50) NULL,
    [Empresa] NVARCHAR(150) NULL,
    [Direccion] NVARCHAR(250) NULL,
    [Barrio] NVARCHAR(100) NULL,
    [ContactoEmergenciaNombre] NVARCHAR(150) NULL,
    [ContactoEmergenciaTelefono] NVARCHAR(20) NULL,
    [Estado] NVARCHAR(20) NOT NULL CONSTRAINT [DF_Consultores_Estado] DEFAULT ('Activo'),
    [FechaIngreso] DATE NULL,
    [FechaNacimiento] DATE NULL,
    [Habilitado] BIT NOT NULL CONSTRAINT [DF_Consultores_Habilitado] DEFAULT ((1)),
    [FotoUrl] NVARCHAR(500) NULL,
    [Observaciones] NVARCHAR(MAX) NULL,
    [MotivoDeshabilitacion] NVARCHAR(500) NULL,
    [FechaDeshabilitacion] DATETIME2 NULL,
    [Activo] BIT NOT NULL CONSTRAINT [DF_Consultores_Activo] DEFAULT ((1)),
    [FechaCreacion] DATETIME2 NOT NULL CONSTRAINT [DF_Consultores_FechaCreacion] DEFAULT (GETUTCDATE()),
    [FechaModificacion] DATETIME2 NULL,
    [CreadoPor] NVARCHAR(450) NULL,
    [ModificadoPor] NVARCHAR(450) NULL,
    CONSTRAINT [PK_Consultores] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_Consultores_Email]
    ON [dbo].[Consultores] ([Email] ASC)
    WHERE [Activo] = 1;
GO

CREATE NONCLUSTERED INDEX [IX_Consultores_Habilitado]
    ON [dbo].[Consultores] ([Habilitado] ASC, [Activo] ASC);
GO
