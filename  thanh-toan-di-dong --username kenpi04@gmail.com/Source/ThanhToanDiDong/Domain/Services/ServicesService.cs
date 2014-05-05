using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DAO;
using Domain.Entity;
using PagedList;

namespace Domain.Services
{
    public partial class ServicesService : BaseService
    {
        IRepository<Entity.Service> _serviceEventRepository = new EfRepository<Entity.Service>();
        public IPagedList<Entity.Service> GetPage(int pageIndex = 1, int pageSize = 15)
        {
            var q = _serviceEventRepository.Table;
            
            q.OrderBy(x=>x.Id);
            return q.ToPagedList(pageIndex, pageSize);
        }
        public IEnumerable<Entity.Service> GetAll(Func<Entity.Service, Boolean> where = null)
        {
            var q = _serviceEventRepository.Table;
            if (where != null)
                return q.Where(where);
            return q;

        }
        public void InsertOrUpdate(Entity.Service entity)
        {
            if (entity == null)
                throw new Exception("Service is null");
            if (entity.Id == 0)
                _serviceEventRepository.Insert(entity);
            else
                _serviceEventRepository.Update(entity);
        }
        public void Delete(Entity.Service entity)
        {
            if (entity == null)
                throw new Exception("Service is null");
            _serviceEventRepository.Delete(entity);
        }
      
    }
}
