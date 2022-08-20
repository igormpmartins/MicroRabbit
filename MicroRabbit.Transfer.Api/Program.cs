using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.IoC;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace MicroRabbit.Transfer.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //For Net5, would be Startup
            builder.Services.AddMediatR(typeof(Program));

            var connStr = builder.Configuration.GetConnectionString("TransferDbConnection");
            builder.Services.AddDbContext<TransferDbContext>(opt => opt.UseSqlServer(connStr));

            // Add services to the container.
            RegisterServices(builder.Services);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transfer Microservice", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transfer Microservice V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            ConfigureEventBus(app);

            app.Run();
        }

        private static void ConfigureEventBus(WebApplication app)
        {
            var eventBus = app.Services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }
    }
}