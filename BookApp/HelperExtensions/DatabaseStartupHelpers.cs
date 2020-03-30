﻿// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using DataLayer.EfCode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceLayer.DatabaseServices.Concrete;

namespace BookApp.HelperExtensions
{
    public static class DatabaseStartupHelpers
    {
        /// <summary>
        ///     This makes sure the database is create/updated and optionally it seeds the database with books.
        /// </summary>
        /// <param name="webHost"></param>
        /// <returns></returns>
        public static async Task<IHost> SetupDatabaseAsync(this IHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var config = services.GetRequiredService<IConfiguration>();
                var env = services.GetRequiredService<IWebHostEnvironment>();
                var context = services.GetRequiredService<EfCoreContext>();
                {
                    try
                    {
                        if (config["DemoSetup:UseInMemory"].Equals("true", StringComparison.InvariantCultureIgnoreCase))
                        {
                            await context.Database.MigrateAsync();
                        }
                        else
                        {
                            await context.Database.EnsureCreatedAsync();
                        }

                        await context.SeedDatabaseIfNoBooksAsync(env.WebRootPath);
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while creating/migrating or seeding the database.");
                    }
                }
            }

            return webHost;
        }

        //--------------------------------------------------------
        //private methods 

        private static SqliteConnection SetupSqliteInMemoryConnection()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder {DataSource = ":memory:"};
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            connection.Open(); //see https://github.com/aspnet/EntityFramework/issues/6968
            return connection;
        }
    }
}