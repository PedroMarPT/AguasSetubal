using AguasSetubal.Data;
using AguasSetubal.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace AguasSetubal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Método para adicionar serviços ao contêiner
        public void ConfigureServices(IServiceCollection services)
        {
            // Registro do DbContext com a string de conexão
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            // Filtro de exceções para a página de desenvolvedor de banco de dados
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddScoped<IRepository, Repository>();

            // Configuração do Identity para autenticação e autorização
            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews(options =>
            {
                // Require authenticated users globally
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });


            // Registrar o EmailSender do seu projeto
            services.AddTransient<AguasSetubal.Services.IEmailSender, AguasSetubal.Services.EmailSender>();

            // Adiciona suporte para controllers e views
            services.AddControllersWithViews();

            // Adiciona suporte para Razor Pages
            services.AddRazorPages();
        }

        // Método para configurar o pipeline de requisições HTTP
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Redireciona para a página de erro em produção
                app.UseStatusCodePagesWithReExecute("/Home/NotFound");
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

        // Método para criar roles e usuários padrão
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Definindo os nomes das roles
            string[] roleNames = { "Admin", "Funcionario", "Cliente", "Anonimo" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
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

            // Criação de um utilizador Funcionario por defeito
            var employeeUser = new IdentityUser
            {
                UserName = "funcionario@exemplo.com",
                Email = "funcionario@exemplo.com",
            };

            string employeePassword = "Funcionario@123";
            var employee = await userManager.FindByEmailAsync("funcionario@exemplo.com");

            if (employee == null)
            {
                var createEmployeeUser = await userManager.CreateAsync(employeeUser, employeePassword);
                if (createEmployeeUser.Succeeded)
                {
                    // Atribui a role de Funcionario ao utilizador criado
                    await userManager.AddToRoleAsync(employeeUser, "Funcionario");
                }
            }
        }
    }
}




