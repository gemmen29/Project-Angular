using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [Table("Product")]
    public class Product
    {
       
        public int ID { get; set; }
        [Required]
        [MinLength(5)]
        //[RegularExpression("[a-zA-Z]{5,}", ErrorMessage = "Name must be only characters and more that 5")]
        public string Name { get; set; }

     
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid price")]
        public double Price { get; set; } //make it double instead of nullable

        [Required]
        [MinLength(10)]
        public string Description { get; set; }
   
        public string Color { get; set; }


        [Required]
        [Range(5, int.MaxValue, ErrorMessage = "Discout Must be more than 5")]
        public double Discount{ get; set; }

       
        public string image { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity Must be more than 1")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Category")]
        [ForeignKey("Category")]
        public Nullable<int> CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<ProductCart> Carts { get; set; } = new List<ProductCart>();
        public virtual List<ProductWishList> Wishlists { get; set; } = new List<ProductWishList>();

        public virtual List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        public virtual List<Reviews> Reviews { get; set; } = new List<Reviews>();


    }
}
