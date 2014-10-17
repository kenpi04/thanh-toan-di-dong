using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Stores;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Stores;

namespace Nop.Services.Messages
{
    public partial class MessagesService : IMessagesService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        private const string MESSAGES_ALL_KEY = "Nop.message.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} :  name
        /// {1} : store ID
        /// </remarks>
        private const string MESSAGES_BY_NAME_KEY = "Nop.message.name-{0}-{1}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string MESSAGES_PATTERN_KEY = "Nop.message.";

        #endregion

        #region Fields

        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly ILanguageService _languageService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Ctor
        public MessagesService(ICacheManager cacheManager,
            IRepository<StoreMapping> storeMappingRepository,
            ILanguageService languageService,
            ILocalizedEntityService localizedEntityService,
            IStoreMappingService storeMappingService,
            IRepository<Message> messageRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._storeMappingRepository = storeMappingRepository;
            this._languageService = languageService;
            this._localizedEntityService = localizedEntityService;
            this._storeMappingService = storeMappingService;
            this._messageRepository = messageRepository;
            this._eventPublisher = eventPublisher;
        }
        #endregion

        #region Methods
        public void DeleteMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            _messageRepository.Delete(message);

            _cacheManager.RemoveByPattern(MESSAGES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(message);
        }
        
        public void InsertMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            _messageRepository.Insert(message);

            _cacheManager.RemoveByPattern(MESSAGES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(message);
        }
        
        public void UpdateMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            _messageRepository.Update(message);

            _cacheManager.RemoveByPattern(MESSAGES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(message);
        }

        public List<Message> GetMessageByCustomerId(int CustomerId)
        {
            var query = from t in _messageRepository.Table
                        where t.CustomerId == CustomerId && !t.Deleted
                    select t;
            return query.ToList();
        }
        public Message GetMessageById(int Id)
        {
            var query = from t in _messageRepository.Table
                        where t.Id == Id && !t.Deleted
                        select t;
            return query.FirstOrDefault();
        }

        public List<Message> GetMessage( DateTime? FromDate, DateTime? ToDate ,int Type = 0, string Key = "")
        {
            var query = from t in _messageRepository.Table   
                        where (!t.Deleted)
                        select t;
            if (Type > 0)
                query = query.Where(x => x.Type == Type);
            if(!String.IsNullOrEmpty(Key))
                query = query.Where(x => x.Name.Contains(Key));
            if (FromDate.HasValue && FromDate.Value.Year > 2000)
                query = query.Where(x => x.CreatedOn >= FromDate.Value);
            if (ToDate.HasValue && ToDate.Value.Year > 2000)
                query = query.Where(x => x.CreatedOn <= ToDate.Value);            
            return query.ToList();
        }

        public IPagedList<Message> GetMessage(int page, int pagesize,DateTime? FromDate, DateTime? ToDate, int Type = 0, string Key = "", int CustomerId = 0 )
        {
            var query = from t in _messageRepository.Table
                        where (!t.Deleted)
                        select t;
            if (Type > 0)
                query = query.Where(x => x.Type == Type);
            if (CustomerId > 0)
                query = query.Where(x => x.CustomerId == CustomerId);
            if (!String.IsNullOrEmpty(Key))
                query = query.Where(x => x.Name.Contains(Key));
            if (FromDate.HasValue && FromDate.Value.Year > 2000)
                query = query.Where(x => x.CreatedOn >= FromDate.Value);
            if (ToDate.HasValue && ToDate.Value.Year > 2000)
                query = query.Where(x => x.CreatedOn <= ToDate.Value);
            
            return new PagedList<Message>(query.ToList(),page -1,pagesize);
        }
         #endregion

    }
}
