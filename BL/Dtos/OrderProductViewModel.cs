using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Dtos
{
    public class OrderProductViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Total Price")]
        public double productTotal { get; set; }
        [Display(Name = "Discount")]
        public double productDiscount { get; set; }
        [Display(Name = "Net Price")]
        public double ProductNetPrice { get; set; }
        [Display(Name = "Quantity")]
        public int productQuantity { get; set; }
        public int ProductID { get; set; }
        public int orderID { get; set; }
       //public  Product prodct { get; set; }
       //public ProductDto productDto { get; set; }
       public string productName { get; set; }
    }
    public class ProductDto
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
        public double Discount { get; set; }


        public string image { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity Must be more than 1")]
        public int Quantity { get; set; }
    }
   
}
