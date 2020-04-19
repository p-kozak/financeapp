using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalFinance.Data;
using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain;
using PersonalFinance.Services;

namespace PersonalFinance
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
                //IMPORTANT
                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<FinanceContext>();

            services.AddDbContextPool<FinanceContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("FinanceDb"));
            });
            services.AddControllers();
            services.AddRazorPages(options =>
            {
                //By default require authorization eveywhere
                options.Conventions.AuthorizeFolder("/");
                options.Conventions.AllowAnonymousToPage("/Index");
                options.Conventions.AllowAnonymousToPage("/Error");
                options.Conventions.AllowAnonymousToPage("/Privacy");
                options.Conventions.AllowAnonymousToPage("/Users/Login");
                options.Conventions.AllowAnonymousToPage("/Users/Register");



            });

            services.ConfigureApplicationCookie(o => o.LoginPath = "/Users/Login");
            services.AddScoped<IPersonalFinanceRepository, PersonalFinanceRepository>();
            services.AddScoped<ICurrencyConverter, CurrencyConverter>();
            services.AddAutoMapper(typeof(Startup));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();


            //Authentication goes first
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller}/{action}/{id?}");
            });
        }
    }
}
