using System.Collections.Generic;
using Nop.Core.Domain.Catalog;
using System.Threading.Tasks;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Recently viewed products service
    /// </summary>
    public partial interface IRecentlyViewedProductsService
    {
        /// <summary>
        /// Gets a "recently viewed products" list
        /// </summary>
        /// <param name="number">Number of products to load</param>
        /// <returns>"recently viewed products" list</returns>
        IList<Product> GetRecentlyViewedProducts(int number);
        Task<IList<Product>> GetRecentlyViewedProductsAsync(int number);

        /// <summary>
        /// Adds a product to a recently viewed products list
        /// </summary>
        /// <param name="productId">Product identifier</param>
        void AddProductToRecentlyViewedList(int productId);
        void AddProductToRecentlyViewedListAsync(int productId);
    }
}
