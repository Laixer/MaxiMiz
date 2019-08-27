using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maximiz.Model.Enums;
using Maximiz.Repositories;
using Maximiz.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Maximiz
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<ICampaignRepository, CampaignRepository>();
            services.AddTransient<IAdGroupRepository, AdGroupRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            // Map Enums to Type
            // TODO: Place somewhere else
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Status>("ad_item_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ApprovalState>("approval_state");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<BidStrategy>("bid_strategy");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<BudgetModel>("budget_model");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Connection>("connection");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Delivery>("delivery");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Device>("device");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<OS>("operating_system");

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //
            });
        }
    }
}
