using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DAO;
using Domain.Entity;
using PagedList;

namespace Domain.Services
{
    public partial class OrderNoteService : BaseService
    {
        IRepository<OrderNote> _orderNoteRepository = new EfRepository<OrderNote>();
        public IPagedList<OrderNote> GetPage(int pageIndex = 1, int pageSize = 15)
        {
            var q = _orderNoteRepository.Table;
            
            q.OrderBy(x=>x.Id);
            return q.ToPagedList(pageIndex, pageSize);
        }
        public IEnumerable<OrderNote> GetAll(Func<OrderNote, Boolean> where = null)
        {
            var q = _orderNoteRepository.Table;
            if (where != null)
                return q.Where(where);
            return q;

        }
        public void InsertOrUpdate(OrderNote entity)
        {
            if (entity == null)
                throw new Exception("OrderNote is null");
            if (entity.Id == 0)
                _orderNoteRepository.Insert(entity);
            else
                _orderNoteRepository.Update(entity);
        }
        public void Delete(OrderNote entity)
        {
            if (entity == null)
                throw new Exception("OrderNote is null");
            _orderNoteRepository.Delete(entity);
        }
      
    }
}
