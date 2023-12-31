﻿// <auto-generated />
using System;
using BackEnd.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BackEnd.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.7.23375.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BackEnd.Database.Models.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDelete = false,
                            Name = "SuperAdmin",
                            NormalizedName = "SUPERADMIN"
                        },
                        new
                        {
                            Id = 2,
                            IsDelete = false,
                            Name = "Kasir",
                            NormalizedName = "KASIR"
                        });
                });

            modelBuilder.Entity("BackEnd.Database.Models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FireBaseToken")
                        .HasMaxLength(511)
                        .HasColumnType("nvarchar(511)");

                    b.Property<string>("FullName")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLogin")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSuperAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Keywords")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Nik")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RainbowId")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tags")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("ParentId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("BackEnd.Database.Models.Application", b =>
                {
                    b.Property<string>("AppId")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("AppName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.HasKey("AppId");

                    b.ToTable("Applications");

                    b.HasData(
                        new
                        {
                            AppId = "WebScrap",
                            AppName = "Web Crawler System",
                            IsDelete = false
                        });
                });

            modelBuilder.Entity("BackEnd.Database.Models.GroupMenu", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("MenuId")
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsAdd")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEdit")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("UpdateBy")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("RoleId", "MenuId");

                    b.HasIndex("MenuId");

                    b.ToTable("GroupMenus");
                });

            modelBuilder.Entity("BackEnd.Database.Models.Menu", b =>
                {
                    b.Property<int>("MenuId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MenuId"));

                    b.Property<string>("AppId")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Icon")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int?>("Kolom")
                        .HasColumnType("int");

                    b.Property<int>("MenuIndex")
                        .HasColumnType("int");

                    b.Property<string>("MenuLink")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("MenuName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("ParentMenuId")
                        .HasColumnType("int");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UpdateBy")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("MenuId");

                    b.HasIndex("AppId");

                    b.HasIndex("MenuId");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("MenuId"), new[] { "AppId" });

                    b.HasIndex("ParentMenuId");

                    b.ToTable("Menus");

                    b.HasData(
                        new
                        {
                            MenuId = 1,
                            AppId = "WebScrap",
                            Icon = "settings",
                            IsActive = true,
                            IsDeleted = false,
                            MenuIndex = 1,
                            MenuLink = "#",
                            MenuName = "Administrator",
                            UniqueName = "administrator"
                        },
                        new
                        {
                            MenuId = 2,
                            AppId = "WebScrap",
                            Icon = "people",
                            IsActive = true,
                            IsDeleted = false,
                            MenuIndex = 1,
                            MenuLink = "/AdminConsole/usermanage",
                            MenuName = "Users",
                            ParentMenuId = 1,
                            UniqueName = "users"
                        },
                        new
                        {
                            MenuId = 3,
                            AppId = "WebScrap",
                            Icon = "control_point",
                            IsActive = true,
                            IsDeleted = false,
                            MenuIndex = 2,
                            MenuLink = "/AdminConsole/rolemanage",
                            MenuName = "Roles",
                            ParentMenuId = 1,
                            UniqueName = "roles"
                        },
                        new
                        {
                            MenuId = 4,
                            AppId = "WebScrap",
                            Icon = "menus",
                            IsActive = true,
                            IsDeleted = false,
                            MenuIndex = 3,
                            MenuLink = "/AdminConsole/menumanage",
                            MenuName = "Menus",
                            ParentMenuId = 1,
                            UniqueName = "menus"
                        },
                        new
                        {
                            MenuId = 5,
                            AppId = "WebScrap",
                            Icon = "fingerprint",
                            IsActive = true,
                            IsDeleted = false,
                            MenuIndex = 4,
                            MenuLink = "/AdminConsole/privilegemanage",
                            MenuName = "Privileges",
                            ParentMenuId = 1,
                            UniqueName = "privileges"
                        },
                        new
                        {
                            MenuId = 6,
                            AppId = "WebScrap",
                            Icon = "thumb_up",
                            IsActive = true,
                            IsDeleted = false,
                            MenuIndex = 5,
                            MenuLink = "/master",
                            MenuName = "Master",
                            ParentMenuId = 1,
                            UniqueName = "masters"
                        });
                });

            modelBuilder.Entity("BackEnd.Database.Models.NotifData", b =>
                {
                    b.Property<int>("NotifDataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotifDataId"));

                    b.Property<int?>("AppUserId")
                        .HasColumnType("int");

                    b.Property<string>("Auth")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("P256dh")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Url")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.HasKey("NotifDataId");

                    b.HasIndex("AppUserId");

                    b.ToTable("NotifDatas");
                });

            modelBuilder.Entity("BackEnd.Database.Models.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDirectToUrl")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("ModuleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("To")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("BackEnd.Database.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreateBy")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Deskripsi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HargaSatuan")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("Jumlah")
                        .HasColumnType("int");

                    b.Property<string>("Nama")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TanggalKadaluarsa")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateBy")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("BackEnd.Database.Models.ProductTansaction", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<long>("TransactionId")
                        .HasColumnType("bigint");

                    b.Property<int>("Kuantitas")
                        .HasColumnType("int");

                    b.HasKey("ProductId", "TransactionId");

                    b.HasIndex("TransactionId");

                    b.ToTable("ProductTansactions");
                });

            modelBuilder.Entity("BackEnd.Database.Models.Transaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AlamatTujuan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("AppUserId")
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("TotalHarga")
                        .HasColumnType("int");

                    b.Property<string>("UpdateBy")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("BackEnd.Database.Models.UserApplication", b =>
                {
                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("AppId")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLogin")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LoginDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateBy")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("UserId", "AppId", "RoleId");

                    b.HasIndex("AppId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserApplications");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BackEnd.Database.Models.AppUser", b =>
                {
                    b.HasOne("BackEnd.Database.Models.AppUser", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("BackEnd.Database.Models.GroupMenu", b =>
                {
                    b.HasOne("BackEnd.Database.Models.Menu", "Menu")
                        .WithMany("GroupMenus")
                        .HasForeignKey("MenuId")
                        .IsRequired();

                    b.HasOne("BackEnd.Database.Models.AppRole", "AppRole")
                        .WithMany("GroupMenus")
                        .HasForeignKey("RoleId")
                        .IsRequired();

                    b.Navigation("AppRole");

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("BackEnd.Database.Models.Menu", b =>
                {
                    b.HasOne("BackEnd.Database.Models.Application", "Application")
                        .WithMany("Menus")
                        .HasForeignKey("AppId");

                    b.HasOne("BackEnd.Database.Models.Menu", "ParentMenu")
                        .WithMany("ChildMenu")
                        .HasForeignKey("ParentMenuId");

                    b.Navigation("Application");

                    b.Navigation("ParentMenu");
                });

            modelBuilder.Entity("BackEnd.Database.Models.NotifData", b =>
                {
                    b.HasOne("BackEnd.Database.Models.AppUser", "AppUser")
                        .WithMany("NotifDatas")
                        .HasForeignKey("AppUserId");

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("BackEnd.Database.Models.ProductTansaction", b =>
                {
                    b.HasOne("BackEnd.Database.Models.Product", "Product")
                        .WithMany("ProductTansactions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackEnd.Database.Models.Transaction", "Transaction")
                        .WithMany("ProductTansactions")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("BackEnd.Database.Models.Transaction", b =>
                {
                    b.HasOne("BackEnd.Database.Models.AppUser", "AppUser")
                        .WithMany("Transactions")
                        .HasForeignKey("AppUserId");

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("BackEnd.Database.Models.UserApplication", b =>
                {
                    b.HasOne("BackEnd.Database.Models.Application", "Application")
                        .WithMany("UserApplications")
                        .HasForeignKey("AppId");

                    b.HasOne("BackEnd.Database.Models.AppRole", "AppRole")
                        .WithMany("UserApplications")
                        .HasForeignKey("RoleId");

                    b.HasOne("BackEnd.Database.Models.AppUser", "User")
                        .WithMany("UserApplications")
                        .HasForeignKey("UserId");

                    b.Navigation("AppRole");

                    b.Navigation("Application");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("BackEnd.Database.Models.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("BackEnd.Database.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("BackEnd.Database.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("BackEnd.Database.Models.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackEnd.Database.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("BackEnd.Database.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BackEnd.Database.Models.AppRole", b =>
                {
                    b.Navigation("GroupMenus");

                    b.Navigation("UserApplications");
                });

            modelBuilder.Entity("BackEnd.Database.Models.AppUser", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("NotifDatas");

                    b.Navigation("Transactions");

                    b.Navigation("UserApplications");
                });

            modelBuilder.Entity("BackEnd.Database.Models.Application", b =>
                {
                    b.Navigation("Menus");

                    b.Navigation("UserApplications");
                });

            modelBuilder.Entity("BackEnd.Database.Models.Menu", b =>
                {
                    b.Navigation("ChildMenu");

                    b.Navigation("GroupMenus");
                });

            modelBuilder.Entity("BackEnd.Database.Models.Product", b =>
                {
                    b.Navigation("ProductTansactions");
                });

            modelBuilder.Entity("BackEnd.Database.Models.Transaction", b =>
                {
                    b.Navigation("ProductTansactions");
                });
#pragma warning restore 612, 618
        }
    }
}
