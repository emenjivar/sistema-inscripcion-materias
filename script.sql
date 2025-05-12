CREATE DATABASE inscripcion_universitaria;
USE inscripcion_universitaria;

CREATE TABLE pensum(
    id INT PRIMARY KEY IDENTITY(1,1),
    carrera VARCHAR(60) NOT NULL
);

CREATE TABLE materia(
    id INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(30) UNIQUE NOT NULL,
    nombre VARCHAR(60) NOT NULL,
    unidades_valorativas INT NOT NULL,
    descripcion VARCHAR(120)
);

CREATE TABLE alumno(
    id INT PRIMARY KEY IDENTITY(1,1),
    carnet VARCHAR(6) UNIQUE NOT NULL,
    id_pensum INT,
    password VARCHAR(64) NOT NULL, -- deberia ser una tabla aparte, lo dejé acá por motivos de practicidad
    FOREIGN KEY(id_pensum) REFERENCES pensum(id)
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
