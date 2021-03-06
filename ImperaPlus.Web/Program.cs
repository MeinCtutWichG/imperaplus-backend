﻿using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace ImperaPlus.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
#if !DEBUG
                .UseApplicationInsights()
#endif
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors", "true")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
