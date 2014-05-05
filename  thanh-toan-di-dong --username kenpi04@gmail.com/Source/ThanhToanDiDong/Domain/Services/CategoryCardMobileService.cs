using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DAO;
using Domain.Entity;
using PagedList;

namespace Domain.Services
{
    public partial class CategoryCardMobileService : BaseService
    {
        IRepository<CategoryCardMobile> _cardMobileCateRepository = new EfRepository<CategoryCardMobile>();
        public IPagedList<CategoryCardMobile> GetPage(int pageIndex = 1, int pageSize = 15)
        {
            var q = _cardMobileCateRepository.Table.Where(x => !x.Deleted);
            
            q.OrderBy(x=>x.Id);
            return q.ToPagedList(pageIndex, pageSize);
        }
        public IEnumerable<CategoryCardMobile> GetAll(Func<CategoryCardMobile, Boolean> where = null)
        {
            var q = _cardMobileCateRepository.Table.Where(x => !x.Deleted);
            if (where != null)
                return q.Where(where);
            return q;

        }
        public void InsertOrUpdate(CategoryCardMobile entity)
        {
            if (entity == null)
                throw new Exception("CategoryCardMobile is null");
            if (entity.Id == 0)
                _cardMobileCateRepository.Insert(entity);
            else
                _cardMobileCateRepository.Update(entity);
        }
        public void Delete(CategoryCardMobile entity)
        {
            if (entity == null)
                throw new Exception("CategoryCardMobile is null");
            _cardMobileCateRepository.Delete(entity);
        }
      
    }
}
