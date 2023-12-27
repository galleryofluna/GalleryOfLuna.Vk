﻿// <auto-generated />
using System;
using GalleryOfLuna.Vk.EntityFramework.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GalleryOfLuna.Vk.Publishing.EntityFramework.Sqlite.Migrations
{
    [DbContext(typeof(SqlitePublishingDbContext))]
    partial class SqlitePublishingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.2");

            modelBuilder.Entity("GalleryOfLuna.Vk.Publishing.EntityFramework.Model.PublishedImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PublishedOn")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Skipped")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("PublishedImages");
                });
#pragma warning restore 612, 618
        }
    }
}
