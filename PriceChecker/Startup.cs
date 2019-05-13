using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PriceChecker.DbProducts;
using PriceChecker.ProductUpdater;
using PriceChecker.Hubs;


namespace PriceChecker
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public DbProductContext ProductContext { get; }

        public Startup(IConfiguration configuration)
        {
            DbProductContext.GetInstance();
            //Updater updater = new Updater(ProductContext);
            //updater.StartUpdatingByTimer();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseFileServer();

            app.UseSignalR(routes =>
            {
                routes.MapHub<PricesHub>("/priceChecker");
            });
        }
    }
}
