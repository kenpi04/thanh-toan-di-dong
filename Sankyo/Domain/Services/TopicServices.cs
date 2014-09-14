using Domain.DAO;
using Domain.Entity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
  public  class TopicServices
    {
        IRepository<Topic> _TopicRepository = new EfRepository<Topic>();
        public Topic GetById(int id)
        {
            return _TopicRepository.GetById(id);
        }
        public Topic GetByName(string name)
        {
            return _TopicRepository.Table.FirstOrDefault(x => x.Name.Equals(name));
        }
        public IPagedList<Topic> GetPage(int pageIndex = 1, int pageSize = 15)
        {
            var q = _TopicRepository.Table;

            q=q.OrderBy(x => x.Id);
            return q.ToPagedList(pageIndex, pageSize);
        }
        public void InsertOrUpdate(Topic entity)
        {
            if (entity == null)
                throw new Exception("_Topic is null");
            if (entity.Id == 0)
                _TopicRepository.Insert(entity);
            else
                _TopicRepository.Update(entity);
        }
        public void Delete(Topic entity)
        {
            if (entity == null)
                throw new Exception("Cardmobile is null");
            _TopicRepository.Delete(entity);
        }
    }
}
