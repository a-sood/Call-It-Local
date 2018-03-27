using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
    
namespace Messages.DataTypes.Database.ReviewService
{
    [Serializable]
    public partial class ReviewInstance
    {

        public ReviewInstance(string companyName, string username, string description, int timestamp, int stars)
        {
            this.CompanyName = companyName;
            this.UserName = username;
            this.Description = description;
            this.Timestamp = timestamp;
            this.Stars = stars;
        }

    }

    public partial class ReviewInstance
    {
    
        public String Description { get; set; } = null;

        public int Stars { get; set; } = 0;

  
        public int Timestamp { get; set; } = 0;

        public String CompanyName { get; set; } = null;

        public String UserName { get; set; } = null;
    }
}
