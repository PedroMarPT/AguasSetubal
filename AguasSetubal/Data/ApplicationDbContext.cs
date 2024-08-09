using Microsoft.EntityFrameworkCore;
using AguasSetubal.Models; // Supondo que suas entidades estão na pasta Models

namespace AguasSetubal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets representam as tabelas do banco de dados
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fatura> Faturas { get; set; }
        public DbSet<LeituraContador> LeiturasContadores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configurações adicionais, como mapeamento de chaves compostas, índices, etc.
        }
    }
}

