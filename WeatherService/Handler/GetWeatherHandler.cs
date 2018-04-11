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
using Newtonsoft.Json;
using Messages.DataTypes.Database.Weather;
using Messages.ServiceBusRequest.WeatherService.Responses;

namespace WeatherService.Handler
{
    class GetWeatherHandler : IHandleMessages<GetWeatherRequest>
    {
        private static HttpClient client = new HttpClient();

        static ILog log = LogManager.GetLogger<GetWeatherRequest>();

        public async Task Handle(GetWeatherRequest message, IMessageHandlerContext context)
        {
            await HandleAsync(message, context);
        }

        public async Task HandleAsync(GetWeatherRequest message, IMessageHandlerContext context)
        {
            bool success = false;
            string returnMessage;
            WeatherInstance weather = null;
            Debug.consoleMsg("GET CuRRENT CONDITIONS FOR THIS KEY: " + message.cityKey);
            try
            {
                var rString = await client.GetStringAsync("http://dataservice.accuweather.com/currentconditions/v1/" + message.cityKey + "?apikey=HuJUKHT8RNpbLRQc59J0YueEkurRjc9c");
                Debug.consoleMsg("RESPONSE: " + rString);
                List<WeatherInstance> weatherList = JsonConvert.DeserializeObject<List<WeatherInstance>>(rString);
                if (weatherList.Count > 0)
                {
                    weather = weatherList.ElementAt(0);
                    success = true;
                    returnMessage = "returnMessage";
                }
                else
                {
                    returnMessage = "Request Failed";
                }
            }
            catch (Exception e)
            {
                success = false;
                returnMessage = "Request Failed";
            }

            GetWeatherResponse response = new GetWeatherResponse(success, returnMessage, weather);
            await context.Reply(response);
        }
    }
}
