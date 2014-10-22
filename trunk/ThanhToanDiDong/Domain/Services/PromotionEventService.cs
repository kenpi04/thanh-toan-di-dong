using Domain.DAO;
using Domain.Entity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services
{
    public class PromotionEventService : BaseService
    {
        IRepository<PromotionEvent> _promotionEventRepository = new EfRepository<PromotionEvent>();
        public PromotionEvent GetById(int id)
        {
            return _promotionEventRepository.GetById(id);
        }

        public IPagedList<PromotionEvent> GetPage(DateTime? startDate, DateTime? endDate, int pageIndex = 1, int pageSize = 15)
        {
            var q = _promotionEventRepository.Table.Where(x => !x.Deleted);
            if (startDate.HasValue) q = q.Where(x => x.CreatedOn >= startDate.Value);
            if (endDate.HasValue) q = q.Where(x => x.CreatedOn <= endDate.Value);

            q = q.OrderByDescending(x => x.CreatedOn);
            return q.ToPagedList(pageIndex, pageSize);
        }
        public IEnumerable<PromotionEvent> GetAll(Func<PromotionEvent, Boolean> where = null)
        {
            var q = _promotionEventRepository.Table.Where(x => !x.Deleted);
            if (where != null)
                return q.Where(where);
            q = q.OrderByDescending(x => x.CreatedOn);
            return q;

        }
        public void InsertOrUpdate(PromotionEvent entity)
        {
            if (entity == null)
                throw new Exception("PromotionEvent is null");
            if (entity.Id == 0)
                _promotionEventRepository.Insert(entity);
            else
                _promotionEventRepository.Update(entity);
        }
        public void Delete(PromotionEvent entity)
        {
            if (entity == null)
                throw new Exception("PromotionEvent is null");
            _promotionEventRepository.Delete(entity);
        }
    }
}
