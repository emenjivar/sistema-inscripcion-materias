CREATE DATABASE inscripcion_universitaria;
GO

USE inscripcion_universitaria;
GO

CREATE TABLE pensum(
    id INT PRIMARY KEY IDENTITY(1,1),
    carrera VARCHAR(60) NOT NULL
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
    dia_semana VARCHAR(2) NOT NULL CHECK (dia_semana in ('lu', 'ma', 'mi', 'ju', 'vi', 'sa', 'do')),
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

SET IDENTITY_INSERT pensum ON
insert into pensum(id, carrera) values (1, 'Ingenieria en sistemas');
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