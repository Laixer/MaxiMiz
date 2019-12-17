using Maximiz.ServiceBus;
using Maximiz.Repositories.Abstraction;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Maximiz.Database;
using Laixer.Library.Injection.ServiceBus;
using Laixer.Library.Injection.Database;
using Maximiz.Repositories.Mock;
using Maximiz.Storage.Abstraction;
using Maximiz.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Maximiz.Identity;
using Laixer.Identity.Dapper.Extensions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Maximiz.Controllers;
using Maximiz.Repositories;
using Maximiz.Core.StateMachine.Abstraction;
using Maximiz.Core.StateMachine;
using Maximiz.Operations;

namespace Maximiz
{

    /// <summary>
    /// Class to setup our configuration and dependency injections.
    /// </summary>
    public sealed class Startup
    {

        /// <summary>
        /// The injected configuration file.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor to inject configuration file.
        /// </summary>
        /// <param name="configuration">The configuration file</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Adds our used services to the host builder service collection. This
        /// sets up our dependency injection.
        /// </summary>
        /// <param name="services">The collection of DI services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Setup used azure services
            services.AddDatabaseProvider<DatabaseProvider, DatabaseProviderOptions>("DatabaseInternal");
            services.AddServiceBusSender<ServiceBusSender, ServiceBusProvider, ServiceBusProviderOptions>
                ("ServiceBusSend", "ServiceBusQueueName", Configuration);

            // Setup custom made services
            //services.AddTransactionHandler<TransactionHandler>();
            services.AddCrudInternalWebClient<CrudInternalWebClient>();
            services.AddMappers();
            services.AdddViewModelServices();
            // TODO Might want to revisit this
            services.AddTransient<IStorageManager, StorageManager>();


            // Setup logging
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            ConfigureServicesRepositories(services);
            ConfigureServicesIdentity(services);

            // Setup MVC structure
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                // options.Filters.Add(new AuthorizeFilter(policy)); TODO Re-enable!!!
            })
            .SetCompatibilityVersion(CompatibilityVersion.Latest);

            // TODO Remove / replace
            services.AddTransient<TestRepository>();
        }

        /// <summary>
        /// Configures our Identity Framework.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        private void ConfigureServicesIdentity(IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password = AppIdentityConstants.PasswordPolicy;
                options.Lockout = AppIdentityConstants.LockoutOptions;
                options.User.RequireUniqueEmail = true;
            })
            .AddDapperStores(options =>
            {
                options.UserTable = "user";
                options.Schema = "application";
                options.MatchWithUnderscore = true;
                options.UseNpgsql<CustomQueryRepository>(Configuration.GetConnectionString("DatabaseInternal"));
            })
            .AddDefaultTokenProviders();

            // TODO Does this work?
            services.Configure<CookieAuthenticationOptions>(options =>
            {
                options.LoginPath = new PathString($"/Login/{nameof(LoginController.Index)}");
                options.AccessDeniedPath = new PathString($"/Login/{nameof(LoginController.Index)}");
                options.LogoutPath = new PathString($"/Login/{nameof(LoginController.Index)}");
            });

            // For accessing our user
            services.AddHttpContextAccessor();
        }

        /// <summary>
        /// Configures our repositories.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        private void ConfigureServicesRepositories(IServiceCollection services)
        {
            // TODO Make scoped in stead of transient? Because of multiple queries & item preservation
            services.AddTransient<ICampaignRepository, MockCampaignRepository>();
            services.AddTransient<IAdGroupRepository, MockAdGroupRepository>();
            services.AddTransient<IAccountRepository, MockAccountRepository>();
            //services.AddTransient<IAdItemRepository, AdItemRepository>();
        }

        /// <summary>
        /// Sets up our http request pipeline.
        /// </summary>
        /// <param name="app">The application builder</param>
        /// <param name="env">The hosting environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Identity
            app.UseAuthentication();

            // Setup our mvc route default template
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    //template: "{controller=Login}/{action=Index}");
                    template: "{controller=CampaignGroupWizard}/{action=ShowWizard}");
                    //template: "{controller=Debug}/{action=Index}");
            });

        }
    }
}
