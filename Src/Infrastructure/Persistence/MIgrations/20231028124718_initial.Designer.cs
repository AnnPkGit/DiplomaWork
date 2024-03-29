﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Persistence.MIgrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231028124718_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Domain.Common.BaseToast", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("Deactivated")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("DeactivatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("BaseToasts");

                    b.HasDiscriminator<string>("Type").HasValue("BaseToast");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Domain.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Avatar")
                        .HasColumnType("longtext");

                    b.Property<string>("Bio")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("Deactivated")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("DeactivatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DeactivatedById");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Domain.Entities.MediaItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int?>("BaseToastWithContentId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BaseToastWithContentId");

                    b.ToTable("MediaItems");
                });

            modelBuilder.Entity("Domain.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Permissions", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "DeleteAccount"
                        },
                        new
                        {
                            Id = 2,
                            Name = "ReadAccountById"
                        },
                        new
                        {
                            Id = 3,
                            Name = "UpdateAccount"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Reaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Reacted")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ToastWithContentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ToastWithContentId");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "SuperAdministrator"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Administrator"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Registered"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Verified"
                        });
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("Deactivated")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("DeactivatedById")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("EmailVerified")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("EmailVerifyCode")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneVerified")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("PhoneVerifyCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeactivatedById");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Persistence.RelationshipTables.RolePermission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermission");

                    b.HasData(
                        new
                        {
                            RoleId = 3,
                            PermissionId = 2
                        },
                        new
                        {
                            RoleId = 3,
                            PermissionId = 3
                        });
                });

            modelBuilder.Entity("Infrastructure.Persistence.RelationshipTables.RoleUser", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("RoleUser");
                });

            modelBuilder.Entity("Domain.Common.BaseToastWithContent", b =>
                {
                    b.HasBaseType("Domain.Common.BaseToast");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasDiscriminator().HasValue("BaseToastWithContent");
                });

            modelBuilder.Entity("Domain.Entities.ReToast", b =>
                {
                    b.HasBaseType("Domain.Common.BaseToast");

                    b.Property<int>("ToastWithContentId")
                        .HasColumnType("int");

                    b.HasIndex("DeactivatedById");

                    b.HasIndex("ToastWithContentId");

                    b.HasDiscriminator().HasValue("ReToast");
                });

            modelBuilder.Entity("Domain.Entities.Quote", b =>
                {
                    b.HasBaseType("Domain.Common.BaseToastWithContent");

                    b.Property<int>("QuotedToastId")
                        .HasColumnType("int");

                    b.HasIndex("DeactivatedById");

                    b.HasIndex("QuotedToastId");

                    b.HasDiscriminator().HasValue("Quote");
                });

            modelBuilder.Entity("Domain.Entities.Reply", b =>
                {
                    b.HasBaseType("Domain.Common.BaseToastWithContent");

                    b.Property<int>("ReplyToToastId")
                        .HasColumnType("int");

                    b.HasIndex("DeactivatedById");

                    b.HasIndex("ReplyToToastId");

                    b.HasDiscriminator().HasValue("Reply");
                });

            modelBuilder.Entity("Domain.Entities.Toast", b =>
                {
                    b.HasBaseType("Domain.Common.BaseToastWithContent");

                    b.HasIndex("DeactivatedById");

                    b.HasDiscriminator().HasValue("Toast");
                });

            modelBuilder.Entity("Domain.Common.BaseToast", b =>
                {
                    b.HasOne("Domain.Entities.Account", "Author")
                        .WithMany("AllToasts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Domain.Entities.Account", b =>
                {
                    b.HasOne("Domain.Entities.User", "DeactivatedBy")
                        .WithMany()
                        .HasForeignKey("DeactivatedById");

                    b.HasOne("Domain.Entities.User", "Owner")
                        .WithOne("Account")
                        .HasForeignKey("Domain.Entities.Account", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeactivatedBy");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Domain.Entities.MediaItem", b =>
                {
                    b.HasOne("Domain.Entities.Account", "Author")
                        .WithMany("MediaItems")
                        .HasForeignKey("AuthorId");

                    b.HasOne("Domain.Common.BaseToastWithContent", null)
                        .WithMany("MediaItems")
                        .HasForeignKey("BaseToastWithContentId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Domain.Entities.Reaction", b =>
                {
                    b.HasOne("Domain.Entities.Account", "Author")
                        .WithMany("Reactions")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Common.BaseToastWithContent", "ToastWithContent")
                        .WithMany("Reactions")
                        .HasForeignKey("ToastWithContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("ToastWithContent");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.HasOne("Domain.Entities.User", "DeactivatedBy")
                        .WithMany()
                        .HasForeignKey("DeactivatedById");

                    b.Navigation("DeactivatedBy");
                });

            modelBuilder.Entity("Infrastructure.Persistence.RelationshipTables.RolePermission", b =>
                {
                    b.HasOne("Domain.Entities.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Persistence.RelationshipTables.RoleUser", b =>
                {
                    b.HasOne("Domain.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.ReToast", b =>
                {
                    b.HasOne("Domain.Entities.User", "DeactivatedBy")
                        .WithMany()
                        .HasForeignKey("DeactivatedById");

                    b.HasOne("Domain.Common.BaseToastWithContent", "ToastWithContent")
                        .WithMany("ReToasts")
                        .HasForeignKey("ToastWithContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeactivatedBy");

                    b.Navigation("ToastWithContent");
                });

            modelBuilder.Entity("Domain.Entities.Quote", b =>
                {
                    b.HasOne("Domain.Entities.User", "DeactivatedBy")
                        .WithMany()
                        .HasForeignKey("DeactivatedById");

                    b.HasOne("Domain.Common.BaseToastWithContent", "QuotedToast")
                        .WithMany("Quotes")
                        .HasForeignKey("QuotedToastId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeactivatedBy");

                    b.Navigation("QuotedToast");
                });

            modelBuilder.Entity("Domain.Entities.Reply", b =>
                {
                    b.HasOne("Domain.Entities.User", "DeactivatedBy")
                        .WithMany()
                        .HasForeignKey("DeactivatedById");

                    b.HasOne("Domain.Common.BaseToastWithContent", "ReplyToToast")
                        .WithMany("Replies")
                        .HasForeignKey("ReplyToToastId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeactivatedBy");

                    b.Navigation("ReplyToToast");
                });

            modelBuilder.Entity("Domain.Entities.Toast", b =>
                {
                    b.HasOne("Domain.Entities.User", "DeactivatedBy")
                        .WithMany("DeactivatedToasts")
                        .HasForeignKey("DeactivatedById");

                    b.Navigation("DeactivatedBy");
                });

            modelBuilder.Entity("Domain.Entities.Account", b =>
                {
                    b.Navigation("AllToasts");

                    b.Navigation("MediaItems");

                    b.Navigation("Reactions");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("Account");

                    b.Navigation("DeactivatedToasts");
                });

            modelBuilder.Entity("Domain.Common.BaseToastWithContent", b =>
                {
                    b.Navigation("MediaItems");

                    b.Navigation("Quotes");

                    b.Navigation("ReToasts");

                    b.Navigation("Reactions");

                    b.Navigation("Replies");
                });
#pragma warning restore 612, 618
        }
    }
}
