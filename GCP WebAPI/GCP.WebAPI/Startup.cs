using GCP.Util;
using System;
using System.Text;
using System.IO;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using GCP.Util.Model;
using GCP.WebApi.Controllers;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using GCP.WebAPI.Filter;
using Microsoft.AspNetCore.Http;

namespace GCP.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            GlobalContext.LogWhenStart(env);
            GlobalContext.HostingEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo 
                {
                    Title = "GCProduct Api", 
                    Version = "v1",
                    Description = "高诚系统WebAPI文档",
                    Contact = new OpenApiContact
                    {
                        Name = "山东盛大高诚测控技术有限公司",
                        Email = string.Empty,
                        Url = new Uri("https://www.sdibm.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "没有许可证",
                        Url = new Uri("https://www.baidu.com")
                    }
                });
                config.OperationFilter<SwaggerDefaultValues>();
                config.IgnoreObsoleteProperties();

                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "SwaggerWebAPI.xml");
                config.IncludeXmlComments(xmlPath);

                // dto对应的描述xml文档路径地址
                if (GlobalContext.SystemConfig.Debug)
                {
                    var splitString = "GCP.WebAPI";
                    var dtoPath = basePath.Substring(0, basePath.IndexOf(splitString));
                    config.IncludeXmlComments(string.Format("{0}/SwaggerModel.xml", Path.Combine(dtoPath, "GCP.Model")));
                }
                else
                {
                    var xmlModelPath = Path.Combine(basePath, "SwaggerModel.xml");
                    config.IncludeXmlComments(xmlModelPath);
                }
            });

            //services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddOptions();
            services.AddCors();
            services.AddControllers(options =>
            {
                options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
            }).AddNewtonsoftJson(options =>
            {
                // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddMemoryCache();
            //services.AddSession();
            //services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  // 注册Encoding

            GlobalContext.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            GlobalContext.Services = services;
            GlobalContext.Configuration = Configuration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                GlobalContext.SystemConfig.Debug = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            string resource = Path.Combine(env.ContentRootPath, "Resource");
            FileHelper.CreateDirectory(resource);

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/Resource",
                FileProvider = new PhysicalFileProvider(resource),
                OnPrepareResponse = GlobalContext.SetCacheControl
            });

            app.UseMiddleware(typeof(GlobalExceptionMiddleware));

            app.UseCors(builder =>
            {
                builder.WithOrigins(GlobalContext.SystemConfig.AllowCorsSite.Split(',')).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            });
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-doc/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api-doc";
                c.SwaggerEndpoint("v1/swagger.json", "GCP Api v1");
            });
            //app.UseSession();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=ApiHome}/{action=Index}/{id?}");
            });
            GlobalContext.ServiceProvider = app.ApplicationServices;
            if (!GlobalContext.SystemConfig.Debug)
            {
                //new JobCenter().Start(); // 定时任务
            }
        }
    }
}
