using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tutorial.DBModels;

namespace Tutorial
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
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase());

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var context = app.ApplicationServices.GetService<ApiContext>();
            AddTestData(context);

            app.UseMvc();
        }

        private static void AddTestData(ApiContext context)
        {
            var testUser1 = new DBModels.User
            {
                Id = "abc123",
                FirstName = "Luke",
                LastName = "Skywalker"
            };

            context.Users.Add(testUser1);

            var testPost1 = new DBModels.Post
            {
                Id = "def234",
                UserId = testUser1.Id,
                Content = "What a piece of junk!"
            };

            context.Posts.Add(testPost1);

            context.SaveChanges();
        }
    }
}
