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
using Syncfusion.XlsIO.Parser.Biff_Records;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AguasSetubal.Helpers;
using AguasSetubal.Models;
using Microsoft.IdentityModel.Logging;
using System.Linq;

namespace AguasSetubal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                cfg.SignIn.RequireConfirmedEmail = true;
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredLength = 6;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = this.Configuration["Tokens:Issuer"],
                        ValidAudience = this.Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"])),
                    };
                });

            services.AddDbContext<ApplicationDbContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddHttpClient();

            services.AddTransient<SeedDb>();
            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<IClientsRepository, ClientsRepository>();
            services.AddScoped<IInvoicesRepository, InvoicesRepository>();
            services.AddScoped<IPricesRepository, PricesRepository>();
            services.AddScoped<IMailHelper, MailHelper>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/NotAuthorized";
                options.AccessDeniedPath = "/Account/NotAuthorized";
            });

            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IUserHelper userHelper,
            IClientsRepository clientsRepository, IPricesRepository pricesRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();

            }
            else
            {
                app.UseExceptionHandler("/Errors/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Chama o método para criar as roles e um utilizador admin por defeito
            CreateUsersAndRoles(userHelper).Wait();

            // Chama o método para criar clientes
            CreateClients(clientsRepository).Wait();

            // Chama o método para criar tabela preços inicial
            CreatePrices(pricesRepository).Wait();
        }
        // Método para criar clientes tabela preços inicial
        private async Task CreatePrices(IPricesRepository pricesRepository)
        {
            var _pricesRepository = pricesRepository;
            var allPrices = _pricesRepository.GetAll();
            if (!allPrices.Any())
            {
                var priceLine = new TabelaPrecos
                {
                    NomeEscalao = "1º escalão até 5 m3",
                    LimiteInferior = 0,
                    LimiteSuperior = 5,
                    ValorUnitario = 0.30M
                };
                await _pricesRepository.CreateAsync(priceLine);

                priceLine = new TabelaPrecos
                {
                    NomeEscalao = "2º escalão superior a 5 m3 e até 15 m3",
                    LimiteInferior = 5,
                    LimiteSuperior = 15,
                    ValorUnitario = 0.80M
                };
                await _pricesRepository.CreateAsync(priceLine);

                priceLine = new TabelaPrecos
                {
                    NomeEscalao = "3º escalão superior a 15 cm3 e até 25 m3",
                    LimiteInferior = 15,
                    LimiteSuperior = 25,
                    ValorUnitario = 1.20M
                };
                await _pricesRepository.CreateAsync(priceLine);

                priceLine = new TabelaPrecos
                {
                    NomeEscalao = "4º escalão superior a 25 m3",
                    LimiteInferior = 25,
                    LimiteSuperior = 0,
                    ValorUnitario = 1.60M
                };
                await _pricesRepository.CreateAsync(priceLine);
            }
        }
        // Método para criar clientes
        private async Task CreateClients(IClientsRepository clientsRepository)
        {
            var _clientsRepository = clientsRepository;

            var allClients = _clientsRepository.GetAll();
            if (!allClients.Any())
            {
                var client = new Cliente
                {
                    Nome = "Teresa Silva",
                    NIF = "234345678",
                    MoradaFaturacao = "Rua das rosas, 33",
                    ContactoTelefonico = "935142121",
                    Email = "teresa.silva@gmail.com",
                    NumeroCartaoCidadao = "125252522"

                };
                await _clientsRepository.CreateAsync(client);

                client = new Cliente
                {
                    Nome = "Santos & Santos, Lda",
                    NIF = "500125125",
                    MoradaFaturacao = "Rua das lagoas, 133 armazém A",
                    ContactoTelefonico = "213456786",
                    Email = "santos_santos@gmail.com",
                    NumeroCartaoCidadao = ""

                };
                await _clientsRepository.CreateAsync(client);
            }
        }
        // Método para criar roles e usuários padrão
        private async Task CreateUsersAndRoles(IUserHelper userHelper)
        {
            var _userHelper = userHelper;

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Funcionario");
            await _userHelper.CheckRoleAsync("Cliente");

            //admin
            var user = await _userHelper.GetUserByEmailAsync("pedro@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Pedro",
                    LastName = "Marques",
                    Email = "pedro@gmail.com",
                    UserName = "pedro@gmail.com",
                    PhoneNumber = "212673409",
                    Address = "Rua de Setúbal, 35"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Couldn't create user in seeder");
                }
                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
                await _userHelper.AddUserToRoleAsync(user, "Admin");


            //Funcionario
            user = await _userHelper.GetUserByEmailAsync("funcionario@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Funcionário",
                    LastName = "Marques",
                    Email = "funcionario@gmail.com",
                    UserName = "funcionario@gmail.com",
                    PhoneNumber = "212673409",
                    Address = "Rua de Setúbal, 44"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Couldn't create user in seeder");
                }
                await _userHelper.AddUserToRoleAsync(user, "Funcionario");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            isInRole = await _userHelper.IsUserInRoleAsync(user, "Funcionario");
            if (!isInRole)
                await _userHelper.AddUserToRoleAsync(user, "Funcionario");

            //Cliente
            user = await _userHelper.GetUserByEmailAsync("cliente@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Cliente",
                    LastName = "Marques",
                    Email = "cliente@gmail.com",
                    UserName = "cliente@gmail.com",
                    PhoneNumber = "212673409",
                    Address = "Rua de Setúbal, 55"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Couldn't create user in seeder");
                }
                await _userHelper.AddUserToRoleAsync(user, "Cliente");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            isInRole = await _userHelper.IsUserInRoleAsync(user, "Cliente");
            if (!isInRole)
                await _userHelper.AddUserToRoleAsync(user, "Cliente");
        }
    }    
}