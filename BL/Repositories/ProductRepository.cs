﻿using BL.Bases;
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

        public IEnumerable<Product> GetAllProduct()
        {
            return GetAll()
                .Include(p=>p.Color)
                .Include(p => p.Category)
                .ToList();
        }
        public IEnumerable<Product> GetNewArrivalsProduct(int numberOfProducts = 0)
        {
            IEnumerable<Product> newArivailsProducts;
            if (numberOfProducts <= 0)
                newArivailsProducts = DbSet.OrderByDescending(p => p.ID);
            else
                newArivailsProducts = DbSet.OrderByDescending(p => p.ID).Take(numberOfProducts);

            return newArivailsProducts;
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
            var product = DbSet
                .Include(p => p.Color)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ID == id);
            return product;
        }

        internal IEnumerable<Product> GetRandomRelatedProducts(int categoryId, int numberOfProducts)
        {
            var query = DbSet
                    .Include(p => p.Color)
                    .Include(p => p.Category)
                    .Where(p => p.CategoryId == categoryId)
                    .OrderBy(p => Guid.NewGuid())
                    .Take(numberOfProducts);
            var x = query.ToQueryString();
            return query;
        }
        #endregion

        public override IEnumerable<Product> GetPageRecords(int pageSize, int pageNumber)
        {
            pageSize = (pageSize <= 0) ? 10 : pageSize;
            pageNumber = (pageNumber < 1) ? 0 : pageNumber - 1;

            var products =  DbSet
                .Skip(pageNumber * pageSize).Take(pageSize)
                .Include(p => p.Color)
                .Include(p => p.Category)
                .Include(p=> p.Reviews.Select(r=>r.Rating)) // 
                .ToList();
            return products;
        }
        public int CountProducts(int categoryId = 0, int colorId = 0)
        {
            if(categoryId != 0)
            {
                return DbSet.Where(p => p.CategoryId == categoryId).Count();
            }
            if (colorId != 0)
            {
                return DbSet.Where(p => p.ColorId == colorId).Count();
            }
            return DbSet.Count();
        }
    }
}
