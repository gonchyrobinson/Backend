SET sql_mode = 'NO_ENGINE_SUBSTITUTION';

-- Crear base de datos y usuarios solo si tienes permisos (ejecuta como root en Docker)
CREATE DATABASE IF NOT EXISTS pasantias_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER IF NOT EXISTS 'appuser'@'%' IDENTIFIED BY 'TuPasswordSeguro123!';
CREATE USER IF NOT EXISTS 'appuser'@'localhost' IDENTIFIED BY 'TuPasswordSeguro123!';
GRANT ALL PRIVILEGES ON pasantias_db.* TO 'appuser'@'%';
GRANT ALL PRIVILEGES ON pasantias_db.* TO 'appuser'@'localhost';
GRANT ALL PRIVILEGES ON pasantias_db.* TO 'myadmin'@'%';
FLUSH PRIVILEGES;

-- Seleccionar la base de datos para el resto del script
USE pasantias_db;

-- Crear tablas según la nueva estructura

CREATE TABLE EMPRESAS (
    id_empresa INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(255),
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
    carrera ENUM(
        'AGRIMENSURA',
        'INGENIERÍA AZUCARERA',
        'INGENIERÍA BIOMÉDICA',
        'INGENIERÍA CIVIL',
        'INGENIERÍA EN COMPUTACIÓN',
        'INGENIERÍA EN INFORMÁTICA',
        'INGENIERÍA ELÉCTRICA',
        'INGENIERÍA ELECTRÓNICA',
        'INGENIERÍA GEODÉSICA Y GEOFÍSICA',
        'INGENIERÍA INDUSTRIAL',
        'INGENIERÍA MECÁNICA',
        'INGENIERÍA QUÍMICA',
        'LICENCIATURA EN FÍSICA',
        'LICENCIATURA EN MATEMÁTICA',
        'LICENCIATURA EN INFORMÁTICA',
        'DISEÑO DE ILUMINACIÓN',
        'PROGRAMADOR UNIVERSITARIO',
        'TECNICATURA UNIVERSITARIA EN TECNOLOGÍA',
        'AZUCARERA E INDUSTRIAS DERIVADAS',
        'TECNICATURA UNIVERSITARIA EN FÍSICA',
        'TECNICATURA UNIVERSITARIA EN FÍSICA AMBIENTAL'
    ),
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
    dni_tutor_empresa VARCHAR(100),
    tutor_facultad VARCHAR(100),
    dni_tutor_facultad VARCHAR(100),
    fecha_inicio DATE,
    fecha_fin DATE,
    tipo_acuerdo ENUM('Pasantia', 'PPS', 'otro'),
    observaciones TEXT,
    frecuencia_pago ENUM('Mensual', 'Trimestral', 'Semestral', 'Anual', 'Otro') NULL,
    tramite_sudocu VARCHAR(255),
    horas_semanales INT,
    area_trabajo VARCHAR(255),
    FOREIGN KEY (id_estudiante) REFERENCES ESTUDIANTES(id_estudiante),
    FOREIGN KEY (id_convenio) REFERENCES CONVENIOS(id_convenio)
);

CREATE TABLE PAGOS (
    id_pago INT PRIMARY KEY AUTO_INCREMENT,
    id_pasantia INT,
    pagado BOOLEAN DEFAULT FALSE,
    fecha_pago DATE DEFAULT NULL,
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
DELIMITER //

-- Trigger para INSERT en EMPRESAS
CREATE TRIGGER trg_empresas_insert AFTER INSERT ON EMPRESAS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'EMPRESAS', 'INSERT', NULL, CONCAT('id_empresa=', NEW.id_empresa, ', nombre=', NEW.nombre), 'TRIGGER');
END;//

-- Trigger para UPDATE en EMPRESAS
CREATE TRIGGER trg_empresas_update AFTER UPDATE ON EMPRESAS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'EMPRESAS', 'UPDATE', CONCAT('id_empresa=', OLD.id_empresa, ', nombre=', OLD.nombre), CONCAT('id_empresa=', NEW.id_empresa, ', nombre=', NEW.nombre), 'TRIGGER');
END;//

-- Trigger para DELETE en EMPRESAS
CREATE TRIGGER trg_empresas_delete AFTER DELETE ON EMPRESAS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'EMPRESAS', 'DELETE', CONCAT('id_empresa=', OLD.id_empresa, ', nombre=', OLD.nombre), NULL, 'TRIGGER');
END;//

-- Repetir para otras tablas críticas (CONVENIOS, ESTUDIANTES, PASANTIAS, PAGOS, USUARIOS)

-- Trigger para INSERT en ESTUDIANTES
CREATE TRIGGER trg_estudiantes_insert AFTER INSERT ON ESTUDIANTES
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'ESTUDIANTES', 'INSERT', NULL, CONCAT('id_estudiante=', NEW.id_estudiante, ', nombre=', NEW.nombre), 'TRIGGER');
END;//

-- Trigger para UPDATE en ESTUDIANTES
CREATE TRIGGER trg_estudiantes_update AFTER UPDATE ON ESTUDIANTES
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'ESTUDIANTES', 'UPDATE', CONCAT('id_estudiante=', OLD.id_estudiante, ', nombre=', OLD.nombre), CONCAT('id_estudiante=', NEW.id_estudiante, ', nombre=', NEW.nombre), 'TRIGGER');
END;//

