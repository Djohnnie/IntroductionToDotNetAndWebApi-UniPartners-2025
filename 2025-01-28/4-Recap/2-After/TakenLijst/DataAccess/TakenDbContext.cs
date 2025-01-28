using Microsoft.EntityFrameworkCore;
using Takenlijst.Models;

namespace Takenlijst.DataAccess;

public class TakenDbContext : DbContext
{
    public DbSet<Taak> Taken { get; set; }

    public TakenDbContext(DbContextOptions<TakenDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Taak>(x =>
        {
            x.HasKey(t => t.Code).IsClustered(false);
            x.Property(t => t.Volgnummer).ValueGeneratedOnAdd();
            x.HasIndex(t => t.Volgnummer).IsUnique().IsClustered();
        });
    }
}