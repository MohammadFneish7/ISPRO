﻿// <auto-generated />
using System;
using ISPRO.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ISPRO.Persistence.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220830163109_Create_Database")]
    partial class Create_Database
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ISPRO.Persistence.Entities.AdminAccount", b =>
                {
                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("ExpiryDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Mobile")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Username");

                    b.ToTable("AdminAccounts");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.CashPayment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Ammount")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("PaymentDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("RechargePeriod")
                        .HasColumnType("time");

                    b.Property<string>("UserAccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("UserAccountId");

                    b.ToTable("CashPayments");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.ManagerAccount", b =>
                {
                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("ExpiryDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Mobile")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Username");

                    b.ToTable("ManagerAccounts");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.PrePaidCard", b =>
                {
                    b.Property<long>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Code"), 1L, 1);

                    b.Property<string>("ConsumerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("ConsumptionDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ExpiryDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("RechargePeriod")
                        .HasColumnType("time");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int");

                    b.HasKey("Code");

                    b.HasIndex("ConsumerId");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("PrePaidCards");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.Project", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime?>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProjectManagerUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Name");

                    b.HasIndex("ProjectManagerUsername");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Bandwidth")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Quota")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectName");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.UserAccount", b =>
                {
                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("ExpiryDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Mobile")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("ResumeDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Username");

                    b.HasIndex("ProjectName");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.CashPayment", b =>
                {
                    b.HasOne("ISPRO.Persistence.Entities.UserAccount", "UserAccount")
                        .WithMany("CashPayments")
                        .HasForeignKey("UserAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserAccount");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.PrePaidCard", b =>
                {
                    b.HasOne("ISPRO.Persistence.Entities.UserAccount", "Consumer")
                        .WithMany("PrePaidCards")
                        .HasForeignKey("ConsumerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ISPRO.Persistence.Entities.Subscription", "Subscription")
                        .WithMany("PrePaidCards")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Consumer");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.Project", b =>
                {
                    b.HasOne("ISPRO.Persistence.Entities.ManagerAccount", "ProjectManager")
                        .WithMany("Projects")
                        .HasForeignKey("ProjectManagerUsername")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProjectManager");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.Subscription", b =>
                {
                    b.HasOne("ISPRO.Persistence.Entities.Project", "Project")
                        .WithMany("Subscriptions")
                        .HasForeignKey("ProjectName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.UserAccount", b =>
                {
                    b.HasOne("ISPRO.Persistence.Entities.Project", "Project")
                        .WithMany("UserAccounts")
                        .HasForeignKey("ProjectName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ISPRO.Persistence.Entities.Subscription", "Subscription")
                        .WithMany("Subscribers")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.ManagerAccount", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.Project", b =>
                {
                    b.Navigation("Subscriptions");

                    b.Navigation("UserAccounts");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.Subscription", b =>
                {
                    b.Navigation("PrePaidCards");

                    b.Navigation("Subscribers");
                });

            modelBuilder.Entity("ISPRO.Persistence.Entities.UserAccount", b =>
                {
                    b.Navigation("CashPayments");

                    b.Navigation("PrePaidCards");
                });
#pragma warning restore 612, 618
        }
    }
}
