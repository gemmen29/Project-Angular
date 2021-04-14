using BL.Bases;
using BL.Interfaces;
using BL.ViewModels;
using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.AppServices
{
   public class CategoryAppService:AppServiceBase
    {
        public CategoryAppService(IUnitOfWork theUnitOfWork) : base(theUnitOfWork)
        {

        }
        #region CURD

        public List<CategoryViewModel> GetAllCateogries()
        {

            return Mapper.Map<List<CategoryViewModel>>(TheUnitOfWork.Category.GetAllCategory());
        }
        public CategoryViewModel GetCategory(int id)
        {
            return Mapper.Map<CategoryViewModel>(TheUnitOfWork.Category.GetById(id));
        }



        public bool SaveNewCategory(CategoryViewModel categoryViewModel)
        {
              if (categoryViewModel == null)
              
  throw new ArgumentNullException();

            bool result = false;
            var category = Mapper.Map<Category>(categoryViewModel);
            if (TheUnitOfWork.Category.Insert(category))
            {
                result = TheUnitOfWork.Commit() > new int();
            }
            return result;
        }


        public bool UpdateCategory(CategoryViewModel categoryViewModel)
        {
            var category = Mapper.Map<Category>(categoryViewModel);
            TheUnitOfWork.Category.Update(category);
            TheUnitOfWork.Commit();

            return true;
        }


        public bool DeleteCategory(int id)
        {
            bool result = false;

            TheUnitOfWork.Category.Delete(id);
            result = TheUnitOfWork.Commit() > new int();

            return result;
        }

        public bool CheckCategoryExists(CategoryViewModel categoryViewModel)
        {
            Category category = Mapper.Map<Category>(categoryViewModel);
            return TheUnitOfWork.Category.CheckCategoryExists(category);
        }
        #endregion
    }
}
