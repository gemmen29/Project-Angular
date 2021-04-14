using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.ViewModels
{
    public class ReviewsViewModel
    {
        public int ID { get; set; }
        public string Description { get; set; }

        [Range(1, 5)]
        public int rating { get; set; }

       
        public int productID { get; set; }
  

       
        public string userID { get; set; }
       
    }
}
