using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.ServiceBusRequest.WeatherService.Requests
{
    [Serializable]
    public class GetCityKeyRequest : WeatherServiceRequest
    {

        public string address;

        public GetCityKeyRequest(string address)
            : base(WeatherRequest.GetCityKey)
        {
            this.address = address;
        }

    }
}
