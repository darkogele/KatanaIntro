using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KatanaIntro
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        string uri = "http://localhost:3333";

    //        using (WebApp.Start<Startup>(uri))
    //        {
    //            Console.WriteLine("Started");
    //            Console.ReadKey();
    //            Console.WriteLine("Stoped");
    //        };
    //    }
    //}

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.Use(async(enviornment, next) =>
            //{
            //    foreach (var Pair in enviornment.Environment)
            //    {
            //        Console.WriteLine("{0}:{1}", Pair.Key, Pair.Value);
            //    }
            //    await next();
            //});

            app.Use(async (enviornment, next) =>
            {
                Console.WriteLine("Requesting :" + enviornment.Request.Path);

                await next();

                Console.WriteLine("Response :" + enviornment.Response.StatusCode);
            });

            ConfigureWebApi(app);

            app.UseHelloWorld();
            //app.UseWelcomePage();
            //app.Run(ctx =>
            //{
            //    return ctx.Response.WriteAsync("Hello World!");
            //});
        }

        private void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            app.UseWebApi(config);
        }
    }

    public static class AppBuilderExtenions
    {
        public static void UseHelloWorld(this IAppBuilder app)
        {
            app.Use<HelloWorldComponent>();
        }
    }

    public class HelloWorldComponent
    {
        private AppFunc _next;

        public HelloWorldComponent(AppFunc next)
        {
            _next = next;
        }
        public Task Invoke(IDictionary<string, object> enviornment)
        {
            var responce = enviornment["owin.ResponseBody"] as Stream;
            using (var writer = new StreamWriter(responce))
            {
                return writer.WriteAsync("Hello!!!");
            }
        }
    }
}
