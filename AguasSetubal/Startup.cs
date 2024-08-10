using AguasSetubal.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace AguasSetubal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método é chamado pelo runtime. Use este método para adicionar serviços ao contêiner.
        public void ConfigureServices(IServiceCollection services)
        {
            // Registro do DbContext com a string de conexão
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            // Filtro de exceções para a página de desenvolvedor de banco de dados
            services.AddDatabaseDeveloperPageExceptionFilter();

            // Configuração do Identity para autenticação e autorização
            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.Password.RequireDigit = true; // Requer um número
                options.Password.RequireLowercase = true; // Requer uma letra minúscula
                options.Password.RequireNonAlphanumeric = false; // Não requer caractere especial
                options.Password.RequireUppercase = true; // Requer uma letra maiúscula
                options.Password.RequiredLength = 6; // Mínimo de 6 caracteres
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Adiciona suporte para controllers e views
            services.AddControllersWithViews();

            // Adiciona suporte para Razor Pages
            services.AddRazorPages();
        }

        // Este método é chamado pelo runtime. Use este método para configurar o pipeline de requisições HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts(); // Força o uso de HTTPS em produção
            }

            app.UseHttpsRedirection(); // Redireciona HTTP para HTTPS
            app.UseStaticFiles(); // Habilita o uso de arquivos estáticos como CSS e JS

            app.UseRouting(); // Configura o roteamento

            app.UseAuthentication(); // Habilita a autenticação
            app.UseAuthorization(); // Habilita a autorização

            // Configura as rotas dos endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages(); // Mapeia as Razor Pages
            });

            // Chama o método para criar as roles e um utilizador admin por defeito
            CreateRoles(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            // Obtenção dos serviços necessários
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Definindo os nomes das roles
            string[] roleNames = { "Admin", "Funcionario", "Cliente", "Anonimo" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                // Verifica se a role já existe, e se não, cria ela
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Criação de um utilizador Admin por defeito
            var powerUser = new IdentityUser
            {
                UserName = "admin@exemplo.com",
                Email = "admin@exemplo.com",
            };

            string adminPassword = "Admin@123";
            var user = await userManager.FindByEmailAsync("admin@exemplo.com");

            if (user == null)
            {
                var createPowerUser = await userManager.CreateAsync(powerUser, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    // Atribui a role de Admin ao utilizador criado
                    await userManager.AddToRoleAsync(powerUser, "Admin");
                }
            }
        }
    }
}


