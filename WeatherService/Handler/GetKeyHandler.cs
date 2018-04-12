using System.Collections.Generic;
using System.Linq;
using System.Text;
using Messages.ServiceBusRequest;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;
using Messages.ServiceBusRequest.ReviewService.Requests;
using Messages.ServiceBusRequest.ReviewService.Responses;
using Messages.DataTypes.Database.ReviewService;
using System.Net.Http;
using Messages;
using System.Web.Script.Serialization;
using Messages.ServiceBusRequest.WeatherService.Requests;
using Newtonsoft.Json.Linq;

namespace WeatherService.Handler
{
    class GetKeyHandler : IHandleMessages<GetCityKeyRequest>
    {
        private static HttpClient client = new HttpClient();
        
        static ILog log = LogManager.GetLogger<GetCityKeyRequest>();

        public async Task Handle(GetCityKeyRequest message, IMessageHandlerContext context)
        {
            await HandleAsync(message, context);
        }

        public async Task HandleAsync(GetCityKeyRequest message, IMessageHandlerContext context)
        {
            bool success = false;
            string keyReturn;

            Debug.consoleMsg("GET KEY FOR THIS ADDRESS: " + message.address);
            //Search the company with the name
            try
            {
                //var rString = await client.GetStringAsync("http://dataservice.accuweather.com/locations/v1/cities/search?apikey=HuJUKHT8RNpbLRQc59J0YueEkurRjc9c&q=" + message.address);
                var rString = await client.GetStringAsync("http://dataservice.accuweather.com/locations/v1/cities/search?apikey=WBa1oSdQcnmCV8CoeT7yQVK8ggY3y8BQ&q=" + message.address);
                Debug.consoleMsg("RESPONSE: " + rString);
                dynamic x = JArray.Parse(rString);
                keyReturn = x[0].Key;
                success = true;
            }
            catch (Exception)
            {
                success = false;
                keyReturn = "Request Failed";
            }

            ServiceBusResponse response = new ServiceBusResponse(success, keyReturn);
            await context.Reply(response);
        }
    }
}
