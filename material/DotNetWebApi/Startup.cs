using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;

namespace DotNetWebApi
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotNetWebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Middleware
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetWebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(e =>
            {
                e.MapGet("/", c => c.Response.WriteAsync("Hello world!"));

                e.MapGet("hello", context =>
                    context.Response.WriteAsJsonAsync(new { Message = "Hello, visitor" })
                );

                e.MapGet("hello/{name}", context => {

                    context.Response.WriteAsync($"Hello, {context.Request.RouteValues["name"]}");

                    return System.Threading.Tasks.Task.CompletedTask;
                });

                e.MapPost("hello", async context => {

                    var value = new StringValues(context.Request.Host.Host);

                    context.Response.Headers.Add("Host-name", value);

                    using var reader = new StreamReader(context.Request.Body, System.Text.Encoding.UTF8);
                    var content = await reader.ReadToEndAsync();

                    System.Console.WriteLine(content);

                    context.Response.StatusCode = 200;
                });

                e.MapGet("db/hello", async context => {

                    var adventureWorks = "data source=localhost,1433;initial catalog=Adventureworks;persist security info=True;user id=sa;password=Password.123;MultipleActiveResultSets=True;";

                    using (var connection = new SqlConnection(adventureWorks))
                      {
                        SqlCommand command = new SqlCommand("EXEC [dbo].[sp_HelloWorld]", connection);
                        command.Connection.Open();
                        var helloDb = command.ExecuteScalar() as string;

                        context.Response.StatusCode = 200;

                        await context.Response.WriteAsync(helloDb);
                    }
                });
                e.MapControllers();
            });
        }
    }
}
