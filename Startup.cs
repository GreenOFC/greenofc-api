using _24hplusdotnetcore.BatchJob;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Mappings;
using _24hplusdotnetcore.Middleware;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.ModelDtos.PtfOmnis;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.AT;
using _24hplusdotnetcore.Services.CheckSims;
using _24hplusdotnetcore.Services.CIMB;
using _24hplusdotnetcore.Services.EC;
using _24hplusdotnetcore.Services.F88;
using _24hplusdotnetcore.Services.MAFC;
using _24hplusdotnetcore.Services.MC;
using _24hplusdotnetcore.Services.OCR;
using _24hplusdotnetcore.Services.Otp;
using _24hplusdotnetcore.Services.PtfOmnis;
using _24hplusdotnetcore.Services.Transaction;
using _24hplusdotnetcore.Settings;
using Amazon.Runtime;
using Amazon.S3;
using AutoMapper;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace _24hplusdotnetcore
{
    public class Startup
    {
        readonly string AllowSpecificOrigins = "_allowSpecificOrigins";

        private readonly IWebHostEnvironment _env;

        public IConfiguration Configuration { get; }


        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MongoDbConnection>(Configuration.GetSection(nameof(MongoDbConnection)));
            services.Configure<MCConfig>(Configuration.GetSection("MCConfig"));
            services.Configure<FIBOConfig>(Configuration.GetSection("FIBOConfig"));
            services.Configure<OCRConfig>(Configuration.GetSection("OCRConfig"));
            services.Configure<CIMBConfig>(Configuration.GetSection("CIMBConfig"));
            services.Configure<MAFCConfig>(Configuration.GetSection("MAFCConfig"));
            services.Configure<MCKiosConfig>(Configuration.GetSection("MCKiosConfig"));
            services.Configure<ATConfig>(Configuration.GetSection("ATConfig"));
            services.Configure<F88Config>(Configuration.GetSection("F88Config"));
            services.Configure<CronJobConfig>(Configuration.GetSection("CronJobConfig"));
            services.Configure<ServerUpload>(Configuration.GetSection("ServerUpload"));
            services.Configure<OtpConfig>(Configuration.GetSection("OtpConfig"));
            services.Configure<ECConfig>(options => Configuration.GetSection("ECConfig").Bind(options));
            services.Configure<S3Config>(Configuration.GetSection(nameof(S3Config)));
            services.Configure<SmsConfig>(Configuration.GetSection(nameof(SmsConfig)));
            services.Configure<EmailConfig>(Configuration.GetSection(nameof(EmailConfig)));
            services.Configure<OKLendingConfig>(Configuration.GetSection(nameof(OKLendingConfig)));
            services.Configure<MobileConfig>(Configuration.GetSection(nameof(MobileConfig)));
            services.Configure<PtfOmniConfig>(Configuration.GetSection(nameof(PtfOmniConfig)));
            services.Configure<PayMeConfig>(Configuration.GetSection(nameof(PayMeConfig)));
            services.Configure<PtfScoringConfig>(Configuration.GetSection(nameof(PtfScoringConfig)));

            services.Scan(scan => scan
                .FromAssemblyOf<IScopedLifetime>()
                    .AddClasses(classes => classes.AssignableTo<IScopedLifetime>())
                        .AsSelf()
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());

            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowSpecificOrigins,
                              builder =>
                              {
                                  builder.WithOrigins("*")
                                         .AllowAnyHeader()
                                         .AllowAnyMethod();
                              });
            });

            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            services.AddDirectoryBrowser();

            services.AddScoped<IAmazonS3>(sp =>
            {
                S3Config s3Config = sp.GetRequiredService<IOptions<S3Config>>().Value;
                var credentials = new BasicAWSCredentials(s3Config.AccessKey, s3Config.SecretKey);
                var config = new AmazonS3Config { ServiceURL = s3Config.ServiceURL };
                return new AmazonS3Client(credentials, config);
            });
            services.AddSingleton<IMongoDbConnection>(sp => sp.GetRequiredService<IOptions<MongoDbConnection>>().Value);
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

            var mongoConnection = Configuration.GetSection("MongoDbConnection:ConnectionString");
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(mongoConnection.Value, "Hangfire", new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        MigrationStrategy = new DropMongoMigrationStrategy(),
                        BackupStrategy = new CollectionMongoBackupStrategy(),
                    },
                    CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
                }));
            services.AddHangfireServer();
            services.AddHostedService<DataSeedingJob>();

