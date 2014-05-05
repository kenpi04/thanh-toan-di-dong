using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DAO;
using Domain.Entity;
using PagedList;

namespace Domain.Services
{
    public partial class ProviderService : BaseService
    {
        IRepository<Provider> _providerRepository = new EfRepository<Provider>();
        public IPagedList<Provider> GetPage(int pageIndex = 1, int pageSize = 15)
        {
            var q = _providerRepository.Table;
            
            q.OrderBy(x=>x.Id);
            return q.ToPagedList(pageIndex, pageSize);
        }
        public IEnumerable<Provider> GetAll(Func<Provider, Boolean> where = null)
        {
            var q = _providerRepository.Table;
            if (where != null)
                return q.Where(where);
            return q;

        }
        public void InsertOrUpdate(Provider entity)
        {
            if (entity == null)
                throw new Exception("Provider is null");
            if (entity.Id == 0)
                _providerRepository.Insert(entity);
            else
                _providerRepository.Update(entity);
        }
        public void Delete(Provider entity)
        {
            if (entity == null)
                throw new Exception("Provider is null");
            _providerRepository.Delete(entity);
        }
      
    }
}
