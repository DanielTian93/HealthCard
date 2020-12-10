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
            #region ����
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
                //�޸�ʱ������л���ʽ
                //options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy/MM/dd HH:mm:ss" });
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            //����ֵJSON���л�
            //build.AddWebApiConventions();
            //ע��ȫ�ֹ�����
            build.AddMvcOptions(t =>
            {
                t.Filters.Add(new AuthAttribute());
                t.Filters.Add(typeof(GlobalExceptionsFilter));
            });

            #region DIע��
            //���ַ�ʽʵ��DIע��
            //1.ע��ҵ��DI���񣨽ӿ�ע��ʵ����ͬһ�����ռ��£�
            services.RegisterInterface();
            //2.����ע��,���޶������ռ�
            services.RegisterAttribute();
            #endregion

            //������ʹ��

            //����dapper�Զ����»���ת�����շ��ʽ
            DefaultTypeMap.MatchNamesWithUnderscores = true;


            //ʹ��AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // ע���ʵ��SwaggerProviderʹ��Ĭ������
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSenparcWeixinServices(Configuration);
#if DEBUG

            #region swagger
            services.AddSwaggerGen();
            var XmlComments = XmlCommentsFilePathHelper.XmlCommentsFilePath();
            services.ConfigureSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "HealthCard API", Version = "v1" });

                //������Ҫչʾswagger��xml
                foreach (string x in XmlComments)
                {
                    options.IncludeXmlComments(x);
                }
                //�����ϼ�[Obsolete] 
                options.IgnoreObsoleteActions();
                //�����ϼ�[Obsolete]  ��swagger�ĵ��ϾͲ������ɶ�Ӧ�����Ի򷽷�
                options.IgnoreObsoleteProperties();
                //swagger���token Header
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
            LogManager.LoadConfiguration("nlog.config"); //�������洴�����ļ�������
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
