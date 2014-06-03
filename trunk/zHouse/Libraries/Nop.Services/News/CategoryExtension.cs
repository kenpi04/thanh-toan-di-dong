using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Domain.News;

namespace Nop.Services.News
{
    /// <summary>
    /// Represents category news extensions
    /// 
    /// Revision History
    /// Date			Author		                    Reason for Change
    /// -----------------------------------------------------------
    /// 
    /// </summary>
    public static class CategoryExtensions
    {
        /// <summary>
        /// Sort categories news for tree representation
        /// </summary>
        /// <param name="source">Categories news</param>
        /// <param name="parentId">Parent category news identifier</param>
        /// <param name="ignoreCategoriesWithoutExistingParent">A value indicating whether categories without parent category in provided category list (source) should be ignored</param>
        /// <returns>Sorted categories</returns>
        public static IList<CategoryNews> SortCategoriesForTree(this IList<CategoryNews> source, int parentId = 0, bool ignoreCategoriesWithoutExistingParent = false)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var result = new List<CategoryNews>();

            foreach (var cat in source.ToList().FindAll(c => c.ParentCategoryNewsId == parentId))
            {
                result.Add(cat);
                result.AddRange(SortCategoriesForTree(source, cat.Id, true));
            }
            if (!ignoreCategoriesWithoutExistingParent && result.Count != source.Count)
            {
                //find categories without parent in provided category source and insert them into result
                foreach (var cat in source)
                    if (result.Where(x => x.Id == cat.Id).FirstOrDefault() == null)
                        result.Add(cat);
            }
            return result;
        }

        /// <summary>
        /// Get bread crumb of category news
        /// </summary>
        /// <param name="category">A CategoryNews</param>
        /// <param name="categoryService">ICategoryNewsService</param>
        /// <returns>A string bread crumb</returns>
        public static string GetCategoryBreadCrumb(this CategoryNews category, ICategoryNewsService categoryService)
        {
            string result = string.Empty;

            while (category != null && !category.Deleted)
            {
                if (String.IsNullOrEmpty(result))
                    result = category.Name;
                else
                    result = category.Name + " >> " + result;

                category = categoryService.GetCategoryById(category.ParentCategoryNewsId);

            }
            return result;
        }
    }

}
