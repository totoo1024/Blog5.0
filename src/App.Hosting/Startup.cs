using System;
using System.Reflection;
using App.Core.Config;
using App.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using App.Framwork.DataValidation.Extensions;
using App.Framwork.DependencyInjection;
using App.Framwork.DependencyInjection.Extensions;
using App.Framwork.Generate.Geetest;
using App.Framwork.Mapper.Extensions;
using App.Hosting.Extensions;
using Autofac;
using EasyCaching.Core;
using EasyCaching.Core.Configurations;
using EasyCaching.CSRedis;
using EasyCaching.InMemory;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Hosting
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        string cacheProviderName = "default";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //更改视图实时生效
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;//关闭GDPR规范
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //添加Session 服务
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;

            });
            services.AddHttpContextAccessor();

            //自动注入配置
            services.AddConfig(Configuration);

            services.AddMvc().AddNewtonsoftJson(opt =>
            {
                //忽略循环引用
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                //不改变字段大小
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            services.AddControllersWithViews().AddValidation();//加入模型验证
            services.AddMapper();//自动模型映射
            //services.AddAutoDependencyInjection();//自动注入（和下方的 builder.RegisterModule<AutofacModule>(); 二选一即可）

            #region 缓存配置

            var sysConfig = services.BuildServiceProvider().GetService<SysConfig>();
            services.AddEasyCaching(options =>
            {
                ////使用文档 https://easycaching.readthedocs.io/en/latest
                if (sysConfig.UseRedis)
                {
                    options.UseCSRedis(Configuration);
                    cacheProviderName = EasyCachingConstValue.DefaultCSRedisName;
                }
                else
                {
                    cacheProviderName = EasyCachingConstValue.DefaultInMemoryName;
                    options.UseInMemory(Configuration);
                }
                options.WithJson(cacheProviderName);

            });

            #endregion

            #region 配置数据库连接
            //支持多数据连接
            services.AddSqlSugarConnection(Configuration);
            #endregion

            #region cookie全局设置

            //设置认证cookie名称、过期时间、是否允许客户端读取
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = "appsoft";//cookie名称
                options.Cookie.HttpOnly = true;//不允许客户端获取
                options.SlidingExpiration = true;// 是否在过期时间过半的时候，自动延期
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            });

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseSession();
            app.UseCookiePolicy();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //使用静态Autofac容器
            app.UseStaticContainer();

            //异常处理中间件
            app.UseExceptionHandle();

            #region 解决Ubuntu Nginx 代理不能获取IP问题
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            #endregion

            app.UseAuthentication();
            app.UseAuthorization();

            #region 路由配置

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            #endregion
        }

        /// <summary>
        /// Autofac容器配置
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //自动注入
            builder.RegisterModule<AutofacModule>();

            //自动注入service（业务层）
            builder.AutoRegisterService("service");

            //使用AspectCore加入动态代理（拦截器）注：切记Autofac.Extensions.DependencyInjection的nuget包必须是6.0版本，否则会出异常
            builder.AddAspectCoreInterceptor(x => x.CacheProviderName = cacheProviderName);
        }
    }
}
