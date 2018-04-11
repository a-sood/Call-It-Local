using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.ServiceBusRequest.WeatherService
{
    [Serializable]
    public class WeatherServiceRequest : ServiceBusRequest
    {
        public WeatherServiceRequest(WeatherRequest requestType)
            : base(Service.Weather)
        {
            this.requestType = requestType;
        }

        /// <summary>
        /// Indicates the type of request the client is seeking from the Weather Service
        /// </summary>
        public WeatherRequest requestType;
    }

    public enum WeatherRequest { GetCityKey, GetWeather };
}
