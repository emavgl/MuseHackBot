using Bot_Application1.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;

namespace Bot_Application1
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static Dictionary<string, CustomMessages> BotStrings { get; set; }
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            BotStrings = JsonConvert.DeserializeObject<Dictionary<string, CustomMessages>>(File.ReadAllText(HttpContext.Current.Server.MapPath("~/bot.json")));
        }
    }
}
