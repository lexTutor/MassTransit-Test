using GeneralMicroservices.EmailingEvent.Controllers.EventingControllers;
using GeneralMicroservices.EmailingEvent.Model;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralMicroservices.EmailingEvent
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

            services.AddMassTransit(config =>
            {
                config.AddConsumer<MailEventController>();

                config.AddDelayedMessageScheduler();

                //Uri schedulerEndpoint = new Uri("queue:scheduler");

                //config.AddMessageScheduler(schedulerEndpoint);


                config.UsingRabbitMq((busCtx, RbMqBusCfg) =>
                {
                    RbMqBusCfg.Host(
                        Configuration["RabbitMqConguration:Host"], 
                        hostData => {
                            hostData.Username(Configuration["RabbitMqConguration:UserName"]);
                            hostData.Password(Configuration["RabbitMqConguration:PassWord"]);
                        });
                    //RbMqBusCfg.UseMessageScheduler(schedulerEndpoint);

                    RbMqBusCfg.UseDelayedMessageScheduler();

                    RbMqBusCfg.ReceiveEndpoint("email-sendingQueue", cfg =>
                        {
                            //cfg.UseScheduledRedelivery(r =>
                            //{
                            //    r.Intervals(TimeSpan.FromSeconds(120));
                            //});

                            cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(120)));
                            cfg.PrefetchCount = 1;
                            cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(30)));
                            cfg.ConfigureConsumer<MailEventController>(busCtx);
                        });
                });
            });

            services.AddMassTransitHostedService();
            services.AddScoped<MailEventController>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GeneralMicroservices.EmailingEvent", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeneralMicroservices.EmailingEvent v1"));
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
