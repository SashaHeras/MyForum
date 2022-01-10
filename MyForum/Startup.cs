using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyForum.Core.Interfaces.Infrastructure;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Models;
using MyForum.Data.Repository.Repositories;
using MyForum.Extensions;

namespace MyForum
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private EnvironmentDetails environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.DisableCachingForAllHttpCalls();
            environment = services.ConfigurePoco<EnvironmentDetails>(Configuration.GetSection("Environment"));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
  
            services.AddAntiforgery(opts => opts.Cookie.Name = "AntiForgery." + environment.ApplicationName);
            services.AddHttpClient();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IMarkRepository, MarkRepository>();
            services.AddSingleton<IPostRepository, PostRepository>();
            services.AddSingleton<ITopicRepository, TopicRepository>();
            services.AddSingleton<ICommentRepository, CommentRepository>();

            services.AddHttpContextAccessor();
            
            services.AddLongRunningTasks();

            services.AddControllers();

            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = true;
            });
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.AddDistributedMemoryCache();
            services.AddSession();
            
            services.AddDbContext<MyForumContext>(options => 
                options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                sqlServerDbContextOptionsBuilder => 
                    sqlServerDbContextOptionsBuilder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

            services.AddControllersWithViews();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAllServices();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowSpecificOrigin");
            app.UseResponseCaching();
            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {                
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Login}");
            });
        }
    }
}
