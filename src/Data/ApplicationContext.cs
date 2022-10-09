using Microsoft.EntityFrameworkCore;

using dog_api.Models.Infra; // FIXME

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options) : base(options) { }
    public DbSet<DogImage> DogImages => Set<DogImage>();
}