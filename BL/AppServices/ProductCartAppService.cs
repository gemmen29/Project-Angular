using BL.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Dtos;
using DAL.Models;
using BL.Interfaces;

namespace BL.AppServices
{
     public class ProductCartAppService: AppServiceBase
    {
        public ProductCartAppService(IUnitOfWork theUnitOfWork) : base(theUnitOfWork)
        {

        }
        public List<ProductCartViewModel> GetAllProductCart()
        {

            return Mapper.Map<List<ProductCartViewModel>>(TheUnitOfWork.ProductCart.GetAllProductCart());
        }
   
        public bool SaveNewProductCart(ProductCartViewModel productCartViewModel)
        {
            if (productCartViewModel == null)
                throw new ArgumentNullException();
            bool result = false;
            var productCart = Mapper.Map<ProductCart>(productCartViewModel);
            if (TheUnitOfWork.ProductCart.Insert(productCart))
            {
                result = TheUnitOfWork.Commit() > new int();
            }
            return result;
        }
        public bool DeleteProductCart(int id)
        {
            if(id<=0)
                throw new InvalidOperationException();
            bool result = false;

            TheUnitOfWork.ProductCart.Delete(id);
            result = TheUnitOfWork.Commit() > new int();

            return result;
        }


    }
}
