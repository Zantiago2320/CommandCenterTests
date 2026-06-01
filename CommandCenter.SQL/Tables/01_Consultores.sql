-- ============================================================
-- CommandCenter.SQL / Tables / 01_Consultores.sql
-- Módulo: DataTeam (reutilizable para RRHH, DevSecOps, etc.)
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Consultores')
BEGIN
	CREATE TABLE [dbo].[Consultores] (
		[Id]              INT IDENTITY(1,1)   NOT NULL,
		[Nombre]          NVARCHAR(100)        NOT NULL,
		[Apellido]        NVARCHAR(100)        NOT NULL,
		[Email]           NVARCHAR(200)        NOT NULL,
		[Telefono]        NVARCHAR(20)         NULL,
		[Cargo]           NVARCHAR(150)        NULL,
		[Tecnologia]      NVARCHAR(100)        NULL,
		[NivelSeniority]  NVARCHAR(50)         NULL,
		[FechaIngreso]    DATE                 NULL,
		[FechaNacimiento] DATE                 NULL,
		[Habilitado]      BIT                  NOT NULL DEFAULT 1,
		[FotoUrl]         NVARCHAR(500)        NULL,
		[Observaciones]   NVARCHAR(MAX)        NULL,
		[Activo]          BIT                  NOT NULL DEFAULT 1,
		[FechaCreacion]   DATETIME2            NOT NULL DEFAULT GETUTCDATE(),
		[FechaModificacion] DATETIME2          NULL,
		[CreadoPor]       NVARCHAR(450)        NULL,
		[ModificadoPor]   NVARCHAR(450)        NULL,
		CONSTRAINT [PK_Consultores] PRIMARY KEY CLUSTERED ([Id] ASC)
	);

	CREATE UNIQUE INDEX [UX_Consultores_Email]
		ON [dbo].[Consultores] ([Email])
		WHERE [Activo] = 1;

	CREATE INDEX [IX_Consultores_Habilitado]
		ON [dbo].[Consultores] ([Habilitado], [Activo]);

	PRINT '✅ Tabla Consultores creada.';
END
ELSE
	PRINT '⚠️ Tabla Consultores ya existe.';
GO
