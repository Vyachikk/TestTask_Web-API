using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestTask.Models;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ASUS-MAIN\\SQLEXPRESS;Database=TaskDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("doctors");

            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.DoctorName)
                .HasMaxLength(20)
                .HasColumnName("doctor_name");
            entity.Property(e => e.DoctorPatronymic)
                .HasMaxLength(20)
                .HasColumnName("doctor_patronymic");
            entity.Property(e => e.DoctorSurname)
                .HasMaxLength(20)
                .HasColumnName("doctor_surname");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");

            entity.HasOne(d => d.Region).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK_doctors_regions");

            entity.HasOne(d => d.Room).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_doctors_rooms");

            entity.HasOne(d => d.Specialization).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_doctors_specializations");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("patients");

            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.PatientAddress)
                .HasMaxLength(200)
                .HasColumnName("patient_address");
            entity.Property(e => e.PatientGender).HasColumnName("patient_gender");
            entity.Property(e => e.PatientName)
                .HasMaxLength(50)
                .HasColumnName("patient_name");
            entity.Property(e => e.PatientPatronymic)
                .HasMaxLength(50)
                .HasColumnName("patient_patronymic");
            entity.Property(e => e.PatientRegion).HasColumnName("patient_region");
            entity.Property(e => e.PatientSurname)
                .HasMaxLength(50)
                .HasColumnName("patient_surname");

            entity.HasOne(d => d.Region).WithMany(p => p.Patients)
                .HasForeignKey(d => d.PatientRegion)
                .HasConstraintName("FK_patient_regions");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.ToTable("regions");

            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.RegionNumber).HasColumnName("region_number");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("rooms");

            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.RoomNumber).HasColumnName("room_number");
            entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.ToTable("specializations");

            entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");
            entity.Property(e => e.SpecializationName)
                .HasMaxLength(50)
                .HasColumnName("specialization_name");
            entity.Property(e => e.SpecializationNumber).HasColumnName("specialization_number");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
