using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text;

namespace BookstoreAPI.Filters
{
    public class IPLocationLookupFilter : IActionFilter
    {
        private readonly IConfiguration _configuration;

        public IPLocationLookupFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string customLookupIP = _configuration["CustomLookupIP"];
            string allowedCountry = _configuration["AllowedCountry"];

            // Get remote IP address or a custom IP address based on the environment
            string remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            if (customLookupIP != null) remoteIpAddress = customLookupIP;

            // Get country code of the IP address from IPAPI
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://ipapi.co/" + remoteIpAddress + "/country/");
            request.UserAgent="ipapi.co/#c-sharp-v1.03";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var reader = new System.IO.StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);

            // Block requests from unauthorized countries
            if (reader.ReadToEnd() != allowedCountry)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context){}
    }

    public class IPLocationLookupAttribute : TypeFilterAttribute
    {
        public IPLocationLookupAttribute() : base(typeof(IPLocationLookupFilter)){}
    }
}
