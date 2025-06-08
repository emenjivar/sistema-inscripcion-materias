CREATE DATABASE inscripcion_universitaria;
GO

USE inscripcion_universitaria;
GO

CREATE TABLE pensum(
    id INT PRIMARY KEY IDENTITY(1,1),
    carrera VARCHAR(256) NOT NULL
);

CREATE TABLE materia(
    id INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(30) UNIQUE NOT NULL,
    nombre VARCHAR(120) NOT NULL,
    unidades_valorativas INT NOT NULL,
    descripcion VARCHAR(120)
);

CREATE TABLE usuario(
	id INT PRIMARY KEY IDENTITY(1,1),
	username VARCHAR(30) NOT NULL,
	email VARCHAR(64) NOT NULL,
	nombres VARCHAR(60) NOT NULL,
	apellidos VARCHAR(60) NOT NULL,
	password VARCHAR(64) NOT NULL,
	rol VARCHAR(15) NOT NULL CHECK (rol IN ('admin', 'alumno'))
);

CREATE TABLE alumno(
    id INT PRIMARY KEY IDENTITY(1,1),
    carnet VARCHAR(6) UNIQUE NOT NULL,
	nombres VARCHAR(60) NOT NULL,
	apellidos VARCHAR(60) NOT NULL,
    id_pensum INT,
	id_usuario INT NOT NULL,
    FOREIGN KEY(id_pensum) REFERENCES pensum(id),
	FOREIGN KEY(id_usuario) REFERENCES usuario(id)
);

-- Detalle del pensum, materias organizadas por ciclos curriculares
CREATE TABLE pensum_materias(
    id INT PRIMARY KEY IDENTITY(1,1),
    id_pensum INT NOT NULL,
    id_materia INT NOT NULL,
    id_materia_prerequisito INT,
    ciclo_curricular INT NOT NULL, -- ciclo secuencial dentro del plan de estudios
    FOREIGN KEY(id_pensum) REFERENCES pensum(id),
    FOREIGN KEY(id_materia) REFERENCES materia(id),
    FOREIGN KEY(id_materia_prerequisito) REFERENCES materia(id)
);

-- Apertura de ciclo academico, indica que las materias estan listas para inscribir 
CREATE TABLE inscripcion(
    id INT PRIMARY KEY IDENTITY(1,1),
    ciclo_academico INT NOT NULL, -- division temporal del año academico
    anio INT NOT NULL, -- año en el que apertura la inscripcion de ciclo.
    id_pensum INT NOT NULL,
    -- consulta: los alumnos pueden consultar horarios, pero no inscribir
    -- inscripcion: el ciclo ha sido aperturado, los alumnos pueden inscribir materias
    -- cerrado: no se permiten mas inscripciones
    estado VARCHAR(20) NOT NULL CHECK (estado IN ('consulta', 'inscripcion', 'cerrado')),
    FOREIGN KEY(id_pensum) REFERENCES pensum(id)
);

-- Variantes de grupos dentro de la misma carrera (DS01, DSN01, DS02, ...)
CREATE TABLE grupo_clase(
    id INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20) NOT NULL
)

-- Horarios disponibles por ciclo y materia
CREATE TABLE bloque_horario_material(
    id INT PRIMARY KEY IDENTITY(1,1),
    id_inscripcion INT NOT NULL,
    id_materia INT NOT NULL,
    id_grupo INT NOT NULL,
    dia_semana VARCHAR(10) NOT NULL CHECK (dia_semana in ('Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado', 'Domingo')),
    hora_inicio TIME NOT NULL,
    hora_fin TIME NOT NULL,
    FOREIGN KEY(id_inscripcion) REFERENCES inscripcion(id),
    FOREIGN KEY(id_materia) REFERENCES materia(id),
    FOREIGN KEY(id_grupo) REFERENCES grupo_clase(id)
);

-- Materias inscritas por alumno
CREATE TABLE inscripcion_alumno(
    id INT PRIMARY KEY IDENTITY(1,1),
    id_alumno INT NOT NULL,
    id_bloque_horario_materia INT NOT NULL,
    FOREIGN KEY(id_alumno) REFERENCES alumno(id),
    FOREIGN KEY(id_bloque_horario_materia) REFERENCES bloque_horario_material(id)
);

