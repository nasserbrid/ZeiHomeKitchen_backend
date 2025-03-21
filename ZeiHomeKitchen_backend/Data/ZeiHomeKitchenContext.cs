﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ZeiHomeKitchen_backend.Models;

public partial class ZeiHomeKitchenContext : IdentityDbContext<Utilisateur, IdentityRole<int>, int>
{
    public ZeiHomeKitchenContext()
    {
    }

    public ZeiHomeKitchenContext(DbContextOptions<ZeiHomeKitchenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Paiement> Paiements { get; set; }

    public virtual DbSet<Plat> Plats { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Statistique> Statistiques { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=LAPTOP-BLNLO0HN\\MSSQLSERVER2022;Database=ZeiHomeKitchen;Integrated Security=True;Encrypt=False");
        // Server=LAPTOP-BLNLO0HN\\MSSQLSERVER2022;Database=ZeiHomeKitchen;Integrated Security=True;Encrypt=False

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Appeler base.OnModelCreating pour configurer les tables d'identité
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IdIngredient).HasName("PK__Ingredie__9D79738D795CDDBD");

            entity.ToTable("Ingredient");

            entity.Property(e => e.IdIngredient).HasColumnName("id_ingredient");
            entity.Property(e => e.Nom)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<Paiement>(entity =>
        {
            entity.HasKey(e => e.IdPaiement).HasName("PK__Paiement__72D44CFFB0ACBCD7");

            entity.ToTable("Paiement");

            entity.HasIndex(e => e.IdReservation, "UQ__Paiement__92EE588EE27DA788").IsUnique();

            entity.Property(e => e.IdPaiement).HasColumnName("id_paiement");
            entity.Property(e => e.IdReservation).HasColumnName("id_reservation");
            entity.Property(e => e.Montant)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("montant");
            entity.Property(e => e.Moyen)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("moyen");
            entity.Property(e => e.Statut)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("statut");

            entity.HasOne(d => d.IdReservationNavigation).WithOne(p => p.Paiement)
                .HasForeignKey<Paiement>(d => d.IdReservation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Paiement__id_res__47DBAE45");
        });

        modelBuilder.Entity<Plat>(entity =>
        {
            entity.HasKey(e => e.IdPlat).HasName("PK__Plat__3901EAE9C48A75E2");

            entity.ToTable("Plat");

            entity.Property(e => e.IdPlat).HasColumnName("id_plat");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Nom)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.Prix)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("prix");

            entity.HasMany(d => d.IdIngredients).WithMany(p => p.IdPlats)
                .UsingEntity<Dictionary<string, object>>(
                    "PlatIngredient",
                    r => r.HasOne<Ingredient>().WithMany()
                        .HasForeignKey("IdIngredient")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Plat_Ingr__id_in__4F7CD00D"),
                    l => l.HasOne<Plat>().WithMany()
                        .HasForeignKey("IdPlat")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Plat_Ingr__id_pl__4E88ABD4"),
                    j =>
                    {
                        j.HasKey("IdPlat", "IdIngredient").HasName("PK__Plat_Ing__F0D67DD1995683C7");
                        j.ToTable("Plat_Ingredient");
                        j.IndexerProperty<int>("IdPlat").HasColumnName("id_plat");
                        j.IndexerProperty<int>("IdIngredient").HasColumnName("id_ingredient");
                    });

            entity.HasMany(d => d.IdReservations).WithMany(p => p.IdPlats)
                .UsingEntity<Dictionary<string, object>>(
                    "PlatReservation",
                    r => r.HasOne<Reservation>().WithMany()
                        .HasForeignKey("IdReservation")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Plat_Rese__id_re__4BAC3F29"),
                    l => l.HasOne<Plat>().WithMany()
                        .HasForeignKey("IdPlat")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Plat_Rese__id_pl__4AB81AF0"),
                    j =>
                    {
                        j.HasKey("IdPlat", "IdReservation").HasName("PK__Plat_Res__D02F0F612D2E8B6D");
                        j.ToTable("Plat_Reservation");
                        j.IndexerProperty<int>("IdPlat").HasColumnName("id_plat");
                        j.IndexerProperty<int>("IdReservation").HasColumnName("id_reservation");
                    });
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.IdReservation).HasName("PK__Reservat__92EE588FA8CE9DBE");

            entity.ToTable("Reservation", tb => tb.HasTrigger("Statistique_Reservations"));

            entity.Property(e => e.IdReservation).HasColumnName("id_reservation");
            entity.Property(e => e.Adresse)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("adresse");
            entity.Property(e => e.DateReservation)
                .HasColumnType("datetime")
                .HasColumnName("date_reservation");
            entity.Property(e => e.IdStatistique).HasColumnName("id_statistique");
            entity.Property(e => e.IdUtilisateur).HasColumnName("id_utilisateur");
            entity.Property(e => e.Statut)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("statut");

            entity.HasOne(d => d.IdStatistiqueNavigation).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.IdStatistique)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__id_st__412EB0B6");

            entity.HasOne(d => d.IdUtilisateurNavigation).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.IdUtilisateur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__id_ut__4222D4EF");
        });

        modelBuilder.Entity<Statistique>(entity =>
        {
            entity.HasKey(e => e.IdStatistique).HasName("PK__Statisti__6AA7C9478605DF6E");

            entity.ToTable("Statistique");

            entity.Property(e => e.IdStatistique).HasColumnName("id_statistique");
            entity.Property(e => e.DateStatistique).HasColumnName("date_statistique");
            entity.Property(e => e.TotalReservation).HasColumnName("total_reservation");
        });

      

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
