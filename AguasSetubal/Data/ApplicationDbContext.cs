using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AguasSetubal.Models; // Certifique-se de que o namespace está correto

namespace AguasSetubal.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fatura> Faturas { get; set; }
        public DbSet<LeituraContador> LeituraContadores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração específica para a entidade Cliente
            modelBuilder.Entity<Cliente>()
                .Property(c => c.LeituraAtualContador)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();  // Campo obrigatório

            // Configuração específica para a entidade Fatura
            modelBuilder.Entity<Fatura>()
                .Property(f => f.ValorTotal)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();  // Campo obrigatório

            // Configuração específica para a entidade LeituraContador
            modelBuilder.Entity<LeituraContador>()
                .Property(l => l.ValorPagar)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();  // Campo obrigatório

            // Configurar relacionamento entre Fatura e LeituraContador
            modelBuilder.Entity<Fatura>()
                .HasOne(f => f.LeituraContador)
                .WithOne(l => l.Fatura)
                .HasForeignKey<Fatura>(f => f.LeituraContadorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Certifique-se de chamar a base
            base.OnModelCreating(modelBuilder);
        }
    }
}


