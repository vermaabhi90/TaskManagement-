using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskUtility;
using System.Data.SqlClient;

namespace TaskManagement.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(string ConStr) : base(ConStr)
        {
            var connection = new SqlConnectionStringBuilder(ConStr);
            this.Database.Connection.ConnectionString = connection.ConnectionString;
        }
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>()
                .ToTable("tbl_SysUser", "dbo").Property(p => p.Id).HasColumnName("User_Id");
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("tbl_SysUser", "dbo").Property(p => p.Id).HasColumnName("User_Id");

            modelBuilder.Entity<IdentityRole>().ToTable("tbl_SysRole", "dbo").Property(p => p.Id).HasColumnName("RoleId");
            modelBuilder.Entity<IdentityRole>().ToTable("tbl_SysRole", "dbo").Property(p => p.Name).HasColumnName("RoleName");
            modelBuilder.Entity<IdentityUserRole>().ToTable("tbl_SysUserRole").Property(p => p.UserId).HasColumnName("User_Id"); ;
            modelBuilder.Entity<IdentityUserClaim>().ToTable("tbl_SysUserClaim").Property(p => p.Id).HasColumnName("ClaimId");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("tbl_SysUserClaim").Property(p => p.UserId).HasColumnName("User_Id");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("tbl_SysUserLogin").Property(p => p.UserId).HasColumnName("User_Id");

        }
        public static ApplicationDbContext Create(string ConStr)
        {
            Common.SQLCONNSTR = Common.SqlConnectionString;
            return new ApplicationDbContext(ConStr);
        }
        public static ApplicationDbContext Create()
        {
            Common.SQLCONNSTR = Common.SqlConnectionString;
            return new ApplicationDbContext(Common.SqlConnectionString);
        }
    }
}