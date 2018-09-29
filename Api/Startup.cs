﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EnterprisePatterns.Api.Common.Infrastructure.Persistence.NHibernate;
using EnterprisePatterns.Api.BankAccounts.Domain.Repository;
using EnterprisePatterns.Api.Accounts.Infrastructure.Persistence.NHibernate.Repository;
using EnterprisePatterns.Api.Customers.Domain.Repository;
using EnterprisePatterns.Api.Customers.Infrastructure.Persistence.NHibernate.Repository;
using EnterprisePatterns.Api.Movies.Domain.Repository;
using EnterprisePatterns.Api.Movies.Infrastructure.Persistence.NHibernate.Repository;
using AutoMapper;
using EnterprisePatterns.Api.BankAccounts.Application.Assembler;
using EnterprisePatterns.Api.Common.Application;

namespace EnterprisePatterns.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton(new SessionFactory(Environment.GetEnvironmentVariable("OnlineTheater_SQL")));
            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetService<IMapper>();
            services.AddSingleton(new BankAccountCreateAssembler(mapper));
            services.AddScoped<IUnitOfWork, UnitOfWorkNHibernate>();
            services.AddTransient<IBankAccountRepository, BankAccountNHibernateRepository>();
            services.AddTransient<ICustomerRepository, CustomerNHibernateRepository>();
            services.AddTransient<IMovieRepository, MovieNHibernateRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
