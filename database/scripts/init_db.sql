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

-- Verificar que la tabla se creó correctamente
SELECT 'Base de datos inicializada correctamente' AS Estado;