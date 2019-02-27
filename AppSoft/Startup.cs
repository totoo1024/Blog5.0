using System;
using System.Linq;
using System.Reflection;
using App.Common.Cache;
using App.Common.Extensions;
using App.Core;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;

namespace AppSoft
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;//关闭GDPR规范
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDistributedMemoryCache();
            //添加Session 服务
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;

            });
            //注册全局异常过滤器
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add<ExceptionGlobalFilterAttribute>();
            //});
            services.AddHttpContextHelperAccessor();
            //解决序列化json时字段全部变为小写模式
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //防止汉字被自动编码
            //services.Configure<WebEncoderOptions>(options =>
            //{
            //    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            //});
            //设置认证cookie名称、过期时间、是否允许客户端读取
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = "appsoft";//cookie名称
                options.Cookie.Expiration = TimeSpan.FromHours(1);//过期时间
                options.Cookie.HttpOnly = true;//不允许客户端获取
                options.SlidingExpiration = true;// 是否在过期时间过半的时候，自动延期
            });

            Redis.Initialization();

            #region Autofac注入
            //实例化一个autofac的创建容器
            var builder = new ContainerBuilder();
            //架注册业务逻辑层所在程序集中的所有类的对象实例
            builder.RegisterAssemblyTypes(Assembly.Load("App.Services")).Where(t => t.Name.EndsWith("Logic")).AsImplementedInterfaces();
            //注册数据仓储层所在程序集中的所有类的对象实例
            builder.RegisterAssemblyTypes(Assembly.Load("App.Repository")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();
            builder.Populate(services);
            //创建一个Autofac的容器
            var container = builder.Build();
            //第三方IOC接管 core内置DI容
            return container.Resolve<IServiceProvider>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //配置nlog
            loggerFactory.AddNLog();
            env.ConfigureNLog("Configs/nlog.config");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //HttpContext扩展
            app.UseHttpContextHelper();

            //登录授权cookie
            app.UseAuthentication();

            #region 解决Ubuntu Nginx 代理不能获取IP问题
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            #endregion

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "areas",
                template: "{area:exists}/{controller=Login}/{action=Index}/{id?}"
              );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");


            });
        }

    }
}
