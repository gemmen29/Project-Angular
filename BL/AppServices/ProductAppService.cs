using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Bases;
using BL.Interfaces;
using BL.Dtos;
using DAL.Models;

namespace BL.AppServices
{
    public class ProductAppService: AppServiceBase
    {
        public ProductAppService(IUnitOfWork theUnitOfWork) : base(theUnitOfWork)
        {

        }
        public List<ProductViewModel> GetAllProduct()
        {
            var allProducts = TheUnitOfWork.Product.GetAllProduct();

            return Mapper.Map<List<ProductViewModel>>(allProducts);
        }
        public List<ProductViewModel> GetAllProductWhere(int categoryID)
        {
            //    List<Product> products= TheUnitOfWork.Product.GetAllProduct().Where(p => p.Name.Contains(productToSearch)).ToList();
            var searchRes = TheUnitOfWork.Product.GetWhere(p=>p.CategoryId==categoryID, "Reviews");

            return Mapper.Map<List<ProductViewModel>>(searchRes);
        }
        public List<ProductViewModel> GetAllProductWhere( string productToSearch)
        {
            //    List<Product> products= TheUnitOfWork.Product.GetAllProduct().Where(p => p.Name.Contains(productToSearch)).ToList();
            var searchRes = TheUnitOfWork.Product.GetWhere(p => p.Name.Contains(productToSearch), "Reviews");

            return Mapper.Map<List<ProductViewModel>>(searchRes);
        }
        public ProductViewModel GetPoduct(int id)
        {
            return Mapper.Map<ProductViewModel>(TheUnitOfWork.Product.GetProductById(id));
        }



        public bool SaveNewProduct(ProductViewModel productViewModel)
        {
            if (productViewModel == null)
                throw new ArgumentNullException();
            bool result = false;
            var product = Mapper.Map<Product>(productViewModel);
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
        public List<ProductViewModel> SearchFor(string productToSearch)
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
            return Mapper.Map<List<ProductViewModel>>(TheUnitOfWork.Product.GetPageRecords(pageSize, pageNumber));
        }
        #endregion

    }
}
