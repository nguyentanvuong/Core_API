using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers.Utils;
using WebApi.Helpers.DatabaseUtils;
using WebApi.Helpers.Common;
using WebApi.Helpers.Services;
using Microsoft.Extensions.Options;
using WebApi.Helpers;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Net.Mail;
using System.Net;
using WebApi.Models.FAST;
using DevExpress.AspNetCore;
using WebApi.Reports;
using Swashbuckle.AspNetCore.SwaggerGen;
using DevExpress.XtraReports.Web.Extensions;
using SoapCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public static string path = @"D:\jits\CoreAPI-main\CoreAPI-main\RsaContainsKey\";
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            _configuration = configuration;
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDevExpressControls();
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //Config FAST
            GlobalVariable.FASTRestfulURL = Configuration["FAST:URLRestful"].EndsWith("/") ? Configuration["FAST:URLRestful"][0..^1] : Configuration["FAST:URLRestful"];
            GlobalVariable.FASTSOAPURL = (Configuration["FAST:URLSOAP"].EndsWith("/") ? Configuration["FAST:URLSOAP"][0..^1] : Configuration["FAST:URLSOAP"]);
            GlobalVariable.CMUsername = Configuration["FAST:CMUsername"];
            GlobalVariable.CMPassword = Configuration["FAST:CMPassword"];
            FASTGetTokenRequest FastGetTokenRequest = new FASTGetTokenRequest
            {
                username = Configuration["FAST:Username"],
                password = Configuration["FAST:Password"],
                rememberMe = bool.Parse(Configuration["FAST:RememberMe"])
            };
            GlobalVariable.FASTGetTokenRequest = FastGetTokenRequest;

            //Config email server
            GlobalVariable.EmailFrom = Configuration["Smtp:Username"];
            GlobalVariable.SmtpClient = new SmtpClient(Configuration["Smtp:Host"])
            {
                Port = int.Parse(Configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(GlobalVariable.EmailFrom, O9Encrypt.Decrypt(Configuration["Smtp:Password"])),
                EnableSsl = true
            };

            GlobalVariable.DbInUse = appSettings.DatabaseProvider;
            GlobalVariable.DbConnectionString = O9Encrypt.Decrypt(appSettings.DBConnectionString);

            switch (appSettings.DatabaseProvider)
            {
                case GlobalVariable.MemoryDB:
                    services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("LocalDb"));
                    break;
                case GlobalVariable.Oracle:
                    services.AddDbContext<DataContext>(x => x.UseOracle(O9Encrypt.Decrypt(appSettings.DBConnectionString)));
                    break;
                case GlobalVariable.MSSQL:
                    services.AddDbContext<DataContext>(x => x.UseSqlServer(O9Encrypt.Decrypt(appSettings.DBConnectionString)));
                    break;
                case GlobalVariable.MySql:
                    services.AddDbContext<DataContext>(x => x.UseMySql(O9Encrypt.Decrypt(appSettings.DBConnectionString)));
                    break;
                case GlobalVariable.PostgreSQL:
                    services.AddDbContext<DataContext>(x => x.UseNpgsql(O9Encrypt.Decrypt(appSettings.DBConnectionString)));
                    break;
                case GlobalVariable.SqlLite:
                    services.AddDbContext<DataContext>(x => x.UseSqlite(O9Encrypt.Decrypt(appSettings.DBConnectionString)));
                    break;
                default:
                    services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("LocalDb"));
                    break;
            }

            services.AddCors();

            services.AddSoapCore();
            services.TryAddScoped<ISOAPService, SOAPService>();

            services.AddControllers(options =>
            {
                options.Filters.Add(new CustomValidationResponseActionFilter());
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
            }).AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true).AddNewtonsoftJson();

            services.AddMvc(options =>
            {
                options.Filters.Add(new CustomValidationResponseActionFilter());
                options.InputFormatters.Add(new CustomXmlInputFormatter());
                options.OutputFormatters.Add(new CustomXmlOutputFormatter());
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                    return !methodInfo.DeclaringType.AssemblyQualifiedName.StartsWith("DevExpress", StringComparison.OrdinalIgnoreCase);
                });
                c.SwaggerDoc("V1.0a", new OpenApiInfo { Title = "CoreAPI", Version = "V1.0a" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
            });

            services.AddSingleton<IWorkerService, WorkerService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IApiService, ApiService>();
            services.AddScoped<ISystemService, SystemService>();
            services.AddScoped<IIPCService, IPCService>();
            services.AddScoped<IFASTService, FASTService>();
            services.AddScoped<IFASTPublicService, FASTPublicService>();
            services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();
            services.AddScoped<IReportService, ReportService>();
            services.AddHttpContextAccessor();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext context, IOptions<AppSettings> appSettings)
        {
            app.UseRouting();

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            if (appSettings.Value.UsingJWT)
            {
                GlobalVariable.IsUsingJWT = true;
                app.UseMiddleware<JwtMiddleware>();
            }
            if (appSettings.Value.UsingLicense)
            {
                GlobalVariable.IsUsingLicense = true;
                app.UseMiddleware<LicenseMiddleware>();
            }
            if (appSettings.Value.UsingFullLog)
            {
                app.UseMiddleware<LoggingMiddleware>();
            }
            if (appSettings.Value.UsingEncrypt)
            {
                GlobalVariable.IsUsingEncrypt = true;
                app.UseMiddleware<EncryptMiddleware>();
            }

            // Init all setting for ActiveMQ, MemCached
            O9Client.Init(appSettings.Value.Memcached, appSettings.Value);
            AutoLogin auto = new AutoLogin(context, appSettings.Value.DefaultUser);
            auto.GetAllCdlist();
            auto.GetAllSearchFunc();
            auto.GetAllSForm();
            auto.GetNewFASTToken();
            auto.GetDefaultConfigFromDatabase();
            if (O9Client.isInit)
            {
                auto.LoginDefaultUser();
            }

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "CoreAPI/{documentName}/CoreAPI.json";
            });

            app.UseStaticFiles();

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "CoreAPI";
                c.DefaultModelsExpandDepth(-1);
                c.SwaggerEndpoint("/CoreAPI/V1.0a/CoreAPI.json", "CoreAPI Version 1.0a");
                c.InjectStylesheet("/swagger-ui/custom.css");
                c.InjectJavascript("/swagger-ui/custom.js", "text/javascript");
                c.InjectJavascript("/swagger-ui/generate-pdf.js", "text/javascript");
                c.InjectJavascript("/swagger-ui/rapipdf-min.js", "text/javascript");
                c.DocumentTitle = "O9Core API For SBILH";
            });

            app.UseDevExpressControls();
            app.UseEndpoints(endpoints => {
                endpoints.UseSoapEndpoint<ISOAPService>(GlobalVariable.PathSOAPPublic, new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
            });
            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
