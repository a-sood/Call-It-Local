using Messages.DataTypes.Database.ReviewService;
using System;

namespace Messages.ServiceBusRequest.ReviewService.Responses
{
    [Serializable]
    public class GetCompanyReviewsResponse : ServiceBusResponse
    {
        public GetCompanyReviewsResponse(bool result, string response, ReviewList list)
            : base(result, response)
        {
            this.List = list;
        }

        /// <summary>
        /// A list of companies matching the search criteria given by the client
        /// </summary>
        public ReviewList List;
    }
}
