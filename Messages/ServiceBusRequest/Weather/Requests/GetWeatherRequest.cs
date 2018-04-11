using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.ServiceBusRequest.WeatherService.Requests
{
    [Serializable]
    public class GetWeatherRequest  : WeatherServiceRequest
    {
        public string cityKey;

        public GetWeatherRequest(string cityKey)
            : base(WeatherRequest.GetWeather)
        {
            this.cityKey = cityKey;
        }
    }
}
