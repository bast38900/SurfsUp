﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SurfsUp.Data;

#nullable disable

namespace SurfsUp.Migrations
{
    [DbContext(typeof(SurfsUpContext))]
    partial class SurfsUpContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SurfsUp.Models.Board", b =>
                {
                    b.Property<Guid>("BoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BoardName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Equipment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Length")
                        .HasColumnType("float");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<double>("Thickness")
                        .HasColumnType("float");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double>("Volume")
                        .HasColumnType("float");

                    b.Property<double>("Width")
                        .HasColumnType("float");

                    b.HasKey("BoardId");

                    b.HasIndex("OrderId");

                    b.ToTable("Board");
                });

            modelBuilder.Entity("SurfsUp.Models.Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfDelivery")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfSubmission")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("OrderId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("SurfsUp.Models.Board", b =>
                {
                    b.HasOne("SurfsUp.Models.Order", "Order")
                        .WithMany("Boards")
                        .HasForeignKey("OrderId");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("SurfsUp.Models.Order", b =>
                {
                    b.Navigation("Boards");
                });
#pragma warning restore 612, 618
        }
    }
}
