using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZamaraWebApp.Models;

namespace ZamaraWebApp.Data
{
    public class AppDbContext :IdentityDbContext<UserViewModel>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<StaffViewModel> Staffs { get; set; }
    }
}
