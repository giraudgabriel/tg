using Project.Infrastructure;
using Project.Infrastructure.Model;
using Project.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Project.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Project.CrossCutting.Base;
using Project.Api.Helper;
using Project.Api.Hub;
using Project.Api.Hub.Interfaces;

namespace Project.Api
{
    public class Startup
    {
        public const string Key = "qFBfvoaGAaXMPtqUON63xBiVF9EiSLIEZk14CJszAe0";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHubConnection, HubConnections>();
            services.AddCors();
            services.AddControllers().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            //Token
            var key = Encoding.ASCII.GetBytes(Key);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
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
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });


            const string convertZeroDateTime = "Convert Zero Datetime=true";
            var cs = Configuration.GetConnectionString("Project").Contains(convertZeroDateTime) ? Configuration.GetConnectionString("Project") : Configuration.GetConnectionString("Project") + convertZeroDateTime + ";";

            services.AddDbContext<ProjectContext>(options => options.UseMySql(cs));
            services.AddScoped<UnitOfWork>();
            services.AddScoped<IRequestDetail, RequestDetail>();
            ConfigureServiceRepository.Configure(services);
            ConfigureServiceApplicationService.Configure(services);

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.NameIdentifier);
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseErrorCatcherMiddleware();

            app.Use(async (context, next) =>
            {
                context.RequestServices.GetRequiredService<IHubContext<IscoolHub, IIscoolHub>>();

                if (next != null)
                {
                    await next.Invoke();
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<IscoolHub>("/hub/iscool");
                endpoints.MapControllers();
            });
        }
    }
}
