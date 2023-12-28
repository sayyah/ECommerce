using ECommerce.API;
using ECommerce.Application.PersianTranslations.Identity;
using ECommerce.Infrastructure.DataContext;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

DotNetEnv.Env.TraversePath().Load("../.env");

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();

var builder = WebApplication.CreateBuilder(args);

builder
    .Host
    .UseSerilog(
        (hostingContext, loggerConfiguration) =>
            loggerConfiguration
                .Enrich
                .FromLogContext()
                .WriteTo
                .Console(new CustomLogFormatter(), LogEventLevel.Error)
                .WriteTo
                .MSSqlServer(
                    builder.Configuration.GetConnectionString("SunflowerECommerce"),
                    new MSSqlServerSinkOptions
                    {
                        TableName = "LogEvents",
                        SchemaName = "EventLogging",
                        AutoCreateSqlTable = true,
                        BatchPostingLimit = 1000,
                        BatchPeriod = new TimeSpan(00, 00, 30)
                    },
                    restrictedToMinimumLevel: LogEventLevel.Error
                )
                .ReadFrom
                .Configuration(hostingContext.Configuration)
    );

//builder.WebHost.ConfigureKestrel((context, options) =>
//{
//    options.ConfigureEndpointDefaults(listenOptions =>
//    {
//        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
//        listenOptions.UseHttps();
//    });
//});

var siteSetting = builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();

builder.Services.AddControllers();
builder
    .Services
    .AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
    );

builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();

builder
    .Services
    .AddDbContext<SunflowerECommerceDbContext>(
        option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("SunflowerECommerce"))
    );
builder
    .Services
    .AddDbContext<HolooDbContext>(
        option =>
            option
                .UseSqlServer(builder.Configuration.GetConnectionString("HolooConnectionString"),
                    o => o.UseCompatibilityLevel(110))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
    );

// Add Hangfire services.
builder
    .Services
    .AddHangfire(
        configuration =>
            configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(
                    builder.Configuration.GetConnectionString("SunflowerECommerce")
                )
    );

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

builder
    .Services
    .AddSwaggerGen(swagger =>
    {
        //swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
        //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        //swagger.IncludeXmlComments(xmlPath);
        swagger.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
            }
        );
        swagger.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            }
        );
    });

builder.Services.AddDataProtection();

builder.Services.AddSingleton(siteSetting);

builder
    .Services
    .AddIdentity<User, UserRole>(identityOption =>
    {
        identityOption.User.RequireUniqueEmail = true;
        identityOption.Password.RequiredLength = 8;
        identityOption.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<SunflowerECommerceDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<PersianIdentityErrorDescriber>();

builder
    .Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = siteSetting.IdentitySetting?.Audience,
            ValidIssuer = siteSetting.IdentitySetting?.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(siteSetting.IdentitySetting.IdentitySecretKey)
            )
        };
    });

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IConfiguration>(_ => builder.Configuration);

builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddRepositories(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger(c =>
    {
        c.SerializeAsV2 = true;
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
        c.RoutePrefix = string.Empty;
    });
}

using (var scope = app.Services.CreateScope())
{
    if (scope != null)
    {
        var context = scope.ServiceProvider.GetRequiredService<SunflowerECommerceDbContext>();
        context.Database.Migrate();
    }
}

//app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseHangfireDashboard(
    "/SunFlowerHangFire",
    new DashboardOptions { IsReadOnlyFunc = _ => false }
);

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
