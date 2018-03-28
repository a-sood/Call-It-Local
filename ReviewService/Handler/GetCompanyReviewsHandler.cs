﻿using System.Collections.Generic;
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


namespace ReviewService.Handler
{
    class GetCompanyReviewsHandler : IHandleMessages<GetCompanyReviewsRequest>
    {
        private static HttpClient client = new HttpClient();
        /// <summary>
        /// This is a class provided by NServiceBus. Its main purpose is to be use log.Info() instead of Messages.Debug.consoleMsg().
        /// When log.Info() is called, it will write to the console as well as to a log file managed by NServiceBus
        /// </summary>
        /// It is important that all logger member variables be static, because NServiceBus tutorials warn that GetLogger<>()
        /// is an expensive call, and there is no need to instantiate a new logger every time a handler is created.
        static ILog log = LogManager.GetLogger<GetCompanyReviewsRequest>();

        /// <summary>
        /// Saves the echo to the database, reverses the data, and returns it back to the calling endpoint.
        /// </summary>
        /// <param name="message">Information about the echo</param>
        /// <param name="context">Used to access information regarding the endpoints used for this handle</param>
        /// <returns>The response to be sent back to the calling process</returns>
        public async Task Handle(GetCompanyReviewsRequest message, IMessageHandlerContext context)
        {
            //Search the company with the name

            var rString = await client.GetStringAsync("http://35.230.15.112//Reviews/GetCompanyReviews/{\"companyName\":\"" + message.companyName +"\"}");
            /** ADD CODE HERE  --- Anil **/
            ReviewList list = new ReviewList();
            list.List = new List<ReviewInstance>();
            GetCompanyReviewsResponse response = new GetCompanyReviewsResponse(false, "Demo", list);
            
            /*******/


            await context.Reply(response);
        }
    }
}