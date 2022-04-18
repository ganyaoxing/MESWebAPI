using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MESWebAPI.Models;
using MESWebAPI.Service;
using Microsoft.Net.Http.Headers;

#nullable disable

//var builder = WebApplication.CreateBuilder(args);
new PreAction();

try
{
    Log.Information("Starting web host");
    CreateHostBuilder(args).Build().Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
// Add services to the container.

// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers();

// app.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            // options.AddPolicy(name: MyAllowSpecificOrigins,
            //                   builder =>
            //                   {
            //                       builder.WithOrigins("https://localhost:7129/Test/ReturnJson",
            //                       "https://localhost:7129");
            //                   });
            // builder.WithOrigins("https://localhost:7129/Test/ReturnJson",
            //             "https://localhost:7129")
            options.AddDefaultPolicy(
            builder =>
                {
                    builder.AllowAnyOrigin();
                    //builder.WithOrigins("https://localhost:7129/Test/ReturnJson", "https://localhost:7129");
                });
        });
        services.AddControllers().AddNewtonsoftJson(
            options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }
        );
        services.AddMemoryCache();
        // services.AddHealthChecks();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //services.AddSwaggerGen();
        //(c =>
        //{
        //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MES1 API", Version = "v1" });
        //    c.ResolveConflictingActions(p => p.First());
        //});
        services.Configure<TokenManagement>(Configuration.GetSection("TokenManagement"));
        var token = Configuration.GetSection("TokenManagement").Get<TokenManagement>();
        // services.AddAuthentication(x =>
        // {
        //     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        // }).AddJwtBearer(x =>
        // {
        //     x.RequireHttpsMetadata = false;
        //     x.SaveToken = true;
        //     x.TokenValidationParameters = new TokenValidationParameters
        //     {
        //         ValidateIssuerSigningKey = true,
        //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Secret)),
        //         ValidIssuer = token.Issuer,
        //         ValidAudience = token.Audience,
        //         ValidateIssuer = false,
        //         ValidateAudience = false
        //     };
        // });
        services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler(errors =>
        {
            errors.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var error = feature?.Error;
                var result = new ApiResult().Fail(500, error.Message);
                if (error != null)
                {
                    Log.Error($"system has 500 error：{error.Message}-{error.StackTrace}");
                }
                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
            });
        });
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();//异常中间件
        }
        //if (app.Environment.IsDevelopment())
        //{
        //app.UseSwagger();
        //app.UseSwaggerUI();
        //}
        app.UseSerilogRequestLogging();

        // app.UseHttpsRedirection();
        //app.UseAuthentication();
        app.UseRouting();
        app.UseCors();
        app.UseAuthorization();
        #region 测试模块
        //app.UseEndpoints(Endpoints=>
        //{
        //    Endpoints.Map("/",async context=>
        //    {
        //        await context.Response.WriteAsync("HELLLLLLLLLL");
        //    });
        //});
        #endregion

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        //xxxxx/Swagger
        app.UseSwagger();
        app.UseSwaggerUI();
        //(c =>
        //{
        //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MES1 API");
        //});
        //xxxxx/health
        // app.UseHealthChecks("/health");
    }
}

public class PreAction
{
    private static readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

    public PreAction()
    {
        Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", env)
                .CreateLogger();
        Log.Information("Starting web host");
    }
}