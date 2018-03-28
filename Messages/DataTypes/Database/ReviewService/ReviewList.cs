using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.DataTypes.Database.ReviewService
{
    [Serializable]
    public partial class ReviewList
    {
        public List<ReviewInstance> List { get; set; }
    }
}
