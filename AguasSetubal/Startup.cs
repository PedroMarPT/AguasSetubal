using AguasSetubal.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // Este m�todo � chamado pelo runtime. Use este m�todo para adicionar servi�os ao cont�iner.
        public void ConfigureServices(IServiceCollection services)
        {
            // Registro do DbContext com a string de conex�o
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            // Filtro de exce��es para a p�gina de desenvolvedor de banco de dados
            services.AddDatabaseDeveloperPageExceptionFilter();

            // Configura��o do Identity para autentica��o e autoriza��o
            services.AddDefaultIdentity<IdentityUser>(options =>
                options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()  // Adiciona suporte a roles (fun��es)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Adiciona suporte para controllers e views
            services.AddControllersWithViews();

            // Adiciona suporte para Razor Pages
            services.AddRazorPages();
        }

        // Este m�todo � chamado pelo runtime. Use este m�todo para configurar o pipeline de requisi��es HTTP.
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
                app.UseHsts(); // For�a o uso de HTTPS em produ��o
            }

            app.UseHttpsRedirection(); // Redireciona HTTP para HTTPS
            app.UseStaticFiles(); // Habilita o uso de arquivos est�ticos como CSS e JS

            app.UseRouting(); // Configura o roteamento

            app.UseAuthentication(); // Habilita a autentica��o
            app.UseAuthorization(); // Habilita a autoriza��o

            // Configura as rotas dos endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages(); // Mapeia as Razor Pages
            });

            // Chama o m�todo para criar as roles e um utilizador admin por defeito
            CreateRoles(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            // Obten��o dos servi�os necess�rios
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Definindo os nomes das roles
            string[] roleNames = { "Admin", "Funcionario", "Cliente", "Anonimo" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                // Verifica se a role j� existe, e se n�o, cria ela
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Cria��o de um utilizador Admin por defeito
            var powerUser = new IdentityUser
            {
                UserName = "admin@exemplo.com",
                Email = "admin@exemplo.com",
            };

            string userPassword = "Admin@123";
            var user = await userManager.FindByEmailAsync("admin@exemplo.com");

            if (user == null)
            {
                var createPowerUser = await userManager.CreateAsync(powerUser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    // Atribui a role de Admin ao utilizador criado
                    await userManager.AddToRoleAsync(powerUser, "Admin");
                }
            }
        }
    }
}


