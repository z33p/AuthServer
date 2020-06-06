using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthServer.Data;
using AuthServer.Helpers;
using AuthServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer
{
  public class Startup
  {

    public void ApplyMigrations(DataContext context)
    {
      if (context.Database.GetPendingMigrations().Any())
      {
        context.Database.Migrate();
      }
    }
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      ApplyMigrations(new DataContext());
    }
    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors();
      services.AddControllers();

      services.AddDbContext<DataContext>();

      services.AddScoped<IAuthService, AuthService>();

      // Identity Config
      services
        .AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<DataContext>()
        .AddDefaultTokenProviders();

      // configure strongly typed settings objects
      var appSettingsSection = Configuration.GetSection("AppSettings");
      services.Configure<AppSettings>(appSettingsSection);

      // JWT
      var _appSettings = appSettingsSection.Get<AppSettings>();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = true
      };

      services.AddSingleton(tokenValidationParameters);

      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(x =>
      {
        // x.RequireHttpsMetadata = true;
        x.SaveToken = true;
        x.TokenValidationParameters = tokenValidationParameters;
      });
      //   var key = Encoding.ASCII.GetBytes(TokenSecret.Secret);
      //   services.AddAuthentication(x =>
      //   {
      //     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      //     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      //   })
      //   .AddJwtBearer(x =>
      // {
      //   x.RequireHttpsMetadata = false;
      //   x.SaveToken = true;
      //   x.TokenValidationParameters = new TokenValidationParameters
      //   {
      //     ValidateIssuerSigningKey = true,
      //     IssuerSigningKey = new SymmetricSecurityKey(key),
      //     ValidateIssuer = false,
      //     ValidateAudience = false
      //   };
      // });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
      );

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }

  }
}
