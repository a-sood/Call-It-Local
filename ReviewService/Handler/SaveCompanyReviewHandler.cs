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
using System.Web.Script.Serialization;
using Messages;
using System.Net.Http;

namespace ReviewService.Handler
{
    class SaveCompanyReviewHandler : IHandleMessages<SaveCompanyReviewRequest>
    {
        private static HttpClient client = new HttpClient();

        /// <summary>
        /// This is a class provided by NServiceBus. Its main purpose is to be use log.Info() instead of Messages.Debug.consoleMsg().
        /// When log.Info() is called, it will write to the console as well as to a log file managed by NServiceBus
        /// </summary>
        /// It is important that all logger member variables be static, because NServiceBus tutorials warn that GetLogger<>()
        /// is an expensive call, and there is no need to instantiate a new logger every time a handler is created.
        static ILog log = LogManager.GetLogger<SaveCompanyReviewRequest>();

        public async Task Handle(SaveCompanyReviewRequest message, IMessageHandlerContext context)
        {
            await HandleAsync(message, context);
        }

        //public Task Handle(SaveCompanyReviewRequest message, IMessageHandlerContext context)
        //{
        //    return HandleAsync(message, context);
        //}

        /// <summary>
        /// Saves the echo to the database, reverses the data, and returns it back to the calling endpoint.
        /// </summary>
        /// <param name="message">Information about the echo</param>
        /// <param name="context">Used to access information regarding the endpoints used for this handle</param>
        /// <returns>The response to be sent back to the calling process</returns>
        public async Task HandleAsync(SaveCompanyReviewRequest message, IMessageHandlerContext context)
        {
            Debug.consoleMsg("SAVE REVIEW: " + message.review.CompanyName);
            //Save the company review

            ServiceBusResponse response = new ServiceBusResponse(false, "Demo");

            ReviewWrapper review = new ReviewWrapper(message.review);

            var serializer = new JavaScriptSerializer();
            var json_review = serializer.Serialize(review);
            Debug.consoleMsg("JSON: \n" + json_review);

            StringContent post_content = new StringContent(json_review, Encoding.UTF8, "application/json");
            Debug.consoleMsg("POST CONTENT:" + post_content.ToString());

            var httpresponse = await client.PostAsync("http://35.230.15.112/Reviews/SaveCompanyReview/", post_content);
            Debug.consoleMsg(httpresponse.RequestMessage.ToString());

            var responseString = await httpresponse.Content.ReadAsStringAsync();
            Debug.consoleMsg("RESPONSE:\n" + responseString);

            await context.Reply(response);
        }
    }
}