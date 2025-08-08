-- Script para insertar datos aleatorios de prueba en las tablas principales

-- Insertar empresas
INSERT INTO EMPRESAS (nombre, vigencia, fecha_inicio, fecha_fin, tipo_contrato, encargado, celular, correo_electronico, sudocu)
VALUES
('Empresa Alpha', 'vigente', '2025-01-01', '2026-01-01', 'temporal', 'Juan Encargado', '123456789', 'alpha@empresa.com', '2025-01-01'),
('Empresa Beta', 'no_vigente', '2024-01-01', '2024-12-31', 'indefinido', 'Maria Encargada', '987654321', 'beta@empresa.com', '2024-01-01');

-- Insertar convenios
INSERT INTO CONVENIOS (id_empresa, representante_empresa, nro_acuerdo_marco, domicilio_legal, expediente, doc_representante_empresa, representante_facultad, doc_representante_facultad, fecha_firma, fecha_caducidad)
VALUES
(1, 'Juan Encargado', 1001, 'Calle Falsa 123', 'EXP-001', '12345678', 'Dr. Facultad', '87654321', '2025-01-10', '2026-01-10'),
(2, 'Maria Encargada', 1002, 'Avenida Siempreviva 742', 'EXP-002', '23456789', 'Dra. Facultad', '98765432', '2024-02-15', '2024-12-15');

-- Insertar estudiantes
INSERT INTO ESTUDIANTES (apellido, nombre, documento, domicilio, libreta, carrera, area_trabajo, email)
VALUES
('Pérez', 'Juan', '12345678', 'Calle 1', 'L123', 'Ingeniería', 'Sistemas', 'juan.perez@correo.com'),
('García', 'María', '87654321', 'Calle 2', 'L456', 'Administración', 'Recursos Humanos', 'maria.garcia@correo.com');

-- Insertar pasantías
INSERT INTO PASANTIAS (id_estudiante, id_convenio, asignacion_mensual, obra_social, art, tutor_empresa, tutor_facultad, expediente, fecha_inicio, fecha_fin, tipo_acuerdo, observaciones)
VALUES
(1, 1, 50000.00, 'OSDE', 'ART1', 'Encargado Empresa', 'Tutor Facultad', 'EXP-PA-001', '2025-03-01', '2025-09-01', 'Pasantia', 'Observaciones de prueba'),
(2, 2, 60000.00, 'Swiss Medical', 'ART2', 'Encargada Empresa', 'Tutora Facultad', 'EXP-PA-002', '2024-04-01', '2024-10-01', 'PPS', 'Observaciones de prueba 2');

-- Insertar pagos
INSERT INTO PAGOS (id_pasantia, fecha_pago, fecha_vencimiento, monto, observaciones)
VALUES
(1, '2025-03-10', '2025-03-30', 10000.00, 'Primer pago'),
(1, '2025-04-10', '2025-04-30', 10000.00, 'Segundo pago'),
(2, '2024-04-15', '2024-04-30', 12000.00, 'Pago inicial PPS');

-- Insertar usuarios
INSERT INTO USUARIOS (nombre_usuario, contrasena_hash, rol, correo)
VALUES
('admin', 'hashadmin', 'admin', 'admin@correo.com'),
('empresa1', 'hash1', 'empresa', 'empresa1@correo.com'),
('estudiante1', 'hash2', 'estudiante', 'estudiante1@correo.com');
