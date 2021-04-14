using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [Table("Wishlist")]
    public class Wishlist
    {
        public int ID { get; set; }
        public virtual List<ProductWishList> Wishlists { get; set; } = new List<ProductWishList>();
        [ForeignKey("ApplicationUserIdentity")]
        public string ApplicationUserIdentity_Id { get; set; }
        public virtual ApplicationUserIdentity ApplicationUserIdentity { get; set; }
    }
}
