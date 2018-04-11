using Messages.NServiceBus.Commands;
using Messages.NServiceBus.Events;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.CompanyDirectory;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Messages.ServiceBusRequest.Echo;
using Messages.ServiceBusRequest.Echo.Requests;
using Messages.ServiceBusRequest.ReviewService;
using Messages.ServiceBusRequest.ReviewService.Requests;
using Messages.ServiceBusRequest.WeatherService;
using Messages.ServiceBusRequest.WeatherService.Requests;
using NServiceBus;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Communication
{
    partial class ClientConnection
    {
        private ServiceBusResponse weatherRequest(WeatherServiceRequest request)
        {
            switch (request.requestType)
            {
                case (WeatherRequest.GetCityKey):
                    return GetCityKey((GetCityKeyRequest)request);
                case (WeatherRequest.GetWeather):
                    return GetWeather((GetWeatherRequest)request);
                default:
                    return new ServiceBusResponse(false, "Error: Invalid Request. Request received was:" + request.requestType.ToString());
            }
        }

        private ServiceBusResponse GetCityKey(GetCityKeyRequest request)
        {
            if (authenticated == false)
            {
                return new ServiceBusResponse(false, "Error: You must be logged in to use the weather functionality.");
            }

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Weather Service");

            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).
                ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private ServiceBusResponse GetWeather(GetWeatherRequest request)
        {
            if (authenticated == false)
            {
                return new ServiceBusResponse(false, "Error: You must be logged in to use the weather functionality.");
            }

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Weather Service");

            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).
                ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
