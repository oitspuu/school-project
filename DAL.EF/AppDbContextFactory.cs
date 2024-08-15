using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.EF;

public class AppDbContextFactory :  IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        //Did not update db without connection string
        optionsBuilder.UseNpgsql("Host=localhost;Port=7890;Database=webapp;Username=postgres;Password=postgres");

        return new AppDbContext(optionsBuilder.Options);
    }
}