using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timashev_PI_Lab.Models;

namespace Timashev_PI_Lab
{
    public class Program
    {
        public static User User = null;

        public static List<TechCard> newTechCards = new List<TechCard>();

        public static string Director = "Долгановский Ю. М.";
        public static string Organization = "МОУ СОШ с.Выползово";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
