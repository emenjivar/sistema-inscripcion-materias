using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InscripcionMaterias.Models;

public partial class GestionDbContext : DbContext
{
    public GestionDbContext()
    {
    }

    public GestionDbContext(DbContextOptions<GestionDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumnos { get; set; }

    public virtual DbSet<BloqueHorarioMaterial> BloqueHorarioMaterials { get; set; }

    public virtual DbSet<GrupoClase> GrupoClases { get; set; }

    public virtual DbSet<Inscripcion> Inscripcions { get; set; }

    public virtual DbSet<InscripcionAlumno> InscripcionAlumnos { get; set; }

    public virtual DbSet<Materium> Materia { get; set; }

    public virtual DbSet<Pensum> Pensums { get; set; }

    public virtual DbSet<PensumMateria> PensumMaterias { get; set; }

    public virtual DbSet<ResultadoCicloAcademico> ResultadoCicloAcademicos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=PC-ANSP-1593;Database=inscripcion_universitaria;Trusted_Connection=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__alumno__3213E83F3A3B16BF");

            entity.ToTable("alumno");

            entity.HasIndex(e => e.Carnet, "UQ__alumno__4CDEAA6EA0EC4AA2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("apellidos");
            entity.Property(e => e.Carnet)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("carnet");
            entity.Property(e => e.IdPensum).HasColumnName("id_pensum");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Nombres)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("nombres");

            entity.HasOne(d => d.IdPensumNavigation).WithMany(p => p.Alumnos)
                .HasForeignKey(d => d.IdPensum)
                .HasConstraintName("FK__alumno__id_pensu__403A8C7D");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Alumnos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__alumno__id_usuar__412EB0B6");
        });

        modelBuilder.Entity<BloqueHorarioMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__bloque_h__3213E83F30913B76");

            entity.ToTable("bloque_horario_material");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiaSemana)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("dia_semana");
            entity.Property(e => e.HoraFin).HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.IdGrupo).HasColumnName("id_grupo");
            entity.Property(e => e.IdInscripcion).HasColumnName("id_inscripcion");
            entity.Property(e => e.IdMateria).HasColumnName("id_materia");

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.BloqueHorarioMaterials)
                .HasForeignKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__bloque_ho__id_gr__5165187F");

            entity.HasOne(d => d.IdInscripcionNavigation).WithMany(p => p.BloqueHorarioMaterials)
                .HasForeignKey(d => d.IdInscripcion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__bloque_ho__id_in__4F7CD00D");

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.BloqueHorarioMaterials)
                .HasForeignKey(d => d.IdMateria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__bloque_ho__id_ma__5070F446");
        });

        modelBuilder.Entity<GrupoClase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__grupo_cl__3213E83FD201E7CD");

            entity.ToTable("grupo_clase");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codigo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codigo");
        });

        modelBuilder.Entity<Inscripcion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__inscripc__3213E83FB48D358D");

            entity.ToTable("inscripcion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.CicloAcademico).HasColumnName("ciclo_academico");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.IdPensum).HasColumnName("id_pensum");

            entity.HasOne(d => d.IdPensumNavigation).WithMany(p => p.Inscripcions)
                .HasForeignKey(d => d.IdPensum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inscripci__id_pe__49C3F6B7");
        });

        modelBuilder.Entity<InscripcionAlumno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__inscripc__3213E83FD7D9AA49");

            entity.ToTable("inscripcion_alumno");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdAlumno).HasColumnName("id_alumno");
            entity.Property(e => e.IdBloqueHorarioMateria).HasColumnName("id_bloque_horario_materia");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.InscripcionAlumnos)
                .HasForeignKey(d => d.IdAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inscripci__id_al__5441852A");

            entity.HasOne(d => d.IdBloqueHorarioMateriaNavigation).WithMany(p => p.InscripcionAlumnos)
                .HasForeignKey(d => d.IdBloqueHorarioMateria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inscripci__id_bl__5535A963");
        });

        modelBuilder.Entity<Materium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__materia__3213E83FDAC676D1");

            entity.ToTable("materia");

            entity.HasIndex(e => e.Codigo, "UQ__materia__40F9A2067F552A40").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codigo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("codigo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.UnidadesValorativas).HasColumnName("unidades_valorativas");
        });

        modelBuilder.Entity<Pensum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pensum__3213E83F5BCBF129");

            entity.ToTable("pensum");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CantidadCiclos).HasColumnName("cantidad_ciclos");
            entity.Property(e => e.Carrera)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("carrera");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.TipoCarrera)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("tipo_carrera");
        });

        modelBuilder.Entity<PensumMateria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pensum_m__3213E83F27E3CBFA");

            entity.ToTable("pensum_materias");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CicloCurricular).HasColumnName("ciclo_curricular");
            entity.Property(e => e.IdMateria).HasColumnName("id_materia");
            entity.Property(e => e.IdMateriaPrerequisito).HasColumnName("id_materia_prerequisito");
            entity.Property(e => e.IdPensum).HasColumnName("id_pensum");

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.PensumMateriaIdMateriaNavigations)
                .HasForeignKey(d => d.IdMateria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pensum_ma__id_ma__44FF419A");

            entity.HasOne(d => d.IdMateriaPrerequisitoNavigation).WithMany(p => p.PensumMateriaIdMateriaPrerequisitoNavigations)
                .HasForeignKey(d => d.IdMateriaPrerequisito)
                .HasConstraintName("FK__pensum_ma__id_ma__45F365D3");

            entity.HasOne(d => d.IdPensumNavigation).WithMany(p => p.PensumMateria)
                .HasForeignKey(d => d.IdPensum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pensum_ma__id_pe__440B1D61");
        });

        modelBuilder.Entity<ResultadoCicloAcademico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__resultad__3213E83FEA82DAD5");

            entity.ToTable("resultado_ciclo_academico");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Aprobado).HasColumnName("aprobado");
            entity.Property(e => e.IdAlumno).HasColumnName("id_alumno");
            entity.Property(e => e.IdMateria).HasColumnName("id_materia");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.ResultadoCicloAcademicos)
                .HasForeignKey(d => d.IdAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resultado__id_al__5812160E");

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.ResultadoCicloAcademicos)
                .HasForeignKey(d => d.IdMateria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resultado__id_ma__59063A47");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuario__3213E83F3933383B");

            entity.ToTable("usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("apellidos");
            entity.Property(e => e.Email)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombres)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("nombres");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Rol)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("rol");
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
