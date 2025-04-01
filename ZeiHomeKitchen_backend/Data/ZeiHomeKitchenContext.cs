using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ZeiHomeKitchen_backend.Models;

public partial class ZeiHomeKitchenContext : IdentityDbContext<Utilisateur, IdentityRole<int>, int>
{
    /// <summary>
    /// Premier constructeur est un constructeur par défaut
    /// </summary>
    public ZeiHomeKitchenContext()
    {
    }

    /// <summary>
    /// Deuxième constructeur permet d'injecter des options de configurations (la chaine de connexion de la bdd) via DbContextOptions. 
    /// </summary>
    /// <param name="options"></param>
    public ZeiHomeKitchenContext(DbContextOptions<ZeiHomeKitchenContext> options)
        : base(options)
    {
    }

    //Les DbSet<T> correspondent à une table dans la base de données.
    //Ces propriétés permettent à Entity Framework d'interagir avec ces entités (CRUD).
    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Paiement> Paiements { get; set; }

    public virtual DbSet<Plat> Plats { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Statistique> Statistiques { get; set; }

    /// <summary>
    /// Cette méthode configure la connexion à la base de données.
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=LAPTOP-BLNLO0HN\\MSSQLSERVER2022;Database=ZeiHomeKitchen;Integrated Security=True;Encrypt=False");
    // Server=LAPTOP-BLNLO0HN\\MSSQLSERVER2022;Database=ZeiHomeKitchen;Integrated Security=True;Encrypt=False

    /// <summary>
    /// La méthode OnModelCreating configure les relations entre les tables et les contraintes.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ingredient>(entity =>
        {
            //Haskey définit la clé primaire d'une table
            //HasMexLength impose une limite de caractères
            //Nous effectuons ici un mapping entre les propriétés en C# et les colonnes en bdd.
            entity.HasKey(e => e.IdIngredient).HasName("PK__Ingredie__9D79738D795CDDBD");
            entity.ToTable("Ingredient");
            entity.Property(e => e.IdIngredient).HasColumnName("id_ingredient");
            entity.Property(e => e.Nom).HasMaxLength(150).IsUnicode(false).HasColumnName("nom");
        });

        //Configuration de Paiement avec une relation one-to-one avec Reservation.
        modelBuilder.Entity<Paiement>(entity =>
        {
            entity.HasKey(e => e.IdPaiement).HasName("PK__Paiement__72D44CFFB0ACBCD7");
            entity.ToTable("Paiement");
            entity.HasIndex(e => e.IdReservation, "UQ__Paiement__92EE588EE27DA788").IsUnique();
            entity.Property(e => e.IdPaiement).HasColumnName("id_paiement");
            entity.Property(e => e.IdReservation).HasColumnName("id_reservation");
            entity.Property(e => e.Montant).HasColumnType("decimal(10, 2)").HasColumnName("montant");
            entity.Property(e => e.Moyen).HasMaxLength(50).IsUnicode(false).HasColumnName("moyen");
            entity.Property(e => e.Statut).HasMaxLength(20).IsUnicode(false).HasColumnName("statut");

            entity.HasOne(d => d.ReservationNavigation).WithOne(p => p.Paiement)
                .HasForeignKey<Paiement>(d => d.IdReservation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Paiement__id_res__47DBAE45");
        });
        //Configuration de Plat avec relations Many-To-Many avec Ingredient et Reservation.
        modelBuilder.Entity<Plat>(entity =>
        {
            entity.HasKey(e => e.IdPlat).HasName("PK__Plat__3901EAE9C48A75E2");
            entity.ToTable("Plat");
            entity.Property(e => e.IdPlat).HasColumnName("id_plat");
            entity.Property(e => e.Description).IsUnicode(false).HasColumnName("description");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Nom).HasMaxLength(150).IsUnicode(false).HasColumnName("nom");
            entity.Property(e => e.Prix).HasColumnType("decimal(10, 2)").HasColumnName("prix");

            entity.HasMany(d => d.Ingredients).WithMany(p => p.Plats)
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
                        //Ici on relation many-to-many entre Plat et Ingredient via une table de liaison Plat_Ingredient
                        j.ToTable("Plat_Ingredient");
                        j.IndexerProperty<int>("IdPlat").HasColumnName("id_plat");
                        j.IndexerProperty<int>("IdIngredient").HasColumnName("id_ingredient");
                    });

            entity.HasMany(d => d.Reservations).WithMany(p => p.Plats)
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
                        //Ici on relation many-to-many entre Plat et Reservation via une table de liaison Plat_Reservation
                        j.ToTable("Plat_Reservation");
                        j.IndexerProperty<int>("IdPlat").HasColumnName("id_plat");
                        j.IndexerProperty<int>("IdReservation").HasColumnName("id_reservation");
                    });
        });
        //Configuration de Reservation 
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.IdReservation).HasName("PK__Reservat__92EE588FA8CE9DBE");
            //HasTrigger nous indique que nous avons mis un trigger SQL sur la table Reservation.
            entity.ToTable("Reservation", tb => tb.HasTrigger("Statistique_Reservations"));
            entity.Property(e => e.IdReservation).HasColumnName("id_reservation");
            entity.Property(e => e.Adresse).HasMaxLength(255).IsUnicode(false).HasColumnName("adresse");
            entity.Property(e => e.DateReservation).HasColumnType("datetime").HasColumnName("date_reservation");
            entity.Property(e => e.IdStatistique).HasColumnName("id_statistique");
            entity.Property(e => e.IdUtilisateur).HasColumnName("id_utilisateur");
            entity.Property(e => e.Statut).HasMaxLength(50).IsUnicode(false).HasColumnName("statut");

            entity.HasOne(d => d.StatistiqueNavigation).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.IdStatistique)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__id_st__412EB0B6");

            entity.HasOne(d => d.UtilisateurNavigation).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.IdUtilisateur)
                //.OnDelete(DeleteBehavior.ClientSetNull)
                //Si pas d'utilisateur, pas de reservation
                .OnDelete(DeleteBehavior.Cascade)
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

    /// <summary>
    /// Méthode qui permet de compléter la configuration du modèle dans une autre partie du code.
    /// Cela est utilie si la configuration du modèle est répartie sur plusieurs fichiers.
    /// </summary>
    /// <param name="modelBuilder"></param>
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
