using Messages.DataTypes.Database.ReviewService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.ServiceBusRequest.ReviewService.Requests
{
    public class SaveCompanyReviewRequest : ReviewServiceRequest
    {
        public SaveCompanyReviewRequest(ReviewInstance review)
            : base(ReviewRequest.GetReviews)
        {
            this.review = review;
        }

        /// <summary>
        /// Information used to search the database for companies
        /// </summary>
        public ReviewInstance review;
    }
}
