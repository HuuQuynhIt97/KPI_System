using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using A4KPI.Helpers;
using A4KPI.DTO;
using A4KPI._Services.Services;
using A4KPI.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using A4KPI.Helpers.AutoMapper;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using A4KPI._Repositories.Interface;
using A4KPI._Repositories.Repositories;
using A4KPI._Services.Interface;
using A4KPI.SignalR;

namespace A4KPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private string factory;
        private string area;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            factory = Configuration.GetSection("Appsettings:Factory").Value;
            area = Configuration.GetSection("Appsettings:Area").Value;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appsettings = Configuration.GetSection("Appsettings").Get<Appsettings>();
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddHttpContextAccessor();
            services.AddControllers()
             .AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
             });
            var connetionString = Configuration.GetConnectionString($"{factory}_{area}_DefaultConnection");
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(connetionString);
            });

            services.AddAutoMapper(typeof(Startup))
                .AddScoped<IMapper>(sp =>
                {
                    return new Mapper(AutoMapperConfig.RegisterMappings());
                })
                .AddSingleton(AutoMapperConfig.RegisterMappings());

            //Swagger
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Score KPI", Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[0]}
                };
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.SetIsOriginAllowed(_ => true).
                AllowAnyMethod().
                AllowAnyHeader().
                AllowCredentials());
            });
            services.AddSignalR();

            //Repository
            services.AddScoped<IAccountGroupAccountRepository, AccountGroupAccountRepository>();
            services.AddScoped<IAccountGroupRepository, AccountGroupRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IDoRepository, DoRepository>();
            services.AddScoped<IFunctionTranslationRepository, FunctionTranslationRepository>();
            services.AddScoped<IKPIAccountRepository, KPIAccountRepository>();
            services.AddScoped<IKPINewRepository, KPINewRepository>();
            services.AddScoped<IOptionFunctionSystemRepository, OptionFunctionSystemRepository>();
            services.AddScoped<IOptionRepository, OptionRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<ITargetPICRepository, TargetPICRepository>();
            services.AddScoped<ITargetRepository, TargetRepository>();
            services.AddScoped<ITargetYTDRepository, TargetYTDRepository>();
            services.AddScoped<ITypeRepository, TypeRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IAccountTypeRepository, AccountTypeRepository>();
            services.AddScoped<IOCRepository, OCRepository>();
            services.AddScoped<ISettingMonthRepository, SettingMonthRepository>();
            services.AddScoped<ITodolist2Repository, Todolist2Repository>();
            services.AddScoped<IActionStatusRepository, ActionStatusRepository>();
            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IVersionRepository, VersionRepository>();
            services.AddScoped<ISystemLanguageRepository, SystemLanguageRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<IEvaluationRepository, EvaluationRepository>();
            services.AddScoped<IScoreRepository, ScoreRepository>();
            services.AddScoped<IKPIScoreRepository, KPIScoreRepository>();

            services.AddScoped<IAttitudeScoreRepository, AttitudeScoreRepository>();
            services.AddScoped<IAttitudeKeypointRepository, AttitudeKeypointRepository>();
            services.AddScoped<IAttitudeBehaviorRepository, AttitudeBehaviorRepository>();
            services.AddScoped<IAttitudeAttchmentRepository, AttitudeAttchmentRepository>();
            services.AddScoped<IAttitudeCategoryRepository, AttitudeCategoryRepository>();
            services.AddScoped<IAttitudeHeadingRepository, AttitudeHeadingRepository>();
            services.AddScoped<IBehaviorCheckRepository, BehaviorCheckRepository>();
            services.AddScoped<IJobTitleRepository, JobTitleRepository>();
            services.AddScoped<IKeypointRepository, KeypointRepository>();
            services.AddScoped<IBehaviorRepository, BehaviorRepository>();
            services.AddScoped<IPointRepository, PointRepository>();

            services.AddScoped<ISpecialContributionScoreRepository, SpecialContributionScoreRepository>();
            services.AddScoped<ISpecialRatioRepository, SpecialRatioRepository>();
            services.AddScoped<ISpecialScoreRepository, SpecialScoreRepository>();
            services.AddScoped<ISpecialTypeRepository, SpecialTypeRepository>();
            services.AddScoped<ISpecialCompactRepository, SpecialCompactRepository>();
            services.AddScoped<IKPIScoreAttchmentRepository, KPIScoreAttchmentRepository>();

            services.AddScoped<IAttitudeSubmitRepository, AttitudeSubmitRepository>();
            services.AddScoped<ISystemFlowRepository, SystemFlowRepository>();
            services.AddScoped<IUserSystemFlowRepository, UserSystemFlowRepository>();
            services.AddScoped<ICommitteeScoreRepository, CommitteeScoreRepository>();
            services.AddScoped<IHRCommentCmteeRepository, HRCommentCmteeRepository>();
            services.AddScoped<ICommitteeSequenceRepository, CommitteeSequenceRepository>();
            services.AddScoped<INewAttitudeContentRepository, NewAttitudeContentRepository>();
            services.AddScoped<INewAttitudeScoreRepository, NewAttitudeScoreRepository>();
            services.AddScoped<INewAttitudeAttchmentRepository, NewAttitudeAttchmentRepository>();
            services.AddScoped<INewAttitudeEvaluationRepository, NewAttitudeEvaluationRepository>();
            services.AddScoped<INewAttitudeSubmitRepository, NewAttitudeSubmitRepository>();
            services.AddScoped<IAccountCampaignRepository, AccountCampaignRepository>();
            services.AddScoped<IPerfomanceEvaluationTypeRepository, PerfomanceEvaluationTypeRepository>();
            services.AddScoped<IPerfomanceEvaluationImpactRepository, PerfomanceEvaluationImpactRepository>();



            //Services
            services.AddScoped<IMailExtension, MailExtension>();
            services.AddScoped<IMailHelper, MailHelper>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountTypeService, AccountTypeService>();
            services.AddScoped<IAccountGroupService, AccountGroupService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOCService, OCService>();
            services.AddScoped<IAccountGroupAccountService, AccountGroupAccountService>();
            services.AddScoped<ITargetYTDService, TargetYTDService>();
            services.AddScoped<IMeetingService, MeetingService>();
            services.AddScoped<ISettingMonthService, SettingMonthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IKPINewService, KPINewService>();
            services.AddScoped<IToDoList2Service, Todolist2Service>();
            services.AddScoped<IVersionService, VersionService>();
            services.AddScoped<ISystemLanguageService, SystemLanguageService>();
            services.AddScoped<IReportService, ReportService>();

            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IEvaluationService, EvaluationService>();
            services.AddScoped<IAttitudeScoreService, AttitudeScoreService>();
            services.AddScoped<IPeopleCommitteeService, PeopleCommitteeService>();
            services.AddScoped<IHQReportService, HQReportService>();
            services.AddScoped<ICoreCompetenciesService, CoreCompetenciesService>();
            services.AddScoped<INewAttitudeScoreService, NewAttitudeScoreService>();

            services.AddScoped<IJobTitleService, JobTitleService>();
            services.AddScoped<IAttitudeAttchmentService, AttitudeAttchmentService>();
            services.AddScoped<IAttitudeBehaviorService, AttitudeBehaviorService>();
            services.AddScoped<IAttitudeCategoryService, AttitudeCategoryService>();
            services.AddScoped<IAttitudeHeadingService, AttitudeHeadingService>();
            services.AddScoped<IAttitudeKeypointService, AttitudeKeypointService>();
            services.AddScoped<IKPIScoreService, KPIScoreService>();
            services.AddScoped<ISpecialContributionScoreService, SpecialContributionScoreService>();
            services.AddScoped<IKPIMonthPerfService, KPIMonthPerfService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<KPIHub>("/kpi-hub");
                endpoints.MapControllers();
            });

        }
    }
}
