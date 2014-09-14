using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.DAO;

namespace Domain.Services
{
  public  class UserServices
    {
      IRepository<User> _UserRepository = new EfRepository<User>();
        public User GetByUserName(string username)
        {
            return _UserRepository.Table.Single(x=>x.Username==username);
        }
        public User Login(string username, string password)
        {
            string md5Pass = Domain.Unilities.MD5Encrypt.MD5Hash(password);
            var user = _UserRepository.Table.FirstOrDefault(x => username== x.Username && x.Password==md5Pass);
            return user;
        }
      
      
        
        public void InsertOrUpdate(User entity)
        {
            if (entity == null)
                throw new Exception("CardMobile is null");
            entity.Password = Domain.Unilities.MD5Encrypt.MD5Hash(entity.Password);
            if (entity.Id == 0)
                _UserRepository.Insert(entity);
            else
                _UserRepository.Update(entity);
        }
        public void Delete(User entity)
        {
            if (entity == null)
                throw new Exception("Cardmobile is null");
            _UserRepository.Delete(entity);
        }
    }
}