-- Registra el resultado (aprobado/reprobado) de cada alumno en cada materia
-- al finalizar un ciclo academico
CREATE TABLE resultado_ciclo_academico(
    id INT PRIMARY KEY IDENTITY(1,1),
    id_alumno INT NOT NULL,
    id_materia INT NOT NULL,
    aprobado BIT NOT NULL, -- 1 para aprobado, 0 para reprobado
    FOREIGN KEY(id_alumno) REFERENCES alumno(id),
    FOREIGN KEY(id_materia) REFERENCES materia(id)
);

ALTER TABLE pensum ADD tipo_carrera VARCHAR(200) NOT NULL;
ALTER TABLE pensum ADD cantidad_ciclos INT NOT NULL;

ALTER TABLE pensum
ADD estado BIT default 1;
GO

SET IDENTITY_INSERT pensum ON
insert into pensum(id, carrera, tipo_carrera, cantidad_ciclos, estado) values (1, 'Ingenieria en sistemas', 'Nocturna', 10, 1);
SET IDENTITY_INSERT pensum OFF
GO


	
-- Materias disponibles para poder armar los pensum
SET IDENTITY_INSERT materia ON
-- ciclo I
insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (1, 'COIDS0104', 'Desarrollo del pensamiento numerico y algebraico', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (2, 'COIDS0204', 'Apropiacion de vocabulario de ingles', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (3, 'COIDS0304', 'Desarrollo de la lectura y composicion', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (4, 'COIDS0404', 'Prevencion de accidentes y enfermedades ocupacionales', 3, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (5, 'COIDS0504', 'Desarrollo de logica de programacion', 4, '');

-- Ciclo II
insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (6, 'COIDS0604', 'Desarrollo del pensamiento matematico', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (7, 'COIDS0704', 'Construccion de frases en ingles sobre cuestiones laborales', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (8, 'COIDS0804', 'Diseno y ejecucion de plan de negocios', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (9, 'COIDS0904', 'Desarrollo del pensamiento estadistico y probabilistico', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (10, 'COIDS1004', 'Programacion estructurada', 4, '');

-- Ciclo III
insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (11, 'COIDS1104', 'Desarrollo del pensamiento matematico avanzado', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (12, 'COIDS1204', 'Descripcion de situaciones actuales en ingles', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (13, 'COIDS1304', 'Manejo de estructuras de datos', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (14, 'COIDS1404', 'Descripcion de fenomenos fisicos', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (15, 'COIDS1504', 'Programacion orientada a objetos', 4, '');

-- Ciclo IV
insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (16, 'COIDS1604', 'Aplicacion de metodos numericos en las ciencias computacionales', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (17, 'COIDS1704', 'Conversacion a nigel intermedio en ingles', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (18, 'COIDS1804', 'Diseno de bases de datos', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (19, 'COIDS1904', 'Aplicacion de metodos y tecnicas de investigacion', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (20, 'COIDS2004', 'Programacion orientada a eventos', 4, '');

-- Ciclo V
insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (21, 'COIDS2104', 'Configuracion de redes informaticas', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (22, 'COIDS2204', 'Aplicacion de tecnicas contables', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (23, 'COIDS2304', 'Programacion de bases de datos', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (24, 'COIDS2404', 'Analisis y diseno de sistemas', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (25, 'COIDS2504', 'Diseno de paginas web', 4, '');

-- Ciclo VI
insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (26, 'COIDS2604', 'Administracion de sistemas operativos', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (27, 'COIDS2704', 'Desarrollo de funciones avanzadas de base de datos', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (28, 'COIDS2804', 'Aplicacion de tecnicas de ingenieria de software', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (29, 'COIDS2904', 'Diseno web adaptable', 4, '');

-- Ciclo VII
insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (30, 'COIDS3004', 'Diseno de arquitecturas de sistemas', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (31, 'COIDS3104', 'Desarrollo de paginas web activas', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (32, 'COIDS3204', 'Gestion de recursos humanos', 3, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (33, 'COIDS3304', 'Desarrollo de aplicaciones moviles basicas', 4, '');

-- Ciclo VIII
insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (34, 'COIDS3404', 'Gestion de proyectos informaticos', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (35, 'COIDS3504', 'Desarrollo de aplicaciones multiplataforma', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (36, 'COIDS3604', 'Desarrollo WEB usando software libre', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (37, 'COIDS3704', 'Desarrollo de aplicaciones moviles avanzadas', 4, '');

-- Ciclo IX
insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (38, 'COIDS3804', 'Gestion de seguridad en sistemas informaticas', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (39, 'COIDS3904', 'Aplicacion de frameworks empresariales', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (40, 'COIDS4004', 'Transacciones comercciales por medios electronicos', 3, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (41, 'COIDS4104', 'Aplicaciones de metodologias agiles de desarrollo de software', 4, '');

-- Ciclo IX
insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (42, 'COIDS4204', 'Gestion de calidad de software', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (43, 'COIDS4304', 'Manejo de la legislacion aplicada a la informatica', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (44, 'COIDS4404', 'Gestion de servidores web', 4, '');

insert into materia(id, codigo, nombre, unidades_valorativas, descripcion)
values (45, 'COIDS4504', 'Direccion de comportamiento humano en el ambiente laboral', 3, '');
SET IDENTITY_INSERT materia OFF

--- Pensum
-- ciclo I
insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 1, null, 1);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 2, null, 1);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 3, null, 1);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 4, null, 1);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 5, null, 1);

-- ciclo II
insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 6, 1, 2);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 7, 2, 2);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 8, null, 2);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 9, 1, 2);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 10, 5, 2);

-- ciclo III
insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 11, 6, 3);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 12, 7, 3);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 13, 10, 3);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 14, 1, 3);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 15, 10, 3);

-- ciclo IV
insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 16, 11, 4);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 17, 12, 4);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 18, null, 4);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 19, null, 4);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 20, 15, 4);

-- ciclo V
insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 21, null, 5);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 22, 1, 5);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 23, 18, 5);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 24, null, 5);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 25, null, 5);

-- ciclo VI
insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 26, null, 6);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 27, 23, 6);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 28, 24, 6);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 29, 25, 6);

-- ciclo VII
insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 30, null, 7);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 31, 25, 7);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 32, null, 7);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 33, 20, 7);

-- ciclo VIII
insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 34, null, 8);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 35, 20, 8);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 36, 29, 8);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 37, 33, 8);

