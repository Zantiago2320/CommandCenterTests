CREATE VIEW [dbo].[VW_ConsultoresActivos]
AS
SELECT
    con.Id,
    con.Nombre,
    con.Apellido,
    con.Nombre + ' ' + con.Apellido AS NombreCompleto,
    con.Email,
    con.Cargo,
    con.Tecnologia,
    con.NivelSeniority,
    con.FechaIngreso,
    con.FechaNacimiento,
    c.Nombre AS CelulaNombre,
    c.Color AS CelulaColor
FROM [dbo].[Consultores] con
LEFT JOIN [dbo].[CelulaMiembros] cm ON cm.ConsultorId = con.Id
LEFT JOIN [dbo].[Celulas] c ON c.Id = cm.CelulaId AND c.Activo = 1
WHERE con.Activo = 1 AND con.Habilitado = 1;
