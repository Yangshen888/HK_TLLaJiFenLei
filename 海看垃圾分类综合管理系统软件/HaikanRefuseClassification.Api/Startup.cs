using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using AutoMapper;

using HaikanRefuseClassification.Api.Auth;
using HaikanRefuseClassification.Api.Configurations;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Extensions.CustomException;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace HaikanRefuseClassification.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var cors = Configuration.GetSection("Cors").Get<string[]>();
            services.AddControllers();
            services.AddCors(o =>
    o.AddPolicy("cors",
        builder => builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()
            .AllowCredentials()
            .WithOrigins(cors)));

            services.AddMemoryCache();

            services.AddHttpContextAccessor();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppAuthenticationSettings>(appSettingsSection);
            services.Configure<MdDesEncrypt>(Configuration.GetSection("MdDesEncrypt"));
            services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(x => x.AllowSynchronousIO = true).Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);
            // JWT
            var appSettings = appSettingsSection.Get<AppAuthenticationSettings>();
            services.AddJwtBearerAuthentication(appSettings);

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = long.MaxValue);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<WebEncoderOptions>(options =>
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs)
            );
            //var serviceProvider = services.BuildServiceProvider();
            //ServiceLocator.SetServices(serviceProvider);

            services
                .AddMvc(config =>
                {
                    //config.Filters.Add(new ValidateModelAttribute());
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddXmlSerializerFormatters().AddXmlDataContractSerializerFormatters();

            services.AddDbContext<HaikanRefuseClassification.Api.Entities.RefuseClassificationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RBAC Management System API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // ×¢ÈëÈÕÖ¾
            services.AddLogging(config =>
            {
                config.AddLog4Net();
            });
            services.AddResponseCompression();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler("/error/500");
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseStaticFiles();

            app.UseFileServer();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("cors");
            app.UseAuthentication();
            app.UseAuthorization();

            app.ConfigureCustomExceptionMiddleware();

            var serviceProvider = app.ApplicationServices;
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            AuthContextService.Configure(httpContextAccessor);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            //app.UseSwagger(o =>
            //{
            //    o.PreSerializeFilters.Add((document, request) =>
            //    {
            //        document.Paths = (Microsoft.OpenApi.Models.OpenApiPaths)document.Paths.ToDictionary(p => p.Key.ToLowerInvariant(), p => p.Value);
            //    });
            //});

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RBAC API V1");
            });
        }
    }
}
