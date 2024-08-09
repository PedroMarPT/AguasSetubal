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
                options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Adiciona suporte para controllers e views
            services.AddControllersWithViews();

            // Adiciona suporte para Razor Pages
            services.AddRazorPages();
        }

        // Este método é chamado pelo runtime. Use este método para configurar o pipeline de requisições HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        }
    }
}

