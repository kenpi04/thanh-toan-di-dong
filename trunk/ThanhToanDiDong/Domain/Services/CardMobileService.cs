using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DAO;
using Domain.Entity;
using PagedList;

namespace Domain.Services
{
    public partial  class CardMobileService:BaseService
    {
        IRepository<CardMobile> _cardMobileRepository = new EfRepository<CardMobile>();
        public CardMobile GetById(int id)
        {
            return _cardMobileRepository.GetById(id);
        }

        public IPagedList<CardMobile> GetPage( int pageIndex = 1, int pageSize = 15)
        {
            var q = _cardMobileRepository.Table.Where(x=>!x.Deleted);
            
            q = q.OrderBy(x=>x.Id);
            return q.ToPagedList(pageIndex, pageSize);
        }
        public IEnumerable<CardMobile> GetAll(Func<CardMobile,Boolean> where=null)
        {
            var q = _cardMobileRepository.Table.Where(x => !x.Deleted);
            if (where != null)
                return q.Where(where);
            return q;

        }
        public void InsertOrUpdate(CardMobile entity)
        {
            if (entity == null)
                throw new Exception("CardMobile is null");
            if (entity.Id == 0)
                _cardMobileRepository.Insert(entity);
            else
                _cardMobileRepository.Update(entity);
        }
        public void Delete(CardMobile entity)
        {
            if (entity == null)
                throw new Exception("Cardmobile is null");
            _cardMobileRepository.Delete(entity);
        }
      
    }
}
