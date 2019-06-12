using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyBGServer.DL;
using MyBGServer.Helpers;
using MyBGServer.Infra;
using MyBGServer.Repo;
using MyBGServer.Services;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using MyBGServer.Hubs;
using Microsoft.Owin;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

[assembly: OwinStartup(typeof(MyBGServer.Startup))]
namespace MyBGServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors();
			services.AddSignalR();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			var appSettingsSection = Configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettingsSection);
			var appSettings = appSettingsSection.Get<AppSettings>();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};
				x.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var accessToken = context.Request.Query["access_token"];
						var path = context.HttpContext.Request.Path;
						if (!string.IsNullOrEmpty(accessToken) &&
							(path.StartsWithSegments("http://localhost:44320/hub/")))
						{
							context.Token = accessToken;
						}
						return Task.CompletedTask;
					}
				};
			});
			services.AddSingleton<Context>();
			services.AddSingleton<IRepo, MyRepo>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IHubService, HubService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}
			app.UseCors(x => x
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());
			app.UseWebSockets();
			app.UseAuthentication();
			app.UseHttpsRedirection();
			app.UseMvc();
			app.UseSignalR(routes =>
			{
				routes.MapHub<MyHub>("/hub");
			});
		}
	}
}
