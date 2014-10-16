using System.Collections.Generic;
using Nop.Core.Domain.Catalog;
using System.Threading.Tasks;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Compare products service interface
    /// </summary>
    public partial interface ICompareProductsService
    {
        /// <summary>
        /// Clears a "compare products" list
        /// </summary>
        void ClearCompareProducts();
        Task ClearCompareProductsAsync();

        Task<List<int>> GetComparedProductIdsAsync();

        /// <summary>
        /// Gets a "compare products" list
        /// </summary>
        /// <returns>"Compare products" list</returns>
        IList<Product> GetComparedProducts();
        Task<IList<Product>> GetComparedProductsAsync();

        /// <summary>
        /// Removes a product from a "compare products" list
        /// </summary>
        /// <param name="productId">Product identifier</param>
        void RemoveProductFromCompareList(int productId);
        Task RemoveProductFromCompareListAsync(int productId);

        /// <summary>
        /// Adds a product to a "compare products" list
        /// </summary>
        /// <param name="productId">Product identifier</param>
        void AddProductToCompareList(int productId);
        Task AddProductToCompareListAsync(int productId);
    }
}
