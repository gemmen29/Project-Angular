using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [Table("Review")]
    public class Reviews
    {
        public int ID { get; set; }
        public string Description { get; set; }

        [Range(1,5)]
        public int rating { get; set; }

        [ForeignKey("product")]
        public int productID { get; set; }
        public virtual Product product { get; set; }

        [ForeignKey("user")]
        public string userID { get; set; }
        public virtual ApplicationUserIdentity user { get; set; }
    }
}
