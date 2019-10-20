using Microsoft.Extensions.Configuration;
using Moneyger.Repositories.Models;
using System;
using Microsoft.Extensions.Options;
using Z.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Moneyger.DataInit
{
    class Program
    {
        private static WASContext WASContext;
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
            string connectionString = config.GetConnectionString("WASContext");
            var options = new DbContextOptionsBuilder<WASContext>()
                .UseNpgsql(connectionString)
                .Options;
            WASContext = new WASContext(options);
            EntityFrameworkManager.ContextFactory = DbContext => WASContext;

            DataInit dataInit = new DataInit(WASContext);
            dataInit.Init();
        }
    }
}
