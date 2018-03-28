using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.DataTypes.Database.ReviewService
{
    [Serializable]
    public class ReviewInstance
    {
        public ReviewInstance() { }

        public ReviewInstance(string company, string content, int stars, string time, string username)
        {
            this.CompanyName = company;
            this.Review = content;
            this.Stars = stars;
            this.TimeStamp = time;
            this.UserName = username;
        }

        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string Review { get; set; }
        public int Stars { get; set; }
        public string TimeStamp { get; set; }

    }
}
