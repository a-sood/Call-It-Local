using Messages.DataTypes.Database.Weather;
using System;
using System.Collections.Generic;

namespace Messages.ServiceBusRequest.WeatherService.Responses
{
    [Serializable]
    public class GetWeatherResponse : ServiceBusResponse
    {
        public GetWeatherResponse(bool result, string response, WeatherInstance weather)
            : base(result, response)
        {
            this.Weather = weather;
        }

        /// <summary>
        /// A list of companies matching the search criteria given by the client
        /// </summary>
        public WeatherInstance Weather;
    }
}
