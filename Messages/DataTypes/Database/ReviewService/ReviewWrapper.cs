using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.DataTypes.Database.ReviewService
{
    public class ReviewWrapper
    {
        public ReviewWrapper(ReviewInstance review)
        {
            this.review = review;
        }

        public ReviewInstance review { get; set; } 
    }
}
