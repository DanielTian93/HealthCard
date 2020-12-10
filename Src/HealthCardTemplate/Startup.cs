using AutoMapper;
using Core;
using Core.Extensions;
using Core.Extensions.DI;
using Core.Filter;
using Core.Helpers;
using Core.UnifiedResponseMessage;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NLog;
using Senparc.CO2NET;
using Senparc.CO2NET.AspNet;
using Senparc.Weixin.RegisterServices;
using System;

namespace HealthCardTemplate
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
            services.AddDbContext<EFContext>(options => options.UseMySql(ConfigurationManager.AppSettings["HCMysqlConnectionString"]));
            #region 跨域
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            #endregion

            var build = services.AddControllers();

            build.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamCaseContractResolver();
                //修改时间的序列化方式
                //options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy/MM/dd HH:mm:ss" });
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            //返回值JSON序列化
            //build.AddWebApiConventions();
            //注册全局过滤器
            build.AddMvcOptions(t =>
            {
                t.Filters.Add(new AuthAttribute());
                t.Filters.Add(typeof(GlobalExceptionsFilter));
            });

            #region DI注入
            //两种方式实现DI注册
            //1.注册业务DI服务（接口注册实现在同一命名空间下）
            services.RegisterInterface();
            //2.特性注册,不限定命名空间
            services.RegisterAttribute();
            #endregion

            //过滤器使用

            //利用dapper自动把下划线转换成驼峰格式
            DefaultTypeMap.MatchNamesWithUnderscores = true;


            //使用AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // 注入的实现SwaggerProvider使用默认设置
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSenparcWeixinServices(Configuration);
#if DEBUG

            #region swagger
            services.AddSwaggerGen();
            var XmlComments = XmlCommentsFilePathHelper.XmlCommentsFilePath();
            services.ConfigureSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "HealthCard API", Version = "v1" });

                //加载需要展示swagger的xml
                foreach (string x in XmlComments)
                {
                    options.IncludeXmlComments(x);
                }
                //方法上加[Obsolete] 
                options.IgnoreObsoleteActions();
                //属性上加[Obsolete]  在swagger文档上就不会生成对应的属性或方法
                options.IgnoreObsoleteProperties();
                //swagger添加token Header
                options.OperationFilter<SwaggerAddToken>();
            });
            #endregion
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IOptions<SenparcSetting> senparcSetting)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            LogManager.LoadConfiguration("nlog.config"); //填入上面创建的文件的名称
            app.UseSenparcGlobal(env, senparcSetting.Value, default);

#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;

            });
#endif
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("any");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
