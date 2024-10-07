using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AguasSetubal.Models; // Certifique-se de que o namespace está correto

namespace AguasSetubal.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fatura> Faturas { get; set; }
        public DbSet<LeituraContador> LeituraContadores { get; set; }
        public DbSet<TabelaPrecos> TabelaPrecos { get; set; }
        public DbSet<RequisicaoContador> RequisicaoContador { get; set; }
        public DbSet<Contador> Contador { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configuração específica para a entidade Fatura
            modelBuilder.Entity<Fatura>()
                .Property(f => f.ValorTotal)
                .HasPrecision(18, 2)
                .IsRequired();  // Campo obrigatório

            // Configuração específica para a entidade leitura contador
            modelBuilder.Entity<LeituraContador>()
                .Property(f => f.LeituraAnterior)
                .HasPrecision(18, 2)
                .IsRequired();  // Campo obrigatório

            modelBuilder.Entity<LeituraContador>()
                .Property(f => f.LeituraAtual)
                .HasPrecision(18, 2)
                .IsRequired();  // Campo obrigatório

            modelBuilder.Entity<LeituraContador>()
                .Property(f => f.Consumo)
                .HasPrecision(18, 2)
                .IsRequired();  // Campo obrigatório

            // Configuração específica para a entidade TabelaPrecos
            modelBuilder.Entity<TabelaPrecos>()
                .Property(f => f.ValorUnitario)
                .HasPrecision(18, 2)
                .IsRequired();  // Campo obrigatório

            // Configurar relacionamento entre Fatura e Varias leituras contador
            modelBuilder.Entity<Fatura>()
                .HasMany(c => c.LeiturasContador)
                .WithOne(e => e.Fatura)
                .HasForeignKey(bc => bc.FaturaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relacionamento entre Cliente e Varias Contadores
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Contadores)
                .WithOne(e => e.Cliente)
                .HasForeignKey(bc => bc.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relacionamento entre Contador e Varias leituras de contador
            modelBuilder.Entity<Contador>()
               .HasMany(c => c.LeiturasContador)
               .WithOne(e => e.Contador)
               .HasForeignKey(bc => bc.ContadorId)
               .OnDelete(DeleteBehavior.Restrict);

            // Certifique-se de chamar a base
            base.OnModelCreating(modelBuilder);
        }

    }
}