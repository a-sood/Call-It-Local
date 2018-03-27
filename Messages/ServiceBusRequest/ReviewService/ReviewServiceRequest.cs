using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.ServiceBusRequest.ReviewService
{
    [Serializable]
    public class ReviewServiceRequest : ServiceBusRequest
    {
        public ReviewServiceRequest(ReviewRequest requestType)
            : base(Service.ReviewService)
        {
            this.requestType = requestType;
        }

        /// <summary>
        /// Indicates the type of request the client is seeking from the Company Directory Service
        /// </summary>
        public ReviewRequest requestType;
    }

    public enum ReviewRequest { GetReviews, SaveReview };
}
