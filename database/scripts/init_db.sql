-- =====================================================
-- Script de inicialización de la base de datos
-- =====================================================


SET sql_mode = 'NO_ENGINE_SUBSTITUTION';

-- Crear base de datos y usuarios solo si tienes permisos (ejecuta como root en Docker)
CREATE DATABASE IF NOT EXISTS pasantias_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER IF NOT EXISTS 'appuser'@'%' IDENTIFIED BY 'TuPasswordSeguro123!';
CREATE USER IF NOT EXISTS 'appuser'@'localhost' IDENTIFIED BY 'TuPasswordSeguro123!';
GRANT ALL PRIVILEGES ON pasantias_db.* TO 'appuser'@'%';
GRANT ALL PRIVILEGES ON pasantias_db.* TO 'appuser'@'localhost';
GRANT ALL PRIVILEGES ON pasantias_db.* TO 'root'@'%';
FLUSH PRIVILEGES;

-- Seleccionar la base de datos para el resto del script
USE pasantias_db;

-- Crear tablas según la nueva estructura

CREATE TABLE EMPRESAS (
    id_empresa INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(255),
    vigencia ENUM('vigente', 'no_vigente'),
    fecha_inicio DATE,
    fecha_fin DATE,
    tipo_contrato ENUM('temporal', 'indefinido', 'otro'),
    encargado VARCHAR(255),
    celular VARCHAR(50),
    correo_electronico VARCHAR(255),
    sudocu DATE,
    eliminado BOOLEAN DEFAULT FALSE,
    fecha_eliminacion DATETIME NULL
);

CREATE TABLE CONVENIOS (
    id_convenio INT PRIMARY KEY AUTO_INCREMENT,
    id_empresa INT,
    representante_empresa VARCHAR(255),
    nro_acuerdo_marco INT,
    domicilio_legal VARCHAR(255),
    expediente VARCHAR(255),
    doc_representante_empresa VARCHAR(255),
    representante_facultad VARCHAR(255),
    doc_representante_facultad VARCHAR(255),
    fecha_firma DATE,
    fecha_caducidad DATE,
    FOREIGN KEY (id_empresa) REFERENCES EMPRESAS(id_empresa)
);

CREATE TABLE ESTUDIANTES (
    id_estudiante INT PRIMARY KEY AUTO_INCREMENT,
    apellido VARCHAR(100),
    nombre VARCHAR(100),
    documento VARCHAR(50),
    domicilio VARCHAR(255),
    libreta VARCHAR(50),
    carrera VARCHAR(100),
    area_trabajo VARCHAR(100),
    email VARCHAR(200),
    eliminado BOOLEAN DEFAULT FALSE,
    fecha_eliminacion DATETIME NULL
);

CREATE TABLE PASANTIAS (
    id_pasantia INT PRIMARY KEY AUTO_INCREMENT,
    id_estudiante INT,
    id_convenio INT,
    asignacion_mensual DECIMAL(10,2),
    obra_social VARCHAR(100),
    art VARCHAR(100),
    tutor_empresa VARCHAR(100),
    tutor_facultad VARCHAR(100),
    expediente VARCHAR(100),
    fecha_inicio DATE,
    fecha_fin DATE,
    tipo_acuerdo ENUM('Pasantia', 'PPS', 'otro'),
    observaciones TEXT,
    FOREIGN KEY (id_estudiante) REFERENCES ESTUDIANTES(id_estudiante),
    FOREIGN KEY (id_convenio) REFERENCES CONVENIOS(id_convenio)
);

CREATE TABLE PAGOS (
    id_pago INT PRIMARY KEY AUTO_INCREMENT,
    id_pasantia INT,
    fecha_pago DATE,
    fecha_vencimiento DATE,
    monto DECIMAL(10,2),
    observaciones TEXT,
    FOREIGN KEY (id_pasantia) REFERENCES PASANTIAS(id_pasantia)
);

CREATE TABLE USUARIOS (
    id_usuario INT PRIMARY KEY AUTO_INCREMENT,
    nombre_usuario VARCHAR(100) UNIQUE,
    contrasena_hash VARCHAR(255),
    rol ENUM('admin', 'empresa', 'estudiante'),
    correo VARCHAR(255) UNIQUE,
    eliminado BOOLEAN DEFAULT FALSE,
    fecha_eliminacion DATETIME NULL
);

CREATE TABLE AUDITORIA (
    id_auditoria INT PRIMARY KEY AUTO_INCREMENT,
    id_usuario INT,
    tabla_afectada VARCHAR(100),
    tipo_operacion ENUM('INSERT', 'UPDATE', 'DELETE', 'LOGIN', 'LOGOUT'),
    datos_anteriores TEXT,
    datos_nuevos TEXT,
    fecha_operacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    funcion_llamada VARCHAR(100),
    FOREIGN KEY (id_usuario) REFERENCES USUARIOS(id_usuario)
);

-- Crear índices
CREATE INDEX idx_estudiante ON PASANTIAS(id_estudiante);
CREATE INDEX idx_convenio ON PASANTIAS(id_convenio);
CREATE INDEX idx_pasantia ON PAGOS(id_pasantia);
CREATE INDEX idx_usuario ON USUARIOS(nombre_usuario);

-- Insertar datos de ejemplo (comentado para que no falle si no hay estructura)
INSERT INTO Estudiantes (Nombre, Email, Carrera) VALUES
('Juan Pérez', 'juan.perez@email.com', 'Ingeniería Informática'),
('María García', 'maria.garcia@email.com', 'Administración de Empresas'),
('Carlos López', 'carlos.lopez@email.com', 'Contabilidad')
ON DUPLICATE KEY UPDATE Nombre = VALUES(Nombre);

-- Script para insertar datos aleatorios de prueba en las tablas principales
SELECT * FROM PASANTIAS;
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
-- Insertar pagos
INSERT INTO PAGOS (id_pasantia, fecha_pago, fecha_vencimiento, monto, observaciones)
VALUES
(2, '2025-03-10', '2025-03-30', 10000.00, 'Primer pago'),
(2, '2025-04-10', '2025-04-30', 10000.00, 'Segundo pago'),
(3, '2024-04-15', '2024-04-30', 12000.00, 'Pago inicial PPS');

-- Insertar usuarios
INSERT INTO USUARIOS (nombre_usuario, contrasena_hash, rol, correo)
VALUES
('admin', 'hashadmin', 'admin', 'admin@correo.com'),
('empresa1', 'hash1', 'empresa', 'empresa1@correo.com'),
('estudiante1', 'hash2', 'estudiante', 'estudiante1@correo.com');

-- Verificar que la tabla se creó correctamente
SELECT 'Base de datos inicializada correctamente' AS Estado;