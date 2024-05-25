﻿// <auto-generated />
using System;
using CoolTattooApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CoolTattooApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("CoolTattooApi.Models.Bisuteria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Fotografia")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Bisuteria");
                });

            modelBuilder.Entity("CoolTattooApi.Models.Clientes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .HasColumnType("longtext");

                    b.Property<int>("Num_Tattoos")
                        .HasColumnType("int");

                    b.Property<bool>("Publicidad")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("TatuadorId")
                        .HasColumnType("int");

                    b.Property<int?>("Telefono")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TatuadorId");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("CoolTattooApi.Models.ImagenesTattoo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Fotografia")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<int>("TatuadorId")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("TatuadorId");

                    b.ToTable("Imagenes");
                });

            modelBuilder.Entity("CoolTattooApi.Models.Publicidad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("FechaFin")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("FechaInicio")
                        .HasColumnType("datetime(6)");

                    b.Property<byte[]>("Imagen")
                        .HasColumnType("longblob");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Publicidad");
                });

            modelBuilder.Entity("CoolTattooApi.Models.Tatuador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Foto")
                        .HasColumnType("longblob");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<int>("Telefono")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tatuador");
                });

            modelBuilder.Entity("CoolTattooApi.Models.Clientes", b =>
                {
                    b.HasOne("CoolTattooApi.Models.Tatuador", "Tatuador")
                        .WithMany("Clientes")
                        .HasForeignKey("TatuadorId");

                    b.Navigation("Tatuador");
                });

            modelBuilder.Entity("CoolTattooApi.Models.ImagenesTattoo", b =>
                {
                    b.HasOne("CoolTattooApi.Models.Tatuador", "Tatuador")
                        .WithMany("ImagenesTattoo")
                        .HasForeignKey("TatuadorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tatuador");
                });

            modelBuilder.Entity("CoolTattooApi.Models.Tatuador", b =>
                {
                    b.Navigation("Clientes");

                    b.Navigation("ImagenesTattoo");
                });
#pragma warning restore 612, 618
        }
    }
}
