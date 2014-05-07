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
        public Order GetById(int id)
        {
            return _orderRepository.GetById(id);
        }
        public IPagedList<Order> GetPage(int pageIndex = 1, int pageSize = 15)
        {
            var q = _orderRepository.Table.Where(x => !x.Deleted);
            
            q.OrderBy(x=>x.Id);
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
