using BL.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.ViewModels;
using DAL.Models;
using BL.Interfaces;

namespace BL.AppServices
{
    public class ReviewsAppService: AppServiceBase
    {
        public ReviewsAppService(IUnitOfWork theUnitOfWork) : base(theUnitOfWork)
        {

        }
        public ReviewsViewModel getproductReview(string userID,int prodID)
        {
            return Mapper.Map<ReviewsViewModel>(TheUnitOfWork.Review.GetReview(userID,prodID));
        }
        private bool SaveNewReview(ReviewsViewModel reviewsViewModel)
        {

            bool result = false;
            var review = Mapper.Map<Reviews>(reviewsViewModel);
            if (TheUnitOfWork.Review.Insert(review))
            {
                result = TheUnitOfWork.Commit() > new int();
            }
            return result;
        }
        private bool UpdateReview(Reviews review,ReviewsViewModel reviewsViewModel)
        {
            //var review = Mapper.Map<Reviews>(reviewsViewModel);
            Mapper.Map(reviewsViewModel, review);
            TheUnitOfWork.Review.Update(review);
            TheUnitOfWork.Commit();

            return true;
        }
        public  bool AddOrUpdateReview(ReviewsViewModel reviewsViewModel)
        {
            if (reviewsViewModel == null)
                throw new ArgumentNullException();
            //check if review exist or not 
            //if exist update it else add new 
            var  review = TheUnitOfWork.Review.GetReview(reviewsViewModel.userID, reviewsViewModel.productID);
            if (review != null)
            {
                reviewsViewModel.ID = review.ID;
                UpdateReview(review,reviewsViewModel);
            }
                
            else
            {
                SaveNewReview(reviewsViewModel);
            }
           
            return true;
        }

    }
}
