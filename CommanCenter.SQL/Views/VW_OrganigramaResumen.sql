CREATE VIEW [dbo].[VW_OrganigramaResumen]
AS
SELECT
    c.Id AS CelulaId,
    c.Nombre AS CelulaNombre,
    c.Color AS Color,
    c.Activo AS Activa,
    COUNT(DISTINCT cm.ConsultorId) AS TotalMiembros,
    COUNT(DISTINCT cl.ConsultorId) AS TotalLideres,
    STRING_AGG(con_l.Nombre + ' ' + con_l.Apellido, ', ') AS Lideres
FROM [dbo].[Celulas] c
LEFT JOIN [dbo].[CelulaMiembros] cm ON cm.CelulaId = c.Id
LEFT JOIN [dbo].[CelulaLideres] cl ON cl.CelulaId = c.Id
LEFT JOIN [dbo].[Consultores] con_l ON con_l.Id = cl.ConsultorId AND con_l.Activo = 1
WHERE c.Activo = 1
GROUP BY c.Id, c.Nombre, c.Color, c.Activo;
