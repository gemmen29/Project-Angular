using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Bases;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BL.Repositories
{
    public class ReviewsRepository : BaseRepository<Reviews>
    {
        private DbContext EC_DbContext;

        public ReviewsRepository(DbContext EC_DbContext) : base(EC_DbContext)
        {
            this.EC_DbContext = EC_DbContext;
        }
        public Reviews GetReview(string userID,int prodID)
        {
           return GetFirstOrDefault(r => r.productID == prodID && r.userID == userID);
         

        }

        //public List<Cart> GetAllCart()
        //{
        //    return GetAll().ToList();
        //}

        public bool InsertReview(Reviews review)
        {
            return Insert(review);
        }
        //public void UpdateCart(Cart cart)
        //{
        //    Update(cart);
        //}
    }
}
