using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Directory;
using Nop.Services.Events;
using System.Transactions;

namespace Nop.Services.Directory
{
    /// <summary>
    /// State province service
    /// </summary>
    public partial class StateProvinceService : IStateProvinceService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {1} : country ID
        /// </remarks>
        private const string STATEPROVINCES_ALL_KEY = "Nop.stateprovince.all-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string STATEPROVINCES_PATTERN_KEY = "Nop.stateprovince.";

        #endregion

        #region Fields

        private readonly IRepository<StateProvince> _stateProvinceRepository;
        private readonly IRepository<District> _districtRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Ward> _wardRepository;
        private readonly IRepository<Street> _streetRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="stateProvinceRepository">State/province repository</param>
        /// <param name="eventPublisher">Event published</param>
        public StateProvinceService(ICacheManager cacheManager,
            IRepository<StateProvince> stateProvinceRepository,
            IEventPublisher eventPublisher,
            IRepository<District> districtRepository,
            IRepository<Ward> wardRepository,
            IRepository<Street> streetRepository)
        {
            _cacheManager = cacheManager;
            _stateProvinceRepository = stateProvinceRepository;
            _eventPublisher = eventPublisher;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _streetRepository = streetRepository;
        }

        #endregion

        #region Methods

        public virtual IList<District> GetDistHCM(int stateId = 23, bool showHidden = false)
        {
            return _cacheManager.Get(string.Format("Nop.district-{0}-{1}", stateId, showHidden), () =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
                {
                    var query = from d in _districtRepository.Table
                                where (d.StateProvinceId == stateId) &&
                                (showHidden || d.Published)
                                select d;

                    return query.ToList();
                }
            });
          
        }     
            
        /// <summary>
        /// Deletes a state/province
        /// </summary>
        /// <param name="stateProvince">The state/province</param>
        public virtual void DeleteStateProvince(StateProvince stateProvince)
        {
            if (stateProvince == null)
                throw new ArgumentNullException("stateProvince");
            
            _stateProvinceRepository.Delete(stateProvince);

            _cacheManager.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(stateProvince);
        }

        /// <summary>
        /// Gets a state/province
        /// </summary>
        /// <param name="stateProvinceId">The state/province identifier</param>
        /// <returns>State/province</returns>
        public virtual StateProvince GetStateProvinceById(int stateProvinceId)
        {
            if (stateProvinceId == 0)
                return null;

            return _stateProvinceRepository.GetById(stateProvinceId);
        }

        /// <summary>
        /// Gets a state/province 
        /// </summary>
        /// <param name="abbreviation">The state/province abbreviation</param>
        /// <returns>State/province</returns>
        public virtual StateProvince GetStateProvinceByAbbreviation(string abbreviation)
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
            {
                var query = from sp in _stateProvinceRepository.Table
                            where sp.Abbreviation == abbreviation
                            select sp;
                var stateProvince = query.FirstOrDefault();
                return stateProvince;
            }
        }
        
        /// <summary>
        /// Gets a state/province collection by country identifier
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>State/province collection</returns>
        public virtual IList<StateProvince> GetStateProvincesByCountryId(int countryId, bool showHidden = false)
        {
            string key = string.Format(STATEPROVINCES_ALL_KEY, countryId);
            return _cacheManager.Get(key, () =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
                {
                    var query = from sp in _stateProvinceRepository.Table
                                orderby sp.DisplayOrder
                                where sp.CountryId == countryId &&
                                (showHidden || sp.Published)
                                select sp;
                    var stateProvinces = query.ToList();
                    return stateProvinces;
                }
            });
        }

        /// <summary>
        /// Inserts a state/province
        /// </summary>
        /// <param name="stateProvince">State/province</param>
        public virtual void InsertStateProvince(StateProvince stateProvince)
        {
            if (stateProvince == null)
                throw new ArgumentNullException("stateProvince");

            _stateProvinceRepository.Insert(stateProvince);

            _cacheManager.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(stateProvince);
        }

        /// <summary>
        /// Updates a state/province
        /// </summary>
        /// <param name="stateProvince">State/province</param>
        public virtual void UpdateStateProvince(StateProvince stateProvince)
        {
            if (stateProvince == null)
                throw new ArgumentNullException("stateProvince");

            _stateProvinceRepository.Update(stateProvince);

            _cacheManager.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(stateProvince);
        }


        public virtual District GetDistrictById(int districtId)
        {
            if (districtId == 0)
                return null;

            return _districtRepository.GetById(districtId);
        }

        public virtual void InsertDistrict(District district)
        {
            if (district == null)
                throw new ArgumentNullException("district");

            _districtRepository.Insert(district);

            //_cacheManager.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(district);
        }

       
        public virtual void UpdateDistrict(District district)
        {
            if (district == null)
                throw new ArgumentNullException("district");

            _districtRepository.Update(district);

            //_cacheManager.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(district);
        }

        public virtual void DeleteDistrict(District district)
        {
            if (district == null)
                throw new ArgumentNullException("district");

            _districtRepository.Delete(district);

            //_cacheManager.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(district);
        }

        #endregion

        public virtual IList<Ward> GetWardByDistrictId(int districtId)
        {
            if (districtId < 0)
                return null;

            return _cacheManager.Get(string.Format("Nop.wardbydistrictId-{0}", districtId), () =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }
                    ))
                {
                    var query = from w in _wardRepository.Table
                                where w.DistrictId == districtId
                                select w;
                    if (query != null)
                        return query.ToList();
                    return null;
                }
            });
        }

        public virtual IList<Street> GetStreetByDistrictId(int districtId)
        {
            if (districtId < 0)
                return null;

            return _cacheManager.Get(string.Format("Nop.streetbydistrictid-{0}", districtId), () =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                    ))
                {
                    var query = from s in _streetRepository.Table
                                where s.DistrictId == districtId
                                select s;
                    if (query != null)
                        return query.ToList();
                    return null;
                }
            });
        }
    }
}
