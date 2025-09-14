-- Normalizar numeración existente (MySQL 8+)
--SE CORRE DESPUES DEL INSERT DE CONVENIOS
UPDATE CONVENIOS c
JOIN (
    SELECT id_convenio,
           ROW_NUMBER() OVER (
             PARTITION BY id_empresa
             ORDER BY COALESCE(fecha_inicio,'1900-01-01'), id_convenio
           ) AS rn
    FROM CONVENIOS
    WHERE id_empresa IS NOT NULL
) t ON t.id_convenio = c.id_convenio
SET c.nro_acuerdo_marco = t.rn;