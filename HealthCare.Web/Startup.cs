using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HealthCare.Web.Services;
using HealthCare.Data.Models;
using HealthCare.Data;
using HealthCare.Data.Services;
using HealthCare.Data.Interfaces;

namespace HealthCare.Web
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
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ApplicationDbContext>(b => b.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            Constants.ConnectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsService, SmsService>();

            services.AddTransient<IMemberRepository, MemberRepository>();
            services.AddTransient<IAccountHeadTypeRepository, AccountHeadTypeRepository>();
            services.AddTransient<IAccountHeadRepository, AccountHeadRepository>();
            services.AddTransient<IAccountHeadHistoryRepository, AccountHeadHistoryRepository>();
            services.AddTransient<IPatientTestRepository, PatientTestService>();
            services.AddTransient<IMedicalPaymentRepository, MedicalPaymentService>();
            services.AddTransient<IAppointmentRepository, AppointmentService>();

            services.AddMvc();

            // Register application services.
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=account}/{action=login}/{id?}");
            });
        }
    }
}
