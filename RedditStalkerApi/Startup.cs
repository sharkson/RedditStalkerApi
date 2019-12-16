using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reddit;
using RedditStalkerService;
using System;
using System.Net.Http;

namespace RedditStalkerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var appId = Configuration.GetSection("appId").Value;
            var appSecret = Configuration.GetSection("appSecret").Value;
            var refreshToken = Configuration.GetSection("refreshToken").Value;
            var redditClient = new RedditClient(appId: appId, appSecret: appSecret, refreshToken: refreshToken);
            services.AddSingleton(redditClient);

            var httpClient = new HttpClient();
            var sharkbotApiUrl = Configuration.GetSection("SharkbotApiUrl").Value;
            httpClient.BaseAddress = new Uri(sharkbotApiUrl);
            services.AddSingleton(httpClient);

            services.AddSingleton<RedditConversationService, RedditConversationService>();
            services.AddSingleton<StalkerService, StalkerService>();
            services.AddSingleton<ApiService, ApiService>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
