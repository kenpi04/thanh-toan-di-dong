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
              pass = MD5Encrypt.MD5Hash(pass);
             var q= _userRepository.Table.Where(x =>x.UserName == user&& x.Password==pass);
             if (q.Count()>0)
             {
                 return q.FirstOrDefault().Id;
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
