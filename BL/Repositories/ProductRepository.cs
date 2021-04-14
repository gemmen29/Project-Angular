using BL.Bases;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class ProductRepository: BaseRepository<Product>
    {

        private DbContext EC_DbContext;

        public ProductRepository(DbContext EC_DbContext) : base(EC_DbContext)
        {
            this.EC_DbContext = EC_DbContext;
        }
        #region CRUB

        public List<Product> GetAllProduct()
        {
            return GetAll().Include(p=>p.Reviews).ToList();
        }
      

        public bool InsertProduct(Product product)
        {
            return Insert(product);
        }
        public void UpdateProduct(Product product)
        {
            Update(product);
        }
        public void DeleteProduct(int id)
        {
            Delete(id);
        }

        public bool CheckProductExists(Product product)
        {
            return GetAny(l => l.ID== product.ID);
        }
        public Product GetProductById(int id)
        {
            return GetFirstOrDefault(l => l.ID == id);
        }
        #endregion
    }
}
