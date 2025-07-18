﻿using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Database.EF;

namespace Persistence;

public static class PersistenceInstaller
{
    public static void PersistenceInstall(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PersistenceOptions>(configuration.GetSection(PersistenceOptions.OptionsName));
        services.AddDbContext<ApplicationDbContext>((provider, builder) =>
        {
            var options = provider.GetRequiredService<IOptions<PersistenceOptions>>();
            builder.UseSqlite(options.Value.ConnectionString);
        });
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddSingleton<DatabaseMigrator>();
        services.AddSingleton<IStartupFilter, MigrateDatabaseOnStartup>();
    }
}