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
    public class TopicServices
    {
        IRepository<Topic> _TopicRepository = new EfRepository<Topic>();
        public Topic GetById(int id)
        {
            return _TopicRepository.GetById(id);
        }
        public Topic GetByName(string name, int languageId = 0)
        {
            var topic = new Topic();
            if (languageId > 0)
                topic = _TopicRepository.Table.FirstOrDefault(x => x.Name.Equals(name) & x.LanguageId == languageId);
            if (topic == null)
                topic = _TopicRepository.Table.FirstOrDefault(x => x.Name.Equals(name));
            return topic;
        }
        public IEnumerable<Topic> GetPage()
        {
            var q = _TopicRepository.Table;

            q = q.OrderBy(x => x.Id);
            return q;
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
