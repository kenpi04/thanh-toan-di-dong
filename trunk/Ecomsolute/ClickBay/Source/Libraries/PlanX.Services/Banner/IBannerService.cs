using PlanX.Core;
using PlanX.Core.Domain.Banner;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanX.Services.Directory
{
    public partial interface IBannerService
    {
        #region Insert ,Update ,Delete

        /// <summary>
        /// Inserts a Banner
        /// </summary>
        /// <param name="Banner">Banner</param>
        void InsertBanner(Banner banner);

        /// <summary>
        /// Updates the Banner
        /// </summary>
        /// <param name="Banner">Banner</param>
        void UpdateBanner(Banner banner);

        /// <summary>
        /// Delete a Banner
        /// </summary>
        /// <param name="Banner">A Banner</param>
        void DeleteBanner(Banner banner);

        #endregion

        #region Methods

        /// <summary>
        /// Gets all Banners
        /// </summary>
        /// <returns>Banner collection</returns>
        IList<Banner> GetAllFBanners(bool showHidden = false);
        /// <summary>
        /// Gets all Banners
        /// </summary>
        /// <returns>Banner collection</returns>
        Task<IList<Banner>> GetAllFBannersAsync(bool showHidden = false);
        /// <summary>
        /// Get all banner allow position, storeId
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="storeId">Store Id</param>
        /// <param name="showHidden">Show Hidden</param>
        /// <returns>Banner collection</returns>
        IList<Banner> GetAllBanners(int position = 0, int storeId = 0, bool showHidden = false);
        /// <summary>
        /// Get all banner allow position, storeId
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="storeId">Store Id</param>
        /// <param name="showHidden">Show Hidden</param>
        /// <returns>Banner collection</returns>
        Task<IList<Banner>> GetAllBannersAsync(int position = 0, int storeId = 0, bool showHidden = false);
        /// <summary>
        /// Gets a Banner
        /// </summary>
        /// <param name="BannerId">Banner identifier</param>
        /// <returns>FrtBanner</returns>
        Banner GetBannerById(int bannerId);
        /// <summary>
        /// Gets a Banner
        /// </summary>
        /// <param name="BannerId">Banner identifier</param>
        /// <returns>FrtBanner</returns>
        Task<Banner> GetBannerByIdAsync(int bannerId);
        #endregion
    }
}
