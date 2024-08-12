using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AguasSetubal.Models; // Supondo que suas entidades estão na pasta Model


namespace AguasSetubal.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets representam as tabelas do banco de dados
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fatura> Faturas { get; set; }
        public DbSet<LeituraContador> LeiturasContadores { get; set; }
        public DbSet<LeituraContador> LeituraContadores { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
            .Property(c => c.LeituraAtualContador)
            .HasColumnType("decimal(18, 2)");  // Ajuste a precisão conforme necessário

            modelBuilder.Entity<Fatura>()
                .Property(f => f.ValorTotal)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<LeituraContador>()
                .Property(l => l.ValorPagar)
                .HasColumnType("decimal(18, 2)");

            base.OnModelCreating(modelBuilder);
            // Configurações adicionais, como mapeamento de chaves compostas, índices, etc.
        }
    }
}


