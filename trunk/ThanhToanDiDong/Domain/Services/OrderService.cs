using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DAO;
using Domain.Entity;
using PagedList;

namespace Domain.Services
{
    public partial class OrderService : BaseService
    {
        IRepository<Order> _orderRepository = new EfRepository<Order>();
        IRepository<CardMobile> _CardmobileRepository = new EfRepository<CardMobile>();
        public Order GetById(int id)
        {
            return _orderRepository.GetById(id);
        }
        public IPagedList<Order> GetPage(int? cateId,bool isCard=true, DateTime? startDate=null,DateTime?endDate=null,OrderType?type=null, OrderStatusEnum? status=null, int pageIndex = 1, int pageSize = 15)
        {
            var q = _orderRepository.Table.Where(x => !x.Deleted);
            if (isCard)
                q = q.Where(x => x.OrderTypeId != (int)OrderType.BILLING);

            if (type.HasValue)
                q = q.Where(x => x.OrderTypeId == (int)type);
            if (cateId.HasValue)
            {
                var cate = _CardmobileRepository.Table.Where(x => x.CategoryCardMobileId == cateId).Select(x => x.Id);
                q = q.Where(x => cate.Contains(x.CardMobileId));
            }
            if (startDate.HasValue)
                q = q.Where(x => x.CreatedOn >= startDate.Value);
            if (endDate.HasValue)
                q = q.Where(x => x.CreatedOn <= endDate.Value);
            if (status.HasValue)
                q = q.Where(x => x.OrderStatusId == (int)status);            
           q=q.OrderByDescending(x=>x.CreatedOn);
            return q.ToPagedList(pageIndex, pageSize);
        }
      
        public IEnumerable<Order> GetAll(Func<Order, Boolean> where = null)
        {
            var q = _orderRepository.Table.Where(x => !x.Deleted);
            if (where != null)
                return q.Where(where);
            return q;

        }
        public void InsertOrUpdate(Order entity)
        {
            if (entity == null)
                throw new Exception("Order is null");
            if (entity.Id == 0)
                _orderRepository.Insert(entity);
            else
                _orderRepository.Update(entity);
        }
        public void Delete(Order entity)
        {
            if (entity == null)
                throw new Exception("Order is null");
            _orderRepository.Delete(entity);
        }
      
    }
}
