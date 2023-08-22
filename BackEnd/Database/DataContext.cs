using Audit.EntityFramework;
using BackEnd.Database.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Database;

public class DataContext : IdentityDbContext<AppUser, AppRole, int>
{
    private readonly DbContextHelper _helper = new();

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        IAuditDbContext auditContext = new DefaultAuditContext(this);
        _helper.SetConfig(auditContext);
    }

    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<AppRole> AppRoles => Set<AppRole>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<UserApplication> UserApplications => Set<UserApplication>();
    public DbSet<Menu> Menus => Set<Menu>();
    //public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupMenu> GroupMenus => Set<GroupMenu>();
    public DbSet<NotifData> NotifDatas => Set<NotifData>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<ProductTansaction> ProductTansactions => Set<ProductTansaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //_ = modelBuilder.HasDefaultSchema("UserRoleService");

        foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType? entityType in modelBuilder.Model.GetEntityTypes()
            .Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType)))
        {
            _ = modelBuilder.Entity(entityType.ClrType)
                .Property<string>("CreateBy").HasMaxLength(500).IsRequired(false);

            _ = modelBuilder.Entity(entityType.ClrType)
                .Property<DateTime>("CreateDate")
                .HasDefaultValueSql(@"GETDATE()");

            _ = modelBuilder.Entity(entityType.ClrType)
                .Property<string>("UpdateBy").HasMaxLength(500).IsRequired(false);

            _ = modelBuilder.Entity(entityType.ClrType)
                .Property<DateTime>("UpdateDate").HasDefaultValueSql(@"GETDATE()");
        }

        _ = modelBuilder.Entity<AppUser>(a =>
        {
            _ = a.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            _ = a.HasQueryFilter(r => r.IsActive);
        });

        _ = modelBuilder.Entity<Application>(a =>
        {
            _ = a.HasData(
                new Application { AppId = "WebScrap", AppName = "Web Crawler System", IsDelete = false }
            );
        });

        _ = modelBuilder.Entity<AppRole>(a =>
        {
            //a.IsMemoryOptimized(true);
            _ = a.HasData(
               new AppRole { Id = 1, Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
               new AppRole { Id = 2, Name = "Kasir", NormalizedName = "KASIR" }
           );
        });

        _ = modelBuilder.Entity<Menu>(a =>
        {
            //a.IsMemoryOptimized(true);
            _ = a.HasIndex(p => p.MenuId)
                .IncludeProperties(p => p.AppId);

            _ = a.HasQueryFilter(r => !r.IsDeleted);

            _ = a.HasOne(p => p.ParentMenu)
                .WithMany(t => t.ChildMenu)
                .HasForeignKey(e => e.ParentMenuId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            _ = a.HasOne(p => p.Application)
                .WithMany(t => t.Menus)
                .HasForeignKey(e => e.AppId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            _ = a.HasData(
                new Menu { MenuId = 1, AppId = "WebScrap", MenuName = "Administrator", UniqueName = "administrator", Icon = "settings", MenuLink = "#", MenuIndex = 1, IsActive = true, IsDeleted = false  },
                new Menu { MenuId = 2, AppId = "WebScrap", ParentMenuId = 1, MenuName = "Users", UniqueName = "users", Icon = "people", MenuLink = "/AdminConsole/usermanage", MenuIndex = 1, IsActive = true, IsDeleted = false },
                new Menu { MenuId = 3, AppId = "WebScrap", ParentMenuId = 1, MenuName = "Roles", UniqueName = "roles", Icon = "control_point", MenuLink = "/AdminConsole/rolemanage", MenuIndex = 2, IsActive = true, IsDeleted = false },
                new Menu { MenuId = 4, AppId = "WebScrap", ParentMenuId = 1, MenuName = "Menus", UniqueName = "menus", Icon = "menus", MenuLink = "/AdminConsole/menumanage", MenuIndex = 3, IsActive = true, IsDeleted = false },
                new Menu { MenuId = 5, AppId = "WebScrap", ParentMenuId = 1, MenuName = "Privileges", UniqueName = "privileges", Icon = "fingerprint", MenuLink = "/AdminConsole/privilegemanage", MenuIndex = 4, IsActive = true, IsDeleted = false },
                new Menu { MenuId = 6, AppId = "WebScrap", ParentMenuId = 1, MenuName = "Master", UniqueName = "masters", Icon = "thumb_up", MenuLink = "/master", MenuIndex = 5, IsActive = true, IsDeleted = false }
            );
        });

        _ = modelBuilder.Entity<UserApplication>(a =>
        {
            //a.IsMemoryOptimized(true);
            _ = a.HasKey(x => new { x.UserId, x.AppId, x.RoleId });

            _ = a.HasOne(p => p.User)
                .WithMany(t => t.UserApplications)
                .HasForeignKey(e => e.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
            _ = a.HasOne(p => p.Application)
                .WithMany(t => t.UserApplications)
                .HasForeignKey(e => e.AppId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
            _ = a.HasOne(p => p.AppRole)
                .WithMany(t => t.UserApplications)
                .HasForeignKey(e => e.RoleId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        _ = modelBuilder.Entity<GroupMenu>(a =>
        {
            //a.IsMemoryOptimized(true);
            _ = a.HasKey(x => new { x.RoleId, x.MenuId });

            _ = a.HasOne(p => p.AppRole)
                .WithMany(t => t.GroupMenus)
                .HasForeignKey(e => e.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);
            _ = a.HasOne(p => p.Menu)
                .WithMany(t => t.GroupMenus)
                .HasForeignKey(e => e.MenuId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);
        });;

        _ = modelBuilder.Entity<NotifData>(a =>
        {
            _ = a.HasOne(p => p.AppUser)
                .WithMany(t => t.NotifDatas)
                .HasForeignKey(e => e.AppUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        _ = modelBuilder.Entity<Transaction>(a =>
        {
            _ = a.HasOne(p => p.AppUser)
                .WithMany(t => t.Transactions)
                .HasForeignKey(e => e.AppUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            _ = a.HasMany(e => e.Product)
                .WithMany(e => e.Transactions)
                .UsingEntity<ProductTansaction>();
        });

        base.OnModelCreating(modelBuilder);
    }

}
