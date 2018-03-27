using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.ServiceBusRequest.ReviewService.Requests
{
    public class GetCompanyReviewsRequest : ReviewServiceRequest
    {
        public GetCompanyReviewsRequest(string companyName)
            : base(ReviewRequest.GetReviews)
        {
            this.companyName = companyName;
        }

        /// <summary>
        /// Information used to search the database for companies
        /// </summary>
        public string companyName;
    }
}
