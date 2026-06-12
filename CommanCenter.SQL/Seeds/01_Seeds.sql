-- ============================================================
-- CommanCenter.SQL / Seeds / 01_Roles y Usuarios
-- Seed inicial: Roles y usuarios de prueba
-- ⚠️ SOLO ejecutar en entorno nuevo. NO en producción con datos.
-- ============================================================

SET QUOTED_IDENTIFIER ON;
GO

-- Crear roles si no existen
IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetRoles] WHERE [Name] = 'Admin')
	INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
	VALUES (NEWID(), 'Admin', 'ADMIN', NEWID());

IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetRoles] WHERE [Name] = 'Supervisor')
	INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
	VALUES (NEWID(), 'Supervisor', 'SUPERVISOR', NEWID());

IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetRoles] WHERE [Name] = 'Senior')
	INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
	VALUES (NEWID(), 'Senior', 'SENIOR', NEWID());

PRINT '✅ Roles insertados (Admin, Supervisor, Senior).';
GO

-- Crear usuarios de prueba con contraseñas en texto plano
DECLARE @AdminId NVARCHAR(MAX) = NEWID();
DECLARE @AlexanderId NVARCHAR(MAX) = NEWID();
DECLARE @SergioId NVARCHAR(MAX) = NEWID();

-- Usuario: admin / Admin@123 / Admin
IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetUsers] WHERE [UserName] = 'admin')
BEGIN
	INSERT INTO [dbo].[AspNetUsers] 
		([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], 
		 [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], 
		 [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount])
	VALUES 
		(@AdminId, 'admin', 'ADMIN', 'admin@commandcenter.com', 'ADMIN@COMMANDCENTER.COM', 1, 
		 'Admin@123', NEWID(), NEWID(), NULL, 0, 0, NULL, 1, 0);
	
	INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId])
	SELECT @AdminId, [Id] FROM [dbo].[AspNetRoles] WHERE [Name] = 'Admin';
	
	PRINT '✅ Usuario creado: admin / Admin@123';
END

-- Usuario: alexander / Alexander@123 / Supervisor
IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetUsers] WHERE [UserName] = 'alexander')
BEGIN
	INSERT INTO [dbo].[AspNetUsers] 
		([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], 
		 [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], 
		 [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount])
	VALUES 
		(@AlexanderId, 'alexander', 'ALEXANDER', 'alexander@commandcenter.com', 'ALEXANDER@COMMANDCENTER.COM', 1, 
		 'Alexander@123', NEWID(), NEWID(), NULL, 0, 0, NULL, 1, 0);
	
	INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId])
	SELECT @AlexanderId, [Id] FROM [dbo].[AspNetRoles] WHERE [Name] = 'Supervisor';
	
	PRINT '✅ Usuario creado: alexander / Alexander@123';
END

-- Usuario: sergio / Sergio@123 / Supervisor
IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetUsers] WHERE [UserName] = 'sergio')
BEGIN
	INSERT INTO [dbo].[AspNetUsers] 
		([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], 
		 [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], 
		 [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount])
	VALUES 
		(@SergioId, 'sergio', 'SERGIO', 'sergio@commandcenter.com', 'SERGIO@COMMANDCENTER.COM', 1, 
		 'Sergio@123', NEWID(), NEWID(), NULL, 0, 0, NULL, 1, 0);
	
	INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId])
	SELECT @SergioId, [Id] FROM [dbo].[AspNetRoles] WHERE [Name] = 'Supervisor';
	
	PRINT '✅ Usuario creado: sergio / Sergio@123';
END

PRINT '✅ 3 usuarios creados (admin, alexander, sergio)';
GO

-- ============================================================
-- CommanCenter.SQL / Seeds / 02_CelulasBase.sql
-- Células iniciales del módulo DataTeam
-- (Solo falta asignar consultores a cada una.)
-- Idempotente: cada célula se inserta solo si no existe.
-- ============================================================

;WITH Base AS (
    SELECT Nombre FROM (VALUES
        ('Administrativo'),
        ('Aurora'),
        ('Bon Voyage'),
        ('Data Stargazers'),
        ('DEVSECOPS'),
        ('Dirección Desarrollo'),
        ('Enterprise Team'),
        ('Facturador'),
        ('Maya'),
        ('MindShift'),
        ('Nova'),
        ('Polaris Software Team'),
        ('Seguridad'),
        ('Sin asignación'),
        ('Transversal Calidad'),
        ('Wakanda')
    ) AS C(Nombre)
)
INSERT INTO [dbo].[Celulas] ([Nombre], [Color], [Activo], [FechaCreacion])
SELECT b.Nombre, '#28a745', 1, GETUTCDATE()
FROM Base b
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[Celulas] c WHERE c.[Nombre] = b.Nombre
);

PRINT '✅ Células base insertadas/verificadas (16).';
GO