-- ciclo IX
insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 38, 26, 9);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 39, 35, 9);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 40, 36, 9);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 41, null, 9);

-- ciclo X
insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 42, null, 10);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 43, null, 10);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 44, null, 10);

insert into pensum_materias(id_pensum, id_materia, id_materia_prerequisito, ciclo_curricular)
values (1, 45, null, 10);


--INSERT PARA INSCRIPCION
INSERT INTO inscripcion (ciclo_academico, anio, id_pensum, estado) VALUES ('1', '2025', '1', 'Cerrado');
INSERT INTO inscripcion (ciclo_academico, anio, id_pensum, estado) VALUES ('2', '2025', '1', 'Inscripcion');

--INSERT DE GRUPOS
INSERT INTO grupo_clase(codigo) VALUES('DSN09')
INSERT INTO grupo_clase(codigo) VALUES('DSN411')
INSERT INTO grupo_clase(codigo) VALUES('DSN321-A')
INSERT INTO grupo_clase(codigo) VALUES('DSN421')
INSERT INTO grupo_clase(codigo) VALUES('DSN521-A')


--INSERT PARA BLOQUE HORARIO MATERIA
INSERT INTO bloque_horario_material (id_inscripcion, id_materia, id_grupo, dia_semana, hora_inicio, hora_fin) VALUES ('1', '1', '3', 'Lunes', '17:25:00.0000000', '19:26:00.0000000');
INSERT INTO bloque_horario_material (id_inscripcion, id_materia, id_grupo, dia_semana, hora_inicio, hora_fin) VALUES ('2', '1', '3', 'Lunes', '09:06:00.0000000', '10:06:00.0000000');
INSERT INTO bloque_horario_material (id_inscripcion, id_materia, id_grupo, dia_semana, hora_inicio, hora_fin) VALUES ('2', '6', '3', 'Lunes', '10:06:00.0000000', '11:07:00.0000000');
INSERT INTO bloque_horario_material (id_inscripcion, id_materia, id_grupo, dia_semana, hora_inicio, hora_fin) VALUES ('2', '5', '3', 'Jueves', '10:07:00.0000000', '11:07:00.0000000');
INSERT INTO bloque_horario_material (id_inscripcion, id_materia, id_grupo, dia_semana, hora_inicio, hora_fin) VALUES ('2', '1', '4', 'Martes', '17:30:00.0000000', '20:30:00.0000000');
INSERT INTO bloque_horario_material (id_inscripcion, id_materia, id_grupo, dia_semana, hora_inicio, hora_fin) VALUES ('2', '3', '3', 'Lunes', '16:23:00.0000000', '18:23:00.0000000');
INSERT INTO bloque_horario_material (id_inscripcion, id_materia, id_grupo, dia_semana, hora_inicio, hora_fin) VALUES ('2', '3', '4', 'Jueves', '16:25:00.0000000', '18:25:00.0000000');




