using PlanX.Core;
using PlanX.Core.Caching;
using PlanX.Core.Data;
using PlanX.Core.Domain.Banner;
using PlanX.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nop.Services.Directory
{
    public partial class BannerService : IBannerService
    {

        #region Constants
        private const string BANNERS_BY_ID_KEY = "Nop.Banner.id-{0}";
        private const string BANNERS_PATTERN_KEY = "Nop.Banner.";
        #endregion

        #region Fields

        private readonly IRepository<Banner> _bannerRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="stateProvinceRepository">State/province repository</param>
        /// <param name="eventPublisher">Event published</param>
        public BannerService(ICacheManager cacheManager,
            IRepository<Banner> BannerRepository,
            IEventPublisher eventPublisher,
            IWorkContext workContext
            )
        {
            _cacheManager = cacheManager;
            _bannerRepository = BannerRepository;
            _eventPublisher = eventPublisher;
            _workContext = workContext;
        }

        #endregion

        #region Insert ,Update ,Delete

        /// <summary>
        /// Inserts a Banner
        /// </summary>
        /// <param name="Banner">Banner</param>
        public virtual void InsertBanner(Banner banner)
        {
            if (banner == null)
                throw new ArgumentNullException("Banner");

            _bannerRepository.Insert(banner);

            //cache
            _cacheManager.RemoveByPattern(BANNERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(banner);
        }

        /// <summary>
        /// Updates the Banner
        /// </summary>
        /// <param name="Banner">Banner</param>
        public virtual void UpdateBanner(Banner banner)
        {
            if (banner == null)
                throw new ArgumentNullException("Banner");

            _bannerRepository.Update(banner);

            //cache
            _cacheManager.RemoveByPattern(BANNERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(banner);
        }

        /// <summary>
        /// Delete a Banner
        /// </summary>
        /// <param name="Banner">A Banner</param>
        public virtual void DeleteBanner(Banner banner)
        {
            if (banner == null)
                throw new ArgumentNullException("Banner");

            _bannerRepository.Delete(banner);

            //cache
            _cacheManager.RemoveByPattern(BANNERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(banner);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all Banners
        /// </summary>
        /// <returns>Banner collection</returns>
        public virtual IList<Banner> GetAllFBanners(bool showHidden = false)
        {
            return GetAllBanners(0, 0, showHidden);
        }
        /// <summary>
        /// Gets all Banners
        /// </summary>
        /// <returns>Banner collection</returns>
        public virtual async Task<IList<Banner>> GetAllFBannersAsync(bool showHidden = false)
        {
            return await GetAllBannersAsync(0, 0, showHidden);
        }
        /// <summary>
        /// Get all banner allow position, store
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="storeId">Store Id</param>
        /// <param name="showHidden">Show Hidden</param>
        /// <returns>Ilist Banner</returns>
        public virtual IList<Banner> GetAllBanners(int position = 0, int storeId = 0, bool showHidden = false)
        {
          
                var query = _bannerRepository.Table;

                if (position > 0)
                    query = query.Where(b => b.Position == position);

                if (!showHidden)
                    query = query.Where(m => m.Published);

                query = query.OrderBy(m => m.DisplayOrder);

                var banners = query.ToList();
                return banners;
            
        }
        /// <summary>
        /// Get all banner allow position, store
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="storeId">Store Id</param>
        /// <param name="showHidden">Show Hidden</param>
        /// <returns>Ilist Banner</returns>
        public virtual async Task<IList<Banner>> GetAllBannersAsync(int position = 0, int storeId = 0, bool showHidden = false)
        {
            return await Task.Factory.StartNew<IList<Banner>>(() =>
            {
               
                    var query = _bannerRepository.Table;

                    if (position > 0)
                        query = query.Where(b => b.Position == position);

                    if (!showHidden)
                        query = query.Where(m => m.Published);

                    query = query.OrderBy(m => m.DisplayOrder);

                    var banners = query.ToList();
                    return banners;
                
            });
        }
        /// <summary>
        /// Gets a Banner
        /// </summary>
        /// <param name="BannerId">Banner identifier</param>
        /// <returns>FrtBanner</returns>
        public virtual Banner GetBannerById(int bannerId)
        {
            if (bannerId == 0)
                return null;

            string key = string.Format(BANNERS_BY_ID_KEY, bannerId);
            return _cacheManager.Get(key, () =>
            {
                return _bannerRepository.GetById(bannerId);
            });
        }
        /// <summary>
        /// Gets a Banner
        /// </summary>
        /// <param name="BannerId">Banner identifier</param>
        /// <returns>FrtBanner</returns>
        public virtual async Task<Banner> GetBannerByIdAsync(int bannerId)
        {
            if (bannerId == 0)
                return null;
            return await Task.Factory.StartNew<Banner>(() =>
            {
                string key = string.Format(BANNERS_BY_ID_KEY, bannerId);
                return _cacheManager.Get(key, () =>
                {
                    return _bannerRepository.GetById(bannerId);
                });
            });
        }
        #endregion
    }
}
