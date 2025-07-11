using CustomerRelationshipManagement.EntityFrameworkCore;
using CustomerRelationshipManagement.Upload;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nest;
using Polly;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace CustomerRelationshipManagement;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(CustomerRelationshipManagementApplicationModule),
    typeof(CustomerRelationshipManagementEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class CustomerRelationshipManagementHttpApiHostModule : AbpModule
{

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {

    }
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        Configure<AbpAntiForgeryOptions>(options =>
        {
            options.TokenCookie.Expiration = TimeSpan.FromDays(365);
            options.AutoValidate = false;
        });
        //添加主机环境
        context.Services.AddTransient<Upload.IHostingEnvironment, HostingEnvironmentAdapter>();
        //配置http上下文
        context.Services.AddHttpContextAccessor();
        // 注册 HttpClient（如果你还没加）
        context.Services.AddHttpClient();

        //获取配置信息
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureAuthentication(context);
        ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);
        ConfigureElasticSearch(context);
    }

    /// <summary>
    /// ElasticSearch 全文检索配置
    /// </summary>
    /// <param name="context"></param>
    private void ConfigureElasticSearch(ServiceConfigurationContext context)
    {        
        // 读取配置
        var configuration = context.Services.GetConfiguration();
        var uri = configuration["ElasticSearch:Uri"];
        
        // 创建连接设置
        var settings = new ConnectionSettings(new Uri(uri)).DefaultIndex(configuration["Elasticsearch:Index"]);

        // 创建 ElasticClient 并注册为单例
        var client = new ElasticClient(settings);
        context.Services.AddSingleton<IElasticClient>(client);
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        context.Services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                .AddJwtBearer(
                option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        //是否验证发行人
                        ValidateIssuer = true,
                        ValidIssuer = configuration["AuthServer:JwtBearer:Issuer"],//发行人

                        //是否验证受众人
                        ValidateAudience = true,
                        ValidAudience = configuration["AuthServer:JwtBearer:Audience"],//受众人

                        //是否验证密钥
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthServer:JwtBearer:SecurityKey"])),

                        ValidateLifetime = true, //验证生命周期

                        RequireExpirationTime = true, //过期时间

                        ClockSkew = TimeSpan.FromSeconds(30)   //平滑过期偏移时间
                    };

                    // ✅ 添加下面的调试代码
                    option.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("❌ JWT 验证失败：" + context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("✅ JWT 验证成功，用户：" + context.Principal?.Identity?.Name);
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            Console.WriteLine("📥 收到 Token：" + context.Token);
                            return Task.CompletedTask;
                        }
                    };
                }

            );
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());

            //options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            //options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<CustomerRelationshipManagementDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}CustomerRelationshipManagement.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<CustomerRelationshipManagementDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}CustomerRelationshipManagement.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<CustomerRelationshipManagementApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}CustomerRelationshipManagement.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<CustomerRelationshipManagementApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}CustomerRelationshipManagement.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            // 注册Application程序集中的所有应用服务为控制器
            options.ConventionalControllers.Create(typeof(CustomerRelationshipManagementApplicationModule).Assembly);
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerRelationshipManagement API", Version = "v1" });

            // 移除所有分组限制，让所有API都能显示
            // 不再需要DocInclusionPredicate，所有API都会显示在v1分组中

            //开启权限小锁
            options.OperationFilter<AddResponseHeadersFilter>();
            options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            options.OperationFilter<SecurityRequirementsOperationFilter>();
            options.CustomSchemaIds(type => type.FullName);

            //给参数设置默认值
            //options.SchemaFilter<SchemaFilter>();

            //在header中添加token，传递到后台
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT授权(数据将在请求头中进行传递)直接在下面框中输入Bearer {token}(注意两者之间是一个空格) \"",
                Name = "Authorization",//jwt默认的参数名称
                In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                BearerFormat = "JWT",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
            });


            
       
            //就是这里！！！！！！！！！
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var xmlPath = Path.Combine(basePath, "CustomerRelationshipManagement.Application.xml");//这个就是刚刚配置的xml文件名
            options.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
        });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();

            });

        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {

        // 初始化 CSRedis
        //RedisHelper.Initialization(new CSRedis.CSRedisClient("10.223.3.246:6379"));

        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.MapAbpStaticAssets();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        /*app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }*/
        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerRelationshipManagement API");

            // 模型的默认扩展深度，设置为 -1 完全隐藏模型
            c.DefaultModelsExpandDepth(1);
            // API文档仅展开标记
            c.DocExpansion(DocExpansion.List);
            c.DefaultModelRendering(ModelRendering.Example);
            c.DefaultModelExpandDepth(-1);
            // API前缀设置为空
            c.RoutePrefix = string.Empty;
            // API页面Title
            c.DocumentTitle = "😍接口文档 - 阿星Plus⭐⭐⭐";
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