-- usuarios
insert into usuario(username, email, nombres, apellidos, password, rol) values ('admin', 'admin@itca.edu.sv', 'Administrador','', 'itca123', 'admin');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('81339', 'silvio.peres@itca.edu.sv', 'Silvio', 'Peres', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('268464', 'winslow.devo@itca.edu.sv', 'Winslow', 'Devo', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos,  password, rol) values ('476868', 'jaquenette.ubsdale@itca.edu.sv', 'Jaquenette', 'Ubsdale', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('104336', 'velvet.ramiro@itca.edu.sv', 'Velvet', 'Ramiro', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('779833', 'brittany.semechik@itca.edu.sv', 'Brittany', 'Semechik', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('512250', 'cassey.dolby@itca.edu.sv', 'Cassey', 'Dolby', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('861674', 'camile.schouthede@itca.edu.sv', 'Camile', 'Schouthede', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('188977', 'celie.brandle@itca.edu.sv', 'Celie', 'Brandle', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('122104', 'bjorn.zapata@itca.edu.sv', 'Bjorn', 'Zapata', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('112319', 'meade.byatt@itca.edu.sv', 'Meade', 'Byatt', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('25275', 'enrique.aumerle@itca.edu.sv', 'Enrique', 'Aumerle', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('277198', 'delly.martinez@itca.edu.sv', 'Delly', 'Martinez', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('807518', 'beatrisa.cowitz@itca.edu.sv', 'Beatrisa', 'Cowitz', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('635861', 'erasmus.champkin@itca.edu.sv', 'Erasmus', 'Champkin', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('725362', 'zolly.local@itca.edu.sv', 'Zolly', 'Local', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('806724', 'crystal.pellingar@itca.edu.sv', 'Crystal', 'Pellingar', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('817472', 'carola.bunner@itca.edu.sv', 'Carola', 'Bunner', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('362474', 'roddie.mouget@itca.edu.sv', 'Roddie', 'Mouget', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos,  password, rol) values ('418114', 'nealon.wonfor@itca.edu.sv', 'Nealon', 'Wonfor', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('689308', 'agna.seaward@itca.edu.sv', 'Agna', 'Seaward', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('41769', 'scarlett.deinert@itca.edu.sv', 'Scarlett', 'Deinert', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('732458', 'ashlin.arrigucci@itca.edu.sv', 'Ashlin', 'Arrigucci', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('66082', 'toinette.attrie@itca.edu.sv', 'Toinette', 'Attrie', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('816904', 'roddie.tomaszynski@itca.edu.sv', 'Roddie', 'Tomaszynski', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('785917', 'adamo.joinsey@itca.edu.sv', 'Adamo', 'Joinsey', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('701056', 'alexa.maith@itca.edu.sv', 'Alexa', 'Maith', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('655130', 'rachael.brandenberg@itca.edu.sv', 'Rachael', 'Brandenberg', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('486747', 'ginger.lamplough@itca.edu.sv', 'Ginger', 'Lamplough', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('96048', 'rodi.kuban@itca.edu.sv', 'Rodi', 'Kuban', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('676821', 'farleigh.middle@itca.edu.sv', 'Farleigh', 'Middle', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('411388', 'boothe.pothecary@itca.edu.sv', 'Boothe', 'Pothecary', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('867381', 'marlane.klosges@itca.edu.sv', 'Marlane', 'Klosges', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('537605', 'kelsi.drinkeld@itca.edu.sv', 'Kelsi', 'Drinkeld', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos,  password, rol) values ('780368', 'paulette.warlaw@itca.edu.sv', 'Paulette', 'Warlaw', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('693004', 'caz.spaughton@itca.edu.sv', 'Caz', 'Spaughton', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('487259', 'austin.sanday@itca.edu.sv', 'Austin', 'Sanday', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('948442', 'romonda.druitt@itca.edu.sv', 'Romonda', 'Druitt', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('495345', 'gordy.cornelius@itca.edu.sv', 'Gordy', 'Cornelius', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('289414', 'findley.randleson@itca.edu.sv', 'Findley', 'Randleson', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('934215', 'alika.crayk@itca.edu.sv', 'Alika', 'Crayk', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('480379', 'kienan.vanyukhin@itca.edu.sv', 'Kienan', 'Vanyukhin', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('94735', 'chelsy.hercules@itca.edu.sv', 'Chelsy', 'Hercules', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('677876', 'mathilda.mcmillian@itca.edu.sv', 'Mathilda', 'McMillian', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('403219', 'jean.trevenu@itca.edu.sv', 'Jean', 'Trevenu', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('12636', 'etheline.dubarry@itca.edu.sv', 'Ethelin', ' Dubarry', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('642193', 'franni.amps@itca.edu.sv', 'Franni', 'Amps', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('935889', 'odell.yerrell@itca.edu.sv', 'Odell', 'Yerrell', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('851243', 'briney.mcmaster@itca.edu.sv', 'Briney', 'McMaster', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('209288', 'elbert.hatfull@itca.edu.sv', 'Elbert', 'Hatfull', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('143963', 'luella.phelip@itca.edu.sv', 'Luella', 'Phelip', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('401667', 'sayers.toten@itca.edu.sv', 'Sayers', 'Toten', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos, password, rol) values ('280691', 'cal.grimme@itca.edu.sv', 'Cal', 'Grimme', 'itca123', 'alumno');
insert into usuario(username, email, nombres, apellidos,  password, rol) values ('971008', 'marlena.scardifield@itca.edu.sv', 'Marlena', 'Scardifield', 'itca123', 'alumno');

-- alumnos
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('81339', 'Silvio', 'Peres', 1, (select top 1 id from usuario where username='81339'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('268464', 'Winslow', 'Devo', 1, (select top 1 id from usuario where username='268464'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('476868', 'Jaquenette', 'Ubsdale', 1, (select top 1 id from usuario where username='476868'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('104336', 'Velvet', 'Ramiro', 1, (select top 1 id from usuario where username='104336'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('779833', 'Brittany', 'Semechik', 1, (select top 1 id from usuario where username='779833'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('512250', 'Cassey', 'Dolby', 1, (select top 1 id from usuario where username='512250'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('861674', 'Camile', 'Schouthede', 1, (select top 1 id from usuario where username='861674'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('188977', 'Celie', 'Brandle', 1, (select top 1 id from usuario where username='188977'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('122104', 'Bjorn', 'Zapata', 1, (select top 1 id from usuario where username='122104'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('112319', 'Meade', 'Byatt', 1, (select top 1 id from usuario where username='112319'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('25275', 'Enrique', 'Aumerle', 1, (select top 1 id from usuario where username='25275'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('277198', 'Delly', 'Martinez', 1, (select top 1 id from usuario where username='277198'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('807518', 'Beatrisa', 'Cowitz', 1, (select top 1 id from usuario where username='807518'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('635861', 'Erasmus', 'Champkin', 1, (select top 1 id from usuario where username='635861'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('725362', 'Zolly', 'Local', 1, (select top 1 id from usuario where username='725362'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('806724', 'Crystal', 'Pellingar', 1, (select top 1 id from usuario where username='806724'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('817472', 'Carola', 'Bunner', 1, (select top 1 id from usuario where username='817472'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('362474', 'Roddie', 'Mouget', 1, (select top 1 id from usuario where username='362474'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('418114', 'Nealon', 'Wonfor', 1, (select top 1 id from usuario where username='418114'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('689308', 'Agna', 'Seaward', 1, (select top 1 id from usuario where username='689308'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('41769', 'Scarlett', 'Deinert', 1, (select top 1 id from usuario where username='41769'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('732458', 'Ashlin', 'Arrigucci', 1, (select top 1 id from usuario where username='732458'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('66082', 'Toinette', 'Attrie', 1, (select top 1 id from usuario where username='66082'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('816904', 'Roddie', 'Tomaszynski', 1, (select top 1 id from usuario where username='816904'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('785917', 'Adamo', 'Joinsey', 1, (select top 1 id from usuario where username='785917'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('701056', 'Alexa', 'Maith', 1, (select top 1 id from usuario where username='701056'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('655130', 'Rachael', 'Brandenberg', 1, (select top 1 id from usuario where username='655130'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('486747', 'Ginger', 'Lamplough', 1, (select top 1 id from usuario where username='486747'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('96048', 'Rodi', 'Kuban', 1, (select top 1 id from usuario where username='96048'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('676821', 'Farleigh', 'Middle', 1, (select top 1 id from usuario where username='676821'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('411388', 'Boothe', 'Pothecary', 1, (select top 1 id from usuario where username='411388'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('867381', 'Marlane', 'Klosges', 1, (select top 1 id from usuario where username='867381'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('537605', 'Kelsi', 'Drinkeld', 1, (select top 1 id from usuario where username='537605'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('780368', 'Paulette', 'Warlaw', 1, (select top 1 id from usuario where username='780368'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('693004', 'Caz', 'Spaughton', 1, (select top 1 id from usuario where username='693004'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('487259', 'Austin', 'Sanday', 1, (select top 1 id from usuario where username='487259'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('948442', 'Romonda', 'Druitt', 1, (select top 1 id from usuario where username='948442'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('495345', 'Gordy', 'Cornelius', 1, (select top 1 id from usuario where username='495345'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('289414', 'Findley', 'Randleson', 1, (select top 1 id from usuario where username='289414'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('934215', 'Alika', 'Crayk', 1, (select top 1 id from usuario where username='934215'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('480379', 'Kienan', 'Vanyukhin', 1, (select top 1 id from usuario where username='480379'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('94735', 'Chelsy', 'Hercules', 1, (select top 1 id from usuario where username='94735'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('677876', 'Mathilda', 'McMillian', 1, (select top 1 id from usuario where username='677876'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('403219', 'Jean', 'Trevenu', 1, (select top 1 id from usuario where username='403219'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('12636', 'Etheline', 'Dubarry', 1, (select top 1 id from usuario where username='12636'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('642193', 'Franni', 'Amps', 1, (select top 1 id from usuario where username='642193'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('935889', 'Odell', 'Yerrell', 1, (select top 1 id from usuario where username='935889'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('851243', 'Briney', 'McMaster', 1, (select top 1 id from usuario where username='851243'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('209288', 'Elbert', 'Hatfull', 1, (select top 1 id from usuario where username='209288'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('143963', 'Luella', 'Phelip', 1, (select top 1 id from usuario where username='143963'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('401667', 'Sayers', 'Toten', 1, (select top 1 id from usuario where username='401667'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('280691', 'Cal', 'Grimme', 1, (select top 1 id from usuario where username='280691'));
insert into alumno(carnet,nombres,apellidos, id_pensum, id_usuario) values('971008', 'Marlena', 'Scardifield', 1, (select top 1 id from usuario where username='971008'));