#if !DEBUG
            //Add batchjob
            services.AddHostedService<PushCustomerToCRM>();

            services.AddHostedService<PullLeadFromCRM>();
            // services.AddHostedService<OCRReceiveJob>();
            services.AddHostedService<PushDataProcessingJob>();
            services.AddHostedService<UploadProcessingJob>();

            services.AddHostedService<GetMasterDataJob>();
            // services.AddHostedService<DataF88ProcessingJob>();
            services.AddHostedService<PtfOmniDataProcessingJob>();

#endif
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddMvcCore().AddNewtonsoftJson(options => options.UseCamelCasing(true));
            ConfigureMCRestClient(services);
            ConfigureOCRRestClient(services);
            ConfigureCIMBRestClient(services);
            ConfigureMAFCMasterDataRestClient(services);
            ConfigureMAFCCheckCustomerRestClient(services);
            ConfigureMAFCDataEntryRestClient(services);
            ConfigureMAFCUploadRestClient(services);
            ConfigureMAFCUpdateDataRestClient(services);
            ConfigureMAFCS37RestClient(services);
            ConfigureF88RestClient(services);
            ConfigureATRestClient(services);
            ConfigureOtpRestClient(services);
            ConfigureECRestClient(services);
            ConfigureSmsRestClient(services);
            ConfigurePtfOmniRestClient(services);
            ConfigureF88AuthenRestClient(services);
            ConfigurePaymeRestClient(services);
            ConfigurePtfScoringRestClient(services);

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
                mc.AddProfile(new MobileMappingProfile());
                mc.AddProfile(new MAFCMappingProfile());
                mc.AddProfile(new TicketMappingProfile());
                mc.AddProfile(new ECMappingProfile());
                mc.AddProfile(new LeadVpsMappingProfile());
                mc.AddProfile(new LeadF88Profile());
                mc.AddProfile(new LeadEcProfile());
                mc.AddProfile(new LeadCimbProfile());
                mc.AddProfile(new LeadMcProfile());
                mc.AddProfile(new PosProfile());
                mc.AddProfile(new LeadSourceMappingProfile());
                mc.AddProfile(new LeadShinhanProfile());
                mc.AddProfile(new GroupNotificationMapping());
                mc.AddProfile(new GroupNotificationUserMapping());
                mc.AddProfile(new LeadPtfProfile());
                mc.AddProfile(new PtfOmniMappingProfile());
                mc.AddProfile(new TransactionProfile());
                mc.AddProfile(new MCProfile());
                mc.AddProfile(new SaleChanelMappingProfile());
                mc.AddProfile(new SaleChanelConfigUserMappingProfile());
                mc.AddProfile(new CheckSimProfile());
                mc.AddProfile(new DebtManageMappingProfile());
                mc.AddProfile(new ImportFileMappingProfile());
                mc.AddProfile(new ProjectProfileReportProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(_env.ContentRootPath + "/" + "notification-credential.json")
            });
            services.AddMemoryCache();

            BsonClassMap.RegisterClassMap<PtfOmniFetchServiceResponse>();
            BsonClassMap.RegisterClassMap<MCResponseDto>();
            BsonClassMap.RegisterClassMap<MAFCCheckCustomerResponse>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "mobileapp")),
                RequestPath = "/mobileapp"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "FileUpload")),
                RequestPath = "/FileUpload"
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(AllowSpecificOrigins);

            app.UseAuthentication();

            app.UseMiddleware<CheckVersionMiddleware>();

            app.UseMiddleware<CheckAcessMiddleware>();

            app.UseMiddleware<SetPermissionMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHangfireDashboard();
        }

        private void ConfigureMCRestClient(IServiceCollection services)
        {
            MCConfig mCConfig = services.BuildServiceProvider().GetService<IOptions<MCConfig>>().Value;

            services.AddRefitClient<IRestLoginService>()
                .ConfigureHttpClient((serviceProvider, conf) =>
                {
                    conf.BaseAddress = new Uri(mCConfig.Host);
                    conf.DefaultRequestHeaders.Add("x-security", mCConfig.SecurityKey);
                });

            services.AddRefitClient<IRestMCService>()
                .ConfigureHttpClient((serviceProvider, conf) =>
                {
                    conf.BaseAddress = new Uri(mCConfig.Host);
                    conf.DefaultRequestHeaders.Add("x-security", mCConfig.SecurityKey);
                })
                .ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
                {
                    var configService = serviceProvider.GetRequiredService<ConfigServices>();
                    Func<Task<string>> getToken = async () =>
                    {
                        var config = configService.FindOneByKey(ConfigKey.MC_AUTHORIZATION);
                        return await Task.FromResult($"{config?.Value}");
                    };
                    return new RestHttpClientHandler(getToken);
                });

        }

        private void ConfigureOCRRestClient(IServiceCollection services)
        {
            services.AddRefitClient<IRestOCRService>()
               .ConfigureHttpClient((serviceProvider, conf) =>
               {
                   var config = serviceProvider.GetRequiredService<IOptions<OCRConfig>>().Value;
                   conf.BaseAddress = new Uri(config.Host);
                   conf.DefaultRequestHeaders.Add("x-api-key", config.APIKey);
               });
        }

        private void ConfigureCIMBRestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        })
            };

            services.AddRefitClient<ICIMBRestService>(refitSettings)
                   .ConfigureHttpClient((serviceProvider, conf) =>
                   {
                       var config = serviceProvider.GetRequiredService<IOptions<CIMBConfig>>().Value;
                       conf.BaseAddress = new Uri(config.Host);
                       conf.DefaultRequestHeaders.Add("partner-id", config.PartnerId);
                       conf.DefaultRequestHeaders.Add("key-id", config.KeyId);
                       //conf.DefaultRequestHeaders.Add("x-identifier", config.XIdentifier);
                   })
                    .ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
                    {
                        var config = serviceProvider.GetRequiredService<IOptions<CIMBConfig>>().Value;
                        string filePath = _env.ContentRootPath + "/" + config.CertFilePath;
                        var certificate = new X509Certificate2(filePath, config.CertPassword);
                        var handler = new HttpClientHandler();
                        handler.ClientCertificates.Add(certificate);
                        return handler;
                    });
        }
        private void ConfigureECRestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        })
            };

            services.AddRefitClient<IECRestAuthorization>(refitSettings)
                   .ConfigureHttpClient((serviceProvider, conf) =>
                   {
                       var config = serviceProvider.GetRequiredService<IOptions<ECConfig>>().Value;
                       conf.BaseAddress = new Uri(config.Host);
                   })
                    .ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
                    {
                        var config = serviceProvider.GetRequiredService<IOptions<ECConfig>>().Value;
                        Func<Task<string>> getToken = async () =>
                        {
                            string token = Convert.ToBase64String(Encoding.ASCII.GetBytes(config.ClientId + ":" + config.ClientSecret));
                            return await Task.FromResult(token);
                        };
                        return new RestHttpClientHandler(getToken);
                    });

            services.AddRefitClient<IECRestService>(refitSettings)
                   .ConfigureHttpClient((serviceProvider, conf) =>
                   {
                       var config = serviceProvider.GetRequiredService<IOptions<ECConfig>>().Value;
                       conf.BaseAddress = new Uri(config.Host);
                   });
        }

        private void ConfigureMAFCMasterDataRestClient(IServiceCollection services)
        {
            services.AddRefitClient<IRestMAFCMasterDataService>()
               .ConfigureHttpClient((serviceProvider, conf) =>
               {
                   var config = serviceProvider.GetRequiredService<IOptions<MAFCConfig>>().Value;
                   conf.BaseAddress = new Uri(config.Host);
               })
               .ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
               {
                   var config = serviceProvider.GetRequiredService<IOptions<MAFCConfig>>().Value;
                   Func<Task<string>> getToken = async () =>
                   {
                       string token = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(config.MasterData.Username + ":" + config.MasterData.Password));
                       return await Task.FromResult(token);
                   };
                   return new RestHttpClientHandler(getToken);
               });
        }

        private void ConfigureMAFCCheckCustomerRestClient(IServiceCollection services)
        {
            services.AddRefitClient<IRestMAFCCheckCustomerService>()
               .ConfigureHttpClient((serviceProvider, conf) =>
               {
                   var config = serviceProvider.GetRequiredService<IOptions<MAFCConfig>>().Value;
                   conf.BaseAddress = new Uri(config.Host);
               })
               .ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
               {
                   var config = serviceProvider.GetRequiredService<IOptions<MAFCConfig>>().Value;
                   Func<Task<string>> getToken = async () =>
                   {
                       string token = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(config.CheckCustomer.Username + ":" + config.CheckCustomer.Password));
                       return await Task.FromResult(token);
                   };
                   return new RestHttpClientHandler(getToken);
               });
        }

        private void ConfigureMAFCDataEntryRestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            };
            services.AddRefitClient<IRestMAFCDataEntryService>(refitSettings)
               .ConfigureHttpClient((serviceProvider, conf) =>
               {
                   var config = serviceProvider.GetRequiredService<IOptions<MAFCConfig>>().Value;
                   conf.BaseAddress = new Uri(config.Host);
               })
               .ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
               {
                   var config = serviceProvider.GetRequiredService<IOptions<MAFCConfig>>().Value;
                   Func<Task<string>> getToken = async () =>
                   {
                       string token = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(config.DataEntry.Username + ":" + config.DataEntry.Password));
                       return await Task.FromResult(token);
                   };
                   return new RestHttpClientHandler(getToken);
               });
        }
        private void ConfigureMAFCUploadRestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            };
            services.AddRefitClient<IRestMAFCUploadService>(refitSettings)
               .ConfigureHttpClient((serviceProvider, conf) =>
               {
                   var config = serviceProvider.GetRequiredService<IOptions<MAFCConfig>>().Value;
                   conf.BaseAddress = new Uri(config.Host);
               })
               .ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
               {
                   var config = serviceProvider.GetRequiredService<IOptions<MAFCConfig>>().Value;
                   Func<Task<string>> getToken = async () =>
                   {
                       string token = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(config.DataUpload.Username + ":" + config.DataUpload.Password));
                       return await Task.FromResult(token);
                   };
                   return new RestHttpClientHandler(getToken);
               });
        }

        private void ConfigureMAFCUpdateDataRestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            };
            services.AddRefitClient<IRestMAFCUpdateDataService>(refitSettings)
               .ConfigureHttpClient((serviceProvider, conf) =>
               {
                   var config = serviceProvider.GetRequiredService<IOptions<MAFCConfig>>().Value;
                   conf.BaseAddress = new Uri(config.Host);
               })
               .ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
               {
                   var config = serviceProvider.GetRequiredService<IOptions<MAFCConfig>>().Value;
                   Func<Task<string>> getToken = async () =>
                   {
                       string token = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(config.DataUpdate.Username + ":" + config.DataUpdate.Password));
                       return await Task.FromResult(token);
                   };
                   return new RestHttpClientHandler(getToken);
               });
        }

        private void ConfigureMAFCS37RestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            };
            services.AddRefitClient<IRestMAFCS37Service>(refitSettings)
               .ConfigureHttpClient((serviceProvider, conf) =>
               {
                   var config = serviceProvider.GetRequiredService<IOptions<MAFCConfig>>().Value;
                   conf.BaseAddress = new Uri(config.Host);
               })
               .ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
               {
                   var config = serviceProvider.GetRequiredService<IOptions<MAFCConfig>>().Value;
                   Func<Task<string>> getToken = async () =>
                   {
                       string token = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(config.S37.Username + ":" + config.S37.Password));
                       return await Task.FromResult(token);
                   };
                   return new RestHttpClientHandler(getToken);
               });
        }

        private void ConfigureATRestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            };

            services.AddRefitClient<IRestATService>()
                .ConfigureHttpClient((serviceProvider, conf) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptions<ATConfig>>().Value;
                    conf.BaseAddress = new Uri(config.Host);
                })
                .ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptions<ATConfig>>().Value;
                    Func<Task<string>> getToken = async () =>
                    {
                        return await Task.FromResult(config.Token);
                    };
                    return new RestHttpClientHandler(getToken);
                });
        }

        private void ConfigureF88RestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            };
            services.AddRefitClient<IRestF88Service>(refitSettings)
                .ConfigureHttpClient((serviceProvider, conf) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptions<F88Config>>().Value;
                    conf.BaseAddress = new Uri(config.Host);
                });
        }

        private void ConfigureOtpRestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            };
            services.AddRefitClient<IOtpRestService>(refitSettings)
                .ConfigureHttpClient((serviceProvider, conf) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptions<OtpConfig>>().Value;
                    conf.BaseAddress = new Uri(config.Host);
                    conf.DefaultRequestHeaders.Add("x-api-key", config.XapiKey);
                });
        }

        private void ConfigureSmsRestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            };
            services.AddRefitClient<ISmsRestService>(refitSettings)
                .ConfigureHttpClient((serviceProvider, conf) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptions<SmsConfig>>().Value;
                    conf.BaseAddress = new Uri(config.Host);
                });
        }

        private void ConfigurePtfOmniRestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            };
            services.AddRefitClient<IRestPtfOmniService>(refitSettings)
                .ConfigureHttpClient((serviceProvider, conf) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptions<PtfOmniConfig>>().Value;
                    conf.BaseAddress = new Uri(config.Host);
                });
        }

        private void ConfigureF88AuthenRestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            };
            services.AddRefitClient<IRestF88AuthenService>(refitSettings)
                .ConfigureHttpClient((serviceProvider, conf) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptions<F88Config>>().Value;
                    conf.BaseAddress = new Uri(config.AuthenHost);
                });
        }

        private void ConfigurePaymeRestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            };
            services.AddRefitClient<IPaymeRestService>(refitSettings)
                .ConfigureHttpClient((serviceProvider, conf) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptions<PayMeConfig>>().Value;
                    conf.BaseAddress = new Uri(config.Host);
                });
        }

        private void ConfigurePtfScoringRestClient(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
            };
            services.AddRefitClient<IPtfScoringTrustingSocialRestService>(refitSettings)
                .ConfigureHttpClient((serviceProvider, conf) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptions<PtfScoringConfig>>().Value;
                    conf.BaseAddress = new Uri(config.Host);
                    conf.DefaultRequestHeaders.Add("PTF-3P-CLIENT", config.ClientKey);
                    conf.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.Token}");
                });
            services.AddRefitClient<IPtfCheckIncomeRestService>(refitSettings)
                .ConfigureHttpClient((serviceProvider, conf) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptions<PtfScoringConfig>>().Value;
                    conf.BaseAddress = new Uri(config.Host);
                    conf.DefaultRequestHeaders.Add("PTF-3P-CLIENT", config.ClientKey);
                    conf.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.Token}");
                });
            services.AddRefitClient<IPtfScoringVnptRestService>(refitSettings)
                .ConfigureHttpClient((serviceProvider, conf) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptions<PtfScoringConfig>>().Value;
                    conf.BaseAddress = new Uri(config.Host);
                    conf.DefaultRequestHeaders.Add("PTF-3P-CLIENT", config.ClientKey);
                    conf.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.Token}");
                });
        }
    }

    class RestHttpClientHandler : HttpClientHandler
    {
        private readonly Func<Task<string>> _getToken;

        public RestHttpClientHandler()
        {

        }

        public RestHttpClientHandler(Func<Task<string>> getToken)
        {
            _getToken = getToken ?? throw new ArgumentNullException("getToken");
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var auth = request.Headers.Authorization;

            if (auth != null && (auth.Scheme == "Bearer" || auth.Scheme == "Basic" || auth.Scheme == "Token"))
            {
                var token = await _getToken().ConfigureAwait(false);
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
            }

            return await base.SendAsync(request, cancellationToken);
        }

    }
}
