using System;
using System.Collections.Generic;
using System.Linq;
using PlanX.Core;
using PlanX.Core.Data;
using PlanX.Core.Domain.Stores;
using PlanX.Core.Domain.ClickBay;
using PlanX.Services.Events;
using PlanX.Services.ClickBay;

namespace PlanX.Services.ClickBay
{
    /// <summary>
    /// TicketConcession service
    /// </summary>
    public partial class TicketConcessionService : ITicketConcessionService
    {
        #region Fields

        private readonly IRepository<TicketConcession> _ticketConcessionRepository;
        private readonly IRepository<TicketType> _ticketTypeRepository;
        private readonly IRepository<Place> _placeRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public TicketConcessionService(IRepository<TicketConcession> TicketConcessionRepository,
            IRepository<TicketType> ticketTypeRepository,
            IRepository<Place> placeRepository,

            IRepository<StoreMapping> storeMappingRepository,
            IEventPublisher eventPublisher)
        {
            this._ticketConcessionRepository = TicketConcessionRepository;
            this._ticketTypeRepository = ticketTypeRepository;
            this._placeRepository = placeRepository;

            this._storeMappingRepository = storeMappingRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        #region TicketConcession
        /// <summary>
        /// Deletes a TicketConcession
        /// </summary>
        /// <param name="TicketConcession">TicketConcession item</param>
        public virtual void DeleteTicketConcession(TicketConcession ticketConcession)
        {
            if (ticketConcession == null)
                throw new ArgumentNullException("TicketConcession");

            ticketConcession.Deleted = true;
            _ticketConcessionRepository.Update(ticketConcession);

            //event notification
            _eventPublisher.EntityDeleted(ticketConcession);
        }

        /// <summary>
        /// Gets a TicketConcession
        /// </summary>
        /// <param name="TicketConcessionId">The TicketConcession identifier</param>
        /// <returns>TicketConcession</returns>
        public virtual TicketConcession GetTicketConcessionById(int TicketConcessionId)
        {
            if (TicketConcessionId == 0)
                return null;
            var query = from tic in _ticketConcessionRepository.Table
                        where (!tic.Deleted) && tic.Id == TicketConcessionId
                        select tic;
            return query.FirstOrDefault();
        }


        public virtual IPagedList<TicketConcession> GetAllTicketConcession(int pageIndex, int pageSize)
        {
            var query = from tic in _ticketConcessionRepository.Table
                        where (!tic.Deleted)
                        orderby tic.CreatedOnUtc descending
                        select tic;

            var TicketConcession = new PagedList<TicketConcession>(query, pageIndex, pageSize);
            return TicketConcession;
        }

        public virtual IPagedList<TicketConcession> SearchTicketConcession(int pageIndex, int pageSize, string PassengerNameSearch = "", string FromPlaceSearch = "", string ToPlaceSearch = "", string TicketTypeSearch = "", string DepartDateSearch = "")
        {
            DateTime _departDateStart;            

            var query = from tic in _ticketConcessionRepository.Table
                        where (!tic.Deleted)
                        select tic;
            if (!string.IsNullOrEmpty(PassengerNameSearch))
                query = query.Where(x => x.PassengerName.ToLower().Contains(PassengerNameSearch.ToLower()));
            if (!string.IsNullOrEmpty(FromPlaceSearch))
                query = query.Where(x => x.FromPlace.ToLower().Contains(FromPlaceSearch.ToLower()));
            if (!string.IsNullOrEmpty(ToPlaceSearch))
                query = query.Where(x => x.ToPlace.ToLower().Contains(ToPlaceSearch.ToLower()));
            if (!string.IsNullOrEmpty(TicketTypeSearch))
                query = query.Where(x => x.TicketType.ToLower().Contains(TicketTypeSearch.ToLower()));

            if (DateTime.TryParse(DepartDateSearch, out _departDateStart))
            {
                DateTime? _departDateEnd = null;
                _departDateEnd = _departDateStart.AddDays(1);

                query = query.Where(x => x.DepartDate > _departDateStart && (_departDateEnd.HasValue || x.DepartDate < _departDateEnd.Value));
            }

            query = query.OrderByDescending(t => t.DepartDate);
                

            var TicketConcession = new PagedList<TicketConcession>(query, pageIndex, pageSize);
            return TicketConcession;
        }

        /// <summary>
        /// Inserts a TicketConcession item
        /// </summary>
        /// <param name="TicketConcession">TicketConcession item</param>
        public virtual void InsertTicketConcession(TicketConcession TicketConcession)
        {
            if (TicketConcession == null)
                throw new ArgumentNullException("TicketConcession");

            _ticketConcessionRepository.Insert(TicketConcession);

            //event notification
            _eventPublisher.EntityInserted(TicketConcession);
        }

        /// <summary>
        /// Updates the TicketConcession item
        /// </summary>
        /// <param name="TicketConcession">TicketConcession item</param>
        public virtual void UpdateTicketConcession(TicketConcession TicketConcession)
        {
            if (TicketConcession == null)
                throw new ArgumentNullException("TicketConcession");

            _ticketConcessionRepository.Update(TicketConcession);

            //event notification
            _eventPublisher.EntityUpdated(TicketConcession);
        }


        #endregion

        #region TicketType
        /// <summary>
        /// Deletes a TicketType
        /// </summary>
        /// <param name="TicketType">TicketType item</param>
        public virtual void DeleteTicketType(TicketType TicketType)
        {
            if (TicketType == null)
                throw new ArgumentNullException("TicketType");

            _ticketTypeRepository.Delete(TicketType);

            //event notification
            _eventPublisher.EntityDeleted(TicketType);
        }

        /// <summary>
        /// Gets a TicketType
        /// </summary>
        /// <param name="TicketTypeId">The TicketType identifier</param>
        /// <returns>TicketType</returns>
        public virtual TicketType GetTicketTypeById(int TicketTypeId)
        {
            if (TicketTypeId == 0)
                return null;

            return _ticketTypeRepository.GetById(TicketTypeId);
        }


        public virtual List<string> GetAllTicketType()
        {
            var query = _ticketTypeRepository.Table;
            return query.OrderBy(x => x.TicketTypeName).Select(x => x.TicketTypeName).ToList();
        }

        public virtual List<TicketType> GetAllType()
        {
            var query = _ticketTypeRepository.Table;
            return query.ToList();
        }

        /// <summary>
        /// Inserts a TicketType item
        /// </summary>
        /// <param name="TicketType">TicketType item</param>
        public virtual void InsertTicketType(TicketType TicketType)
        {
            if (TicketType == null)
                throw new ArgumentNullException("TicketType");

            _ticketTypeRepository.Insert(TicketType);

            //event notification
            _eventPublisher.EntityInserted(TicketType);
        }

        /// <summary>
        /// Updates the TicketType item
        /// </summary>
        /// <param name="TicketType">TicketType item</param>
        public virtual void UpdateTicketType(TicketType TicketType)
        {
            if (TicketType == null)
                throw new ArgumentNullException("TicketType");

            _ticketTypeRepository.Update(TicketType);

            //event notification
            _eventPublisher.EntityUpdated(TicketType);
        }


        #endregion

        #region Place
        /// <summary>
        /// Deletes a Place
        /// </summary>
        /// <param name="Place">Place item</param>
        public virtual void DeletePlace(Place Place)
        {
            if (Place == null)
                throw new ArgumentNullException("Place");

            _placeRepository.Delete(Place);

            //event notification
            _eventPublisher.EntityDeleted(Place);
        }

        /// <summary>
        /// Gets a Place
        /// </summary>
        /// <param name="PlaceId">The Place identifier</param>
        /// <returns>Place</returns>
        public virtual Place GetPlaceById(int PlaceId)
        {
            if (PlaceId == 0)
                return null;

            return _placeRepository.GetById(PlaceId);
        }

        public virtual List<Place> GetAllPlaceToAdmin()
        {
            var query = _placeRepository.Table;
            return query.ToList();
        }

        public virtual List<string> GetAllPlace()
        {
            var query = _placeRepository.Table;
            return query.OrderBy(x => x.PlaceName).Select(x => x.PlaceName).ToList();
        }

        /// <summary>
        /// Inserts a Place item
        /// </summary>
        /// <param name="Place">Place item</param>
        public virtual void InsertPlace(Place Place)
        {
            if (Place == null)
                throw new ArgumentNullException("Place");

            _placeRepository.Insert(Place);

            //event notification
            _eventPublisher.EntityInserted(Place);
        }

        /// <summary>
        /// Updates the Place item
        /// </summary>
        /// <param name="Place">Place item</param>
        public virtual void UpdatePlace(Place Place)
        {
            if (Place == null)
                throw new ArgumentNullException("Place");

            _placeRepository.Update(Place);

            //event notification
            _eventPublisher.EntityUpdated(Place);
        }


        #endregion

        #endregion
    }
}
