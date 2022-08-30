using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using ISPRO.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ISPRO.Persistence.Enums;
using ISPRO.Helpers;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ISPRO.Helpers.Exceptions;

namespace ISPRO.Persistence.Context
{
    public class DataContext : DbContext, IDisposable
    {
        //public static string ConnectionString { get; set; } = "Server=tcp:ispro-dbserver.database.windows.net,1433;Initial Catalog=ISPRO-DB;Persist Security Info=False;User ID=superuser;Password=Warning@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //public delegate string StringAction();
        public static string ApplicationDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;

        public DbSet<AdminAccount> AdminAccounts { get; set; }
        public DbSet<ManagerAccount> ManagerAccounts { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<PrePaidCard> PrePaidCards { get; set; }
        public DbSet<CashPayment> CashPayments { get; set; }

        // Required for the client api
        public DataContext(DbContextOptions options) : base(options)
        {
            Console.WriteLine("Starting migration...");
            //Database.EnsureCreated();
            //Database.Migrate();
            //Database.OpenConnection();
            //EnsureBaseConfigExist();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ManagerAccount>().HasMany(m => m.Projects).WithOne(p => p.ProjectManager)
                .HasForeignKey(p => p.ProjectManagerUsername)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAccount>().HasMany(u => u.PrePaidCards).WithOne(p => p.Consumer)
                .HasForeignKey(p => p.ConsumerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAccount>().HasMany(u => u.CashPayments).WithOne(p => p.UserAccount)
                .HasForeignKey(p => p.UserAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>().HasMany(p => p.UserAccounts).WithOne(u => u.Project)
                .HasForeignKey(u => u.ProjectName)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>().HasMany(p => p.Subscriptions).WithOne(s => s.Project)
                .HasForeignKey(s => s.ProjectName)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subscription>().HasMany(s => s.Subscribers).WithOne(u => u.Subscription)
                .HasForeignKey(u => u.SubscriptionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Subscription>().HasMany(s => s.PrePaidCards).WithOne(p => p.Subscription)
                .HasForeignKey(p => p.SubscriptionId)
                .OnDelete(DeleteBehavior.NoAction);

            var CurrencyConverter = new EnumToStringConverter<Currency>();
            var UserTypeConverter = new EnumToStringConverter<UserType>();

            modelBuilder
                .Entity<PrePaidCard>()
                .Property(e => e.Currency)
                .HasConversion(CurrencyConverter);

            modelBuilder
                .Entity<AdminAccount>()
                .Property(e => e.UserType)
                .HasConversion(UserTypeConverter);

            modelBuilder
                .Entity<ManagerAccount>()
                .Property(e => e.UserType)
                .HasConversion(UserTypeConverter);

            modelBuilder
                .Entity<UserAccount>()
                .Property(e => e.UserType)
                .HasConversion(UserTypeConverter);

            base.OnModelCreating(modelBuilder);
        }

        public IQueryable Set(Type T)
        {
            // Get the generic type definition
            MethodInfo method = typeof(DbContext).GetMethod(nameof(DbContext.Set), BindingFlags.Public | BindingFlags.Instance);

            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod(T);

            return method.Invoke(this, null) as IQueryable;
        }

        private void EnsureBaseConfigExist()
        {
            if (!this.AdminAccounts.Any()) this.Add(new AdminAccount()
            {
                Username = "admin@admins.com",
                DisplayName = "Administrator",
                ExpiryDate = DateTime.MaxValue,
                Password = CryptoHelper.ComputeSHA256Hash("Warning@1234")
            });

            this.SaveChanges();
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            if (typeof(TEntity).IsSubclassOf(typeof(AbstractUser)))
            {
                ((AbstractUser)(object)entity).Password = CryptoHelper.ComputeSHA256Hash("123456789");
            }
            Validate(entity);
            ((BaseEntity)(object)entity).CreationDate = DateTime.Now;
            return base.Add(entity);
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            Validate(entity);
            ((BaseEntity)(object)entity).LastUpdate = DateTime.Now;
            return base.Update(entity);
        }

        private void Validate<TEntity>(TEntity entity)
        {
            if (typeof(TEntity).IsSubclassOf(typeof(AbstractUser)))
            {
                ValidateUsername((AbstractUser)(object)entity);
            }
        }

        private void ValidateUsername(AbstractUser user)
        {
            var domain = string.Empty;
            if (user.GetType() == typeof(AdminAccount)) { 
                domain = "admins.com";
                user.UserType = UserType.ADMIN;
            }
            else if (user.GetType() == typeof(ManagerAccount)) { 
                domain = "managers.com";
                user.UserType = UserType.MANAGER;
            }
            else if (user.GetType() == typeof(UserAccount)) { 
                domain = "users.com";
                user.UserType = UserType.USER_ACCOUNT;
            }

            Regex r = new Regex("^[a-zA-Z0-9_]*$");
            var username = user.Username.Trim();
            if (!user.Username.Contains('@'))
                username += '@' + domain;

            var name_domain = username.Split('@');
            if (name_domain.Length != 2 || !name_domain[1].Equals(domain, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ModelException("Username", "Username has invalid domain.");
            }
            if (!r.IsMatch(name_domain[0]))
            {
                throw new ModelException("Username", "Username accepts only alpha-numeric and '_' characters.");
            }

            if (string.IsNullOrWhiteSpace(user.DisplayName))
            {
                user.DisplayName = user.Username.Split('@')[0];
            }
            user.Username = username.ToLower();
        }

    }
}
