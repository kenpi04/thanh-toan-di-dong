﻿using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.News;

namespace Nop.Services.News
{
    public partial interface ICategoryNewsService
    {
        // Create by Hung Lai 2012-09-12
        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="category">Category</param>
        void DeleteCategory(CategoryNews category);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        IList<CategoryNews> GetAllCategories(bool showHidden = false);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        IList<CategoryNews> GetAllCategories(string categoryName, bool showHidden = false);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        IPagedList<CategoryNews> GetAllCategories(string categoryName, int pageIndex, int pageSize, bool showHidden = false);

        /// <summary>
        /// Gets all categories filtered by parent category identifier
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Category collection</returns>
        IList<CategoryNews> GetAllCategoriesByParentCategoryId(int parentCategoryId,
            bool showHidden = false);

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>Category</returns>
        CategoryNews GetCategoryById(int categoryId);

        /// <summary>
        /// Inserts category
        /// </summary>
        /// <param name="category">Category</param>
        void InsertCategory(CategoryNews category);

        /// <summary>
        /// Updates the category
        /// </summary>
        /// <param name="category">Category</param>
        void UpdateCategory(CategoryNews category);

        /// <summary>
        /// Deletes a news category mapping
        /// </summary>
        /// <param name="NewsCategory">News category</param>
        void DeleteNewsCategory(NewsCategoryNews newsCategory);

        /// <summary>
        /// Gets news category mapping collection
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>News a category mapping collection</returns>
        IPagedList<NewsCategoryNews> GetNewsCategoriesByCategoryId(int categoryId,
            int pageIndex, int pageSize, bool showHidden = false);

        /// <summary>
        /// Gets a News category mapping collection
        /// </summary>
        /// <param name="productId">News identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>News category mapping collection</returns>
        IList<NewsCategoryNews> GetNewsCategoriesByNewsId(int newsId, bool showHidden = false);

        /// <summary>
        /// Gets a news category mapping 
        /// </summary>
        /// <param name="NewsCategoryId">Product category mapping identifier</param>
        /// <returns>Product category mapping</returns>
        NewsCategoryNews GetNewsCategoryById(int NewsCategoryId);

        /// <summary>
        /// Inserts a News category mapping
        /// </summary>
        /// <param name="productCategory">>News category mapping</param>
        void InsertNewsCategory(NewsCategoryNews newsCategory);

        /// <summary>
        /// Updates the news category mapping 
        /// </summary>
        /// <param name="newsCategory">>News category mapping</param>
        void UpdateNewsCategory(NewsCategoryNews newsCategory);

        IList<CategoryNews> GetAll();
    }

}
