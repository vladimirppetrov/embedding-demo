using Microsoft.EntityFrameworkCore;
using MSSqlServerDB.Models;

namespace MSSqlServerDB;

public class EmbedingsDbContext : DbContext
{
    public EmbedingsDbContext(DbContextOptions<EmbedingsDbContext> options) : base(options)
    {
    }
    public DbSet<Embedding> Embeddings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}