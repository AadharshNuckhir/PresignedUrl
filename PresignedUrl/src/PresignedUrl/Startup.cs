using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using PresignedUrl.Helper;

namespace PresignedUrl
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigHelper.LoadEnvironmentVariablesAndParameters();

            services.AddLogging(options =>
            {
                options.ClearProviders();
                options.AddAWSProvider();
            });

            // X-RAY
            AWSSDKHandler.RegisterXRayForAllServices();

#if DEBUG
            BasicAWSCredentials sessionAWSCredentials = new BasicAWSCredentials(ConfigHelper.AWS_ACCESS_KEY_ID, ConfigHelper.AWS_SECRET_ACCESS_KEY);
#endif

            // HealthChecks
            services.AddHealthChecks()
                .AddS3(options =>
                {
                    options.BucketName = ConfigHelper.S3_BUCKET_INVOICE;
                    options.S3Config = new AmazonS3Config
                    {
                        RegionEndpoint = RegionEndpoint.GetBySystemName(ConfigHelper.AWS_REGION)
                    };
                }, "S3");

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
                });

                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}