-- Trigger para DELETE en ESTUDIANTES
CREATE TRIGGER trg_estudiantes_delete AFTER DELETE ON ESTUDIANTES
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'ESTUDIANTES', 'DELETE', CONCAT('id_estudiante=', OLD.id_estudiante, ', nombre=', OLD.nombre), NULL, 'TRIGGER');
END;//

-- Trigger para INSERT en PASANTIAS
CREATE TRIGGER trg_pasantias_insert AFTER INSERT ON PASANTIAS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'PASANTIAS', 'INSERT', NULL, CONCAT('id_pasantia=', NEW.id_pasantia, ', id_estudiante=', NEW.id_estudiante), 'TRIGGER');
END;//

-- Trigger para UPDATE en PASANTIAS
CREATE TRIGGER trg_pasantias_update AFTER UPDATE ON PASANTIAS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'PASANTIAS', 'UPDATE', CONCAT('id_pasantia=', OLD.id_pasantia, ', id_estudiante=', OLD.id_estudiante), CONCAT('id_pasantia=', NEW.id_pasantia, ', id_estudiante=', NEW.id_estudiante), 'TRIGGER');
END;//

-- Trigger para DELETE en PASANTIAS
CREATE TRIGGER trg_pasantias_delete AFTER DELETE ON PASANTIAS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'PASANTIAS', 'DELETE', CONCAT('id_pasantia=', OLD.id_pasantia, ', id_estudiante=', OLD.id_estudiante), NULL, 'TRIGGER');
END;//

-- Trigger para INSERT en CONVENIOS
CREATE TRIGGER trg_convenios_insert AFTER INSERT ON CONVENIOS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'CONVENIOS', 'INSERT', NULL, CONCAT('id_convenio=', NEW.id_convenio, ', id_empresa=', NEW.id_empresa), 'TRIGGER');
END;//

-- Trigger para UPDATE en CONVENIOS
CREATE TRIGGER trg_convenios_update AFTER UPDATE ON CONVENIOS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'CONVENIOS', 'UPDATE', CONCAT('id_convenio=', OLD.id_convenio, ', id_empresa=', OLD.id_empresa), CONCAT('id_convenio=', NEW.id_convenio, ', id_empresa=', NEW.id_empresa), 'TRIGGER');
END;//

-- Trigger para DELETE en CONVENIOS
CREATE TRIGGER trg_convenios_delete AFTER DELETE ON CONVENIOS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'CONVENIOS', 'DELETE', CONCAT('id_convenio=', OLD.id_convenio, ', id_empresa=', OLD.id_empresa), NULL, 'TRIGGER');
END;//

-- Trigger para INSERT en PAGOS
CREATE TRIGGER trg_pagos_insert AFTER INSERT ON PAGOS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'PAGOS', 'INSERT', NULL, CONCAT('id_pago=', NEW.id_pago, ', id_pasantia=', NEW.id_pasantia), 'TRIGGER');
END;//

-- Trigger para UPDATE en PAGOS
CREATE TRIGGER trg_pagos_update AFTER UPDATE ON PAGOS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'PAGOS', 'UPDATE', CONCAT('id_pago=', OLD.id_pago, ', id_pasantia=', OLD.id_pasantia), CONCAT('id_pago=', NEW.id_pago, ', id_pasantia=', NEW.id_pasantia), 'TRIGGER');
END;//

CREATE TRIGGER trg_pagos_delete AFTER DELETE ON PAGOS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NULL, 'PAGOS', 'DELETE', CONCAT('id_pago=', OLD.id_pago, ', id_pasantia=', OLD.id_pasantia), NULL, 'TRIGGER');
END;//

-- Trigger para INSERT en USUARIOS
CREATE TRIGGER trg_usuarios_insert AFTER INSERT ON USUARIOS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NEW.id_usuario, 'USUARIOS', 'INSERT', NULL, CONCAT('id_usuario=', NEW.id_usuario, ', nombre_usuario=', NEW.nombre_usuario), 'TRIGGER');
END;//

-- Trigger para UPDATE en USUARIOS
CREATE TRIGGER trg_usuarios_update AFTER UPDATE ON USUARIOS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (NEW.id_usuario, 'USUARIOS', 'UPDATE', CONCAT('id_usuario=', OLD.id_usuario, ', nombre_usuario=', OLD.nombre_usuario), CONCAT('id_usuario=', NEW.id_usuario, ', nombre_usuario=', NEW.nombre_usuario), 'TRIGGER');
END;//

-- Trigger para DELETE en USUARIOS
CREATE TRIGGER trg_usuarios_delete AFTER DELETE ON USUARIOS
FOR EACH ROW
BEGIN
    INSERT INTO AUDITORIA (id_usuario, tabla_afectada, tipo_operacion, datos_anteriores, datos_nuevos, funcion_llamada)
    VALUES (OLD.id_usuario, 'USUARIOS', 'DELETE', CONCAT('id_usuario=', OLD.id_usuario, ', nombre_usuario=', OLD.nombre_usuario), NULL, 'TRIGGER');
END;//
DELIMITER ;