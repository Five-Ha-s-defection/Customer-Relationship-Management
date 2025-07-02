using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace CustomerRelationshipManagement;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
            .CreateLogger();

        try
        {
            Log.Information("Starting CustomerRelationshipManagement.HttpApi.Host.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
                
            await builder.AddApplicationAsync<CustomerRelationshipManagementHttpApiHostModule>();
            // 跨域
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>//  默认策略
                {
                    policy.WithOrigins("http://localhost:3001") // 前端地址
                          .AllowAnyHeader()//  允许所有请求头
                          .AllowAnyMethod()//  允许所有方法
                          .AllowCredentials(); // 如果使用 cookie/token 认证
                });
            });


            var app = builder.Build();

            //app.UseCors();
            
            await app.InitializeApplicationAsync();

            await app.RunAsync();
             
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
