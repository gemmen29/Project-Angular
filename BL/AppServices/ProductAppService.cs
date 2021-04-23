﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Bases;
using BL.Interfaces;
using BL.Dtos;
using DAL.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace BL.AppServices
{
    public class ProductAppService: AppServiceBase
    {
        public ProductAppService(IUnitOfWork theUnitOfWork, IMapper mapper) : base(theUnitOfWork, mapper)
        {
        }
        public IEnumerable<ProductViewModel> GetAllProduct()
        {
            IEnumerable<Product> allProducts = TheUnitOfWork.Product.GetAllProduct();
            return Mapper.Map<IEnumerable<ProductViewModel>>(allProducts);
        }
        public IEnumerable<ProductViewModel> GetNewArrivalsProduct(int numberOfProducts = 0)
        {
            IEnumerable<Product> allProducts = 
                TheUnitOfWork.Product.GetNewArrivalsProduct(numberOfProducts);

            return Mapper.Map<IEnumerable<ProductViewModel>>(allProducts);
        }
        public List<ProductViewModel> GetAllProductWhere(int categoryID)
        {
            //    List<Product> products= TheUnitOfWork.Product.GetAllProduct().Where(p => p.Name.Contains(productToSearch)).ToList();
            var searchRes = TheUnitOfWork.Product.GetWhere(p=>p.CategoryId==categoryID, "Reviews");

            return Mapper.Map<List<ProductViewModel>>(searchRes);
        }

        public IEnumerable<ProductViewModel> GetProductsByCategoryIdPagination(int catId, int pageSize, int pageNumber)
        {
            pageSize = (pageSize <= 0) ? 10 : pageSize;
            pageNumber = (pageNumber < 1) ? 0 : pageNumber - 1;
            var products = TheUnitOfWork.Product.GetWhere(p => p.CategoryId == catId)
                .Skip(pageNumber * pageSize).Take(pageSize)
                .Include(p => p.Color)
                .Include(p => p.Category)
                .ToList(); ;

            return Mapper.Map<List<ProductViewModel>>(products);
        }

        public IEnumerable<ProductViewModel> GetAllProductWhere( string productToSearch)
        {
            //    List<Product> products= TheUnitOfWork.Product.GetAllProduct().Where(p => p.Name.Contains(productToSearch)).ToList();
            var searchRes = TheUnitOfWork.Product.GetWhere(p => p.Name.Contains(productToSearch), "Reviews");

            return Mapper.Map<List<ProductViewModel>>(searchRes);
        }
        public ProductViewModel GetProduct(int id)
        {
            return Mapper.Map<ProductViewModel>(TheUnitOfWork.Product.GetProductById(id));
        }



        public bool SaveNewProduct(ProductViewModel productViewModel)
        {
            if (productViewModel == null)
                throw new ArgumentNullException();
            bool result = false;
            var product = Mapper.Map<Product>(productViewModel);
            //product.Category = null;
            //product.Color = null;
            if (TheUnitOfWork.Product.Insert(product))
            {
                result = TheUnitOfWork.Commit() > new int();
            }
            return result;
        }


        public bool UpdateProduct(ProductViewModel productViewModel)
        {
            var productFromDb= TheUnitOfWork.Product.GetById(productViewModel.ID);
            if(productViewModel.Image == null)
                productViewModel.Image = productFromDb.Image;
            //var product = Mapper.Map<Product>(productViewModel);
            Mapper.Map(productViewModel, productFromDb);
            TheUnitOfWork.Product.Update(productFromDb);
            TheUnitOfWork.Commit();

            return true;
        }
        public bool DecreaseQuantity(int prodID,int decresedQuantity)
        {
            var product = TheUnitOfWork.Product.GetById(prodID);
            product.Quantity -= decresedQuantity;
            TheUnitOfWork.Product.Update(product);
            TheUnitOfWork.Commit();
            return true;
        }

        public IEnumerable<ProductViewModel> SearchFor(string productToSearch)
        {
            return GetAllProductWhere(productToSearch);
       
           
        }
        public bool DeleteProduct(int id)
        {
            bool result = false;

            TheUnitOfWork.Product.Delete(id);
            result = TheUnitOfWork.Commit() > new int();

            return result;
        }
        public bool CheckProductExists(ProductViewModel productViewModel)
        {
            Product product = Mapper.Map<Product>(productViewModel);
            return TheUnitOfWork.Product.CheckProductExists(product);
        }

        #region pagination
        public int CountEntity()
        {
            return TheUnitOfWork.Product.CountEntity();
        }
        public IEnumerable<ProductViewModel> GetPageRecords(int pageSize, int pageNumber)
        {
            var products = Mapper.Map<List<ProductViewModel>>(TheUnitOfWork.Product.GetPageRecords(pageSize, pageNumber));
            return products;
        }
        #endregion

    }
}
