using Microsoft.EntityFrameworkCore;

namespace EventCorp.Models
{
    public class EventCorpContext : DbContext
    {
        public EventCorpContext(DbContextOptions options) : base(options) { }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Evento> Eventos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Evento>()
                .HasOne(c => c.Categoria)
                .WithMany(e => e.Eventos)
                .HasForeignKey(e => e.CategoriaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}