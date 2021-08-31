using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareDemo
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MiddlewareDemo", Version = "v1" });
            });
        }

        private void HandleBranchAndRejoin(IApplicationBuilder app, ILogger<Startup> logger)
        {
            app.Use(async (context, next) =>
            {
                var branchVer = context.Request.Query["branch"];
                logger.LogInformation("Branch used = {branchVer}", branchVer);

                // Do work that doesn't write to the Response.
                await next();
                // Do other work that doesn't write to the Response.
            });
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            app.UseWhen(context => context.Request.Query.ContainsKey("branch"),
                                   appBuilder => HandleBranchAndRejoin(appBuilder, logger));

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello from main pipeline.");
            });
        }

        //private static void HandleBranch(IApplicationBuilder app)
        //{
        //    app.Run(async context =>
        //    {
        //        var branchVer = context.Request.Query["branch"];
        //        await context.Response.WriteAsync($"Branch used = {branchVer}");
        //    });
        //}

        //public void Configure(IApplicationBuilder app)
        //{
        //    app.MapWhen(context => context.Request.Query.ContainsKey("branch"),
        //                           HandleBranch);

        //    app.Run(async context =>
        //    {
        //        await context.Response.WriteAsync("Hello from non-Map delegate. <p>");
        //    });
        //}

        //private static void HandleMultiSeg(IApplicationBuilder app)
        //{
        //    app.Run(async context =>
        //    {
        //        await context.Response.WriteAsync("Map multiple segments.");
        //    });
        //}

        //public void Configure(IApplicationBuilder app)
        //{
        //    app.Map("/map1/seg1", HandleMultiSeg);

        //    app.Run(async context =>
        //    {
        //        await context.Response.WriteAsync("Hello from non-Map delegate.");
        //    });
        //}

        //private static void HandleMapTest1(IApplicationBuilder app)
        //{
        //    app.Run(async context =>
        //    {
        //        await context.Response.WriteAsync("Map Test 1");
        //    });
        //}

        //private static void HandleMapTest2(IApplicationBuilder app)
        //{
        //    app.Run(async context =>
        //    {
        //        await context.Response.WriteAsync("Map Test 2");
        //    });
        //}

        //public void Configure(IApplicationBuilder app)
        //{
        //    app.Map("/map1", HandleMapTest1);

        //    app.Map("/map2", HandleMapTest2);

        //    app.Run(async context =>
        //    {
        //        await context.Response.WriteAsync("Hello from non-Map delegate. <p>");
        //    });
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app/*, IWebHostEnvironment env*/)
        //{
        //    //app.Use(async (context, next) =>
        //    //{
        //    //    // Do work that doesn't write to the Response.
        //    //    await next.Invoke();
        //    //    // Do logging or other work that doesn't write to the Response.
        //    //});

        //    //app.Run(async context =>
        //    //{
        //    //    await context.Response.WriteAsync("Hello from 2nd delegate.");
        //    //});


        //    //if (env.IsDevelopment())
        //    //{
        //    //    app.UseDeveloperExceptionPage();
        //    //    app.UseSwagger();
        //    //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiddlewareDemo v1"));
        //    //}

        //    //app.UseHttpsRedirection();

        //    //app.UseRouting();

        //    //app.UseAuthorization();

        //    //app.UseEndpoints(endpoints =>
        //    //{
        //    //    endpoints.MapControllers();
        //    //});
        //}
    }
}
