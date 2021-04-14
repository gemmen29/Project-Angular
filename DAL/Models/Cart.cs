using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [Table("Cart")]
    public class Cart
    {
        public int ID { get; set; }
        public virtual List<ProductCart> Products { get; set; } = new List<ProductCart>();
        [ForeignKey("ApplicationUserIdentity")]
        public string ApplicationUserIdentity_Id { get; set; }
        public virtual ApplicationUserIdentity ApplicationUserIdentity { get; set; }
    }
}
