using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DAO;
using Domain.Entity;
using Domain.Unilities;

namespace Domain.Services
{
     public class UserService:BaseService
    {
         IRepository<User> _userRepository = new EfRepository<User>();

         public int Login(string user, string pass)
         {
             var q= _userRepository.Table.FirstOrDefault(x => string.Compare(user, x.UserName, false) == 1 && x.Password.Equals(MD5Encrypt.MD5Hash(pass)));
             if (q != null)
             {
                 return q.Id;
             }
             return -1;
         }
         public void InsertOrUpdate(User entity)
         {
             if (entity.Id == 0)
                 _userRepository.Insert(entity);
             else
                 _userRepository.Update(entity);
         }
    }
}